using Microsoft.AspNetCore.Mvc;
using RestApiSample.Services;

namespace RestApiSample.Interfaces
{
    public interface IFormatResponseService
    {
        public TFormatResponseService getObject();

        public IActionResult GetActionResult();

    }
}