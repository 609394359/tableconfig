/*
* ==============================================================================
*
* FileName: AppSettings.cs
* Created: 2026/1/2 9:06:50
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using Microsoft.Extensions.Configuration;
using System.IO;
using System;

namespace TableConfig.Utils
{
    /// <summary>
    /// appsettings.json操作类
    /// </summary>
    public class AppSettings
    {
        public static IConfiguration Configuration { get; set; }


        public static string EnvironmentName { get; set; }


        static AppSettings()
        {
            EnvironmentName = Environment.GetEnvironmentVariables()["ASPNETCORE_ENVIRONMENT"]?.ToString() ?? "Production";

            var path = Path.Combine(AppContext.BaseDirectory, "Config");
            //ReloadOnChange = true 当appsettings.json被修改时重新加载
            Configuration = new ConfigurationBuilder()
            .AddJsonFile($"{path}/appsettings.{EnvironmentName}.json", optional: true, reloadOnChange: true)
            .Build();
        }

        /// <summary>
        /// 获得配置文件的对象值
        /// </summary>
        /// <param name="jsonPath"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetJson(string jsonPath, string key)
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile(jsonPath).Build(); //json文件地址
            string s = config.GetSection(key).Value; //json某个对象
            return s;
        }
    }
}
