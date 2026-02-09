/*
* ==============================================================================
*
* FileName: GlobalActionMonitor.cs
* Created: 2026/1/1 17:20:29
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using TableConfig.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace TableConfig.Global
{
    public class GlobalActionMonitor : Attribute, IActionFilter, IResultFilter
    {
        public GlobalActionMonitor()
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            #region 模型验证
            if (context.ModelState.IsValid) return;

            ApiResult response = new ApiResult();
            response.Code = 500;

            foreach (var item in context.ModelState.Values)
            {
                foreach (var error in item.Errors)
                {
                    if (!string.IsNullOrEmpty(response.Message))
                    {
                        response.Message += " | ";
                    }

                    response.Message += error.ErrorMessage;
                }
            }

            context.Result = new JsonResult(response);
            #endregion
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
        }
    }
}
