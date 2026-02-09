/*
* ==============================================================================
*
* FileName: ApiResult.cs
* Created: 2026/1/1 16:37:48
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using TableConfig.Attributes;
using TableConfig.Enums;

namespace TableConfig.Models
{
    public class ApiResult
    {
        public ApiResult()
        {
            this.Code = (int)StatusCodeType.Success;
            this.Message = StatusCodeType.Success.ToText();
        }
        public ApiResult(StatusCodeType statusCode)
        {
            this.Code = (int)statusCode;
            this.Message = statusCode.ToText();
        }
        public ApiResult(StatusCodeType statusCode, string message)
        {
            this.Code = (int)statusCode;
            this.Message = message;
        }
        public ApiResult(int statusCode, string message)
        {
            this.Code = statusCode;
            this.Message = message;
        }

        public int Code { get; set; }
        public string Message { get; set; }
    }
    public class ApiResult<T> : ApiResult
    {
        public ApiResult() : base()
        {
        }
        public ApiResult(StatusCodeType statusCode) : base(statusCode)
        {
        }
        public ApiResult(StatusCodeType statusCode, string message) : base(statusCode, message)
        {
        }
        public ApiResult(StatusCodeType statusCode, string message, T data) : base(statusCode, message)
        {
            Data = data;
        }
        public ApiResult(T data) : base(StatusCodeType.Success)
        {
            Data = data;
        }
        public T Data { get; set; }
    }
}
