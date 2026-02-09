/*
* ==============================================================================
*
* FileName: SetupSwagger.cs
* Created: 2025/12/25 17:00:24
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using TableConfig.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TableConfig.Setup
{
    public static class SetupSwagger
    {
        public static void AddSwaggerSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSwaggerGen(c =>
            {
                // 添加自定义 header
                c.AddSecurityDefinition("custom-header", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Token",
                    Type = SecuritySchemeType.ApiKey,
                    Description = "自定义token授权",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "custom-header"
                            }
                        },
                        new string[] {}
                    }
                });

                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "My API",
                    Version = "v1"
                });

                c.SchemaFilter<DisplaySchemaFilter>();
            });

        }

        public static void UseSwaggerSetup(this IApplicationBuilder app)
        {
            app.UseSwagger(o =>
            {
                o.RouteTemplate = "api-docs/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "api-docs";
                c.SwaggerEndpoint("/api-docs/v1/swagger.json", "API v1");
            });
        }
    }


    public class DisplaySchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.MemberInfo == null) return;

            //把参数描述修改为DisplayAttribute的Name内容
            var displayAttribute = context.MemberInfo.GetCustomAttribute<DisplayAttribute>();
            if (displayAttribute != null)
            {
                schema.Description = displayAttribute.Name;
            }


        }
    }
}
