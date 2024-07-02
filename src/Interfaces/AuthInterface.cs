using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using RestApiSample.Models;

namespace RestApiSample.Interfaces
{

    public class ILogin
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class IRegister
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;

        public string Address { get; set; } = null!;

    }

    public interface IAuthService
    {
        public IFormatResponseService Login([FromBody] ILogin user);

        // private string createToken(User user);

    }

}




