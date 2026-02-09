/*
* ==============================================================================
*
* FileName: StatusCodeType.cs
* Created: 2026/1/20 14:29:02
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using TableConfig.Attributes;

namespace TableConfig.Enums
{
    public enum StatusCodeType
    {

        [Text("请求(或处理)成功")]
        Success = 200,

        [Text("内部请求出错")]
        Error = 500,

        [Text("请求参数不完整或不正确")]
        ParameterError = 400,

        [Text("访问请求未授权! 当前 SESSION 失效, 请重新登陆")]
        Unauthorized = 401,

        [Text("用户未登录")]
        NotLogin = 402,

        [Text("您无权进行此操作，请求执行已拒绝")]
        Forbidden = 403,

        [Text("找不到与请求匹配的 HTTP 资源")]
        NotFound = 404,

        [Text("HTTP请求类型不合法")]
        HttpMehtodError = 405,

        [Text("HTTP请求不合法,请求参数可能被篡改")]
        HttpRequestError = 406,

        [Text("该URL已经失效")]
        URLExpireError = 407,
    }
}
