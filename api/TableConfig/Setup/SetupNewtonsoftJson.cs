/*
* ==============================================================================
*
* FileName: SetupNewtonsoftJson.cs
* Created: 2025/12/25 15:22:37
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using TableConfig.Converters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TableConfig.Setup
{
    public class SetupNewtonsoftJson
    {
        public static void Setup(MvcNewtonsoftJsonOptions option)
        {
            // 忽略循环引用
            option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            // 不使用驼峰
            //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            // 设置时间格式
            option.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            // 如字段为null值，该字段不会返回到前端
            //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

            option.SerializerSettings.Converters.Add(new LongTypeConverter());

        }
    }
}
