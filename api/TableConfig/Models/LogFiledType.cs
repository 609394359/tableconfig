/*
* ==============================================================================
*
* FileName: LogFiledType.cs
* Created: 2026/1/2 14:14:05
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace TableConfig.Models
{
    /// <summary>
    /// 数据字段类型
    /// </summary>
    public struct LogFiledType
    {
        public static string String = "string";
        public static string Datetime = "datetime";
        public static string Int = "int";


        public static IEnumerable<SelectListItem> ToList()
        {
            yield return new SelectListItem { Text = "字符串", Value = String };
            yield return new SelectListItem { Text = "日期", Value = Datetime };
            yield return new SelectListItem { Text = "数字", Value = Int };
        }
    }
}
