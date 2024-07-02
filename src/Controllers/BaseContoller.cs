using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace RestApiSample.Controllers
{

    public class BaseController : ControllerBase
    {
        private const string BearerPrefix = "Bearer ";

        private string? getToken()
        {

            if (Request.Headers.TryGetValue("Authorization", out StringValues headerValue))
            {
                string token = headerValue!;

                if (!string.IsNullOrEmpty(token) && token.StartsWith(BearerPrefix, StringComparison.InvariantCultureIgnoreCase))
                {

                    token = token.Substring(BearerPrefix.Length);
                }


                return token;
            }

            return null;
        }

        private JwtSecurityToken deCodeJwt()
        {
            var token = getToken();
            var jwt = new JwtSecurityToken(jwtEncodedString: token);
            return jwt;
        }

        protected string getJwtPayload(string type)
        {
            var jwt = deCodeJwt();
            return jwt.Claims.First(e => e.Type == type).Value;
        }

    }


}