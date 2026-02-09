/*
* ==============================================================================
*
* FileName: AuthorizationAttribute.cs
* Created: 2026/1/8 11:38:25
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using TableConfig.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using TableConfig.Services;

namespace TableConfig.Attributes
{
    /// <summary>
    /// 登录验证过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            #region 判断是否登录
            var _tokenManager = context.HttpContext.RequestServices.GetService<TokenManager>();

            if (!_tokenManager.IsLogin())
            {
                ApiResult response = new ApiResult
                {
                    Code = 401,
                    Message = "登录已过期"
                };

                context.Result = new JsonResult(response) { StatusCode = 200 };
                return;
            }

            #endregion
        }
    }
}
