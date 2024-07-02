using AuthenticationPlugin;
using RestApiSample.Interfaces;
using JsonFlatFileDataStore;
using Microsoft.AspNetCore.Mvc;
using RestApiSample.Services;

namespace RestApiSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly UserService _userService;
        private readonly AuthService _auth;
        private IConfiguration _configuration;

        private AuthCustomService _authService;
        public AuthController(ILogger<UserController> logger, UserService userService, IConfiguration configuration, AuthCustomService authCustomService)
        {
            _logger = logger;
            var store = new DataStore("db.json");
            _userService = userService;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
            _authService = authCustomService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] ILogin user)
        {
            var result = _authService.Login(user);
            return result.GetActionResult();
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] IRegister user)
        {
            var result = await _authService.register(user);
            return result.GetActionResult();
        }

    }

}