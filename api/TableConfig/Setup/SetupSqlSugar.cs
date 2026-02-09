/*
* ==============================================================================
*
* FileName: SetupSqlSugar.cs
* Created: 2026/1/19 16:26:05
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using TableConfig.Models.Entitys;
using TableConfig.Utils;

namespace TableConfig.Setup
{
    public static class SetupSqlSugar
    {
        public static void AddSqlsugarSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var s = AppSettings.Configuration["DbConnection:ConnectionString"];

            services.AddScoped<ISqlSugarClient>(x =>
            {
                return new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = AppSettings.Configuration["DbConnection:ConnectionString"],
                    DbType = DbType.PostgreSQL,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute,
                    MoreSettings = new ConnMoreSettings
                    {
                        // 设为 true (默认值): SqlSugar 会把 C# 的 UserName 自动转成小写 username 去查
                        // 设为 false: SqlSugar 会生成带引号的 "UserName" 去查 (要求数据库字段必须真的是大写)
                        PgSqlIsAutoToLower = false
                    }
                },
                db => {
                    db.Aop.OnLogExecuting = (sql, parm) =>
                    {
                        if (AppSettings.EnvironmentName == "Development")
                        {
                            Console.WriteLine(sql);
                        }
                    };
                    db.QueryFilter.AddTableFilter<IDeleted>(p => p.IsDeleted == false);
                });
            });
        }
    }
}
