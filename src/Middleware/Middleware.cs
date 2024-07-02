using Microsoft.AspNetCore.Authorization;
using RestApiSample.Models;

namespace RestApiSample.Middleware
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public CustomAuthorizeAttribute(Roles roleEnum)
        {
            Roles = roleEnum.ToString().Replace(" ", string.Empty);
        }
    }

}