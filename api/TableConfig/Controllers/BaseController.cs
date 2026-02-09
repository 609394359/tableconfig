using Microsoft.AspNetCore.Mvc;
using TableConfig.Enums;
using TableConfig.Models;

namespace TableConfig.Controllers
{
    public class BaseController : ControllerBase
    {
        protected ApiResult<T> ToResult<T>(T data)
        {
            return new ApiResult<T>(data);
        }
        protected ApiResult ToResult(StatusCodeType statusCode)
        {
            return new ApiResult(statusCode);
        }
        protected ApiResult ToResult(StatusCodeType statusCode, string message)
        {
            return new ApiResult(statusCode, message);
        }
    }
}
