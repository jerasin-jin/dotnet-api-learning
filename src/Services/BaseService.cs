using Microsoft.AspNetCore.Mvc;
using RestApiSample.Interfaces;

namespace RestApiSample.Services
{

    public enum DefaultStatus
    {
        NotFound,
        Success,
        BadRequest,
        Unknown
    }

    public class TFormatResponseService
    {
        public object? value { get; set; }
        public string status { get; set; } = null!;
    }

    public class FormatResponseService : ControllerBase, IFormatResponseService
    {

        public object? _value { get; set; }

        public DefaultStatus _status { get; set; }

        public TFormatResponseService getObject()
        {
            return new TFormatResponseService
            {
                status = this._status.ToString(),
                value = this._value
            };
        }

        public IActionResult GetActionResult()
        {
            switch (_status)
            {
                case DefaultStatus.NotFound:
                    return NotFound(new TFormatResponseService
                    {
                        status = this._status.ToString(),
                        value = this._value
                    });
                case DefaultStatus.Success:
                    return Ok(new TFormatResponseService
                    {
                        status = this._status.ToString(),
                        value = this._value
                    });
                case DefaultStatus.BadRequest:
                    return BadRequest(new TFormatResponseService
                    {
                        status = this._status.ToString(),
                        value = this._value
                    });
                default:
                    return StatusCode(500);
            }
        }


    }

}
