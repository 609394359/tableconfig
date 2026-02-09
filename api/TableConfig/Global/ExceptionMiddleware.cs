/*
* ==============================================================================
*
* FileName: ExceptionMiddleware.cs
* Created: 2026/1/1 17:24:23
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;
using TableConfig.Models;
using TableConfig.Utils;

namespace TableConfig.Global
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Tip ex)
            {
                context.Response.StatusCode = 200;
                var response = new ApiResult(ex.Code, ex.Message);

                await context.Response.WriteAsJsonAsync(response);

            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                var response = new ApiResult(500, ex.Message);

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
