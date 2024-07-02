using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthenticationPlugin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RestApiSample.Interfaces;
using RestApiSample.Models;


namespace RestApiSample.Services
{

    public class AuthCustomService : IAuthService
    {

        private readonly UserService _userService;
        private IConfiguration _configuration;
        private readonly FormatResponseService _formatResponseService;


        public AuthCustomService(UserService userService, IConfiguration configuration, FormatResponseService formatResponseService)
        {
            _userService = userService;
            _configuration = configuration;
            _formatResponseService = formatResponseService;
        }

        public IFormatResponseService Login([FromBody] ILogin user)
        {
            var getUser = _userService.getUserByEmail(user.Email);
            var result = getUser;

            if (result.getObject().status != DefaultStatus.Success.ToString())
            {
                result.getObject().value = null;
                return result;
            }

            var findUser = getUser.getObject().value as User;
            if (findUser == null || !SecurePasswordHasherHelper.Verify(user.Password, findUser.Password))
            {
                _formatResponseService._status = DefaultStatus.BadRequest;
                _formatResponseService._value = null;
                return _formatResponseService;
            }

            var jwt = createToken(findUser);

            _formatResponseService._status = DefaultStatus.Success;
            _formatResponseService._value = new { token = jwt };
            return _formatResponseService;
        }


        private string createToken(User user)
        {
            var claims = new Claim[]{
            new Claim("id",user.Id.ToString()),
             new Claim("role",user.Role),
            new Claim(JwtRegisteredClaimNames.Email,user.Email),
            new Claim(JwtRegisteredClaimNames.Iss,"localhost.com"),
            new Claim(JwtRegisteredClaimNames.Aud,"localhost.com"),
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Tokens:Key").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }


        public async Task<IFormatResponseService> register(IRegister user)
        {
            var createUser = await _userService.createUser(user);
            return createUser;
        }
    }




}