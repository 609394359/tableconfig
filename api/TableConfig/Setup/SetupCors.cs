/*
* ==============================================================================
*
* FileName: SetupCors.cs
* Created: 2025/12/25 16:50:11
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using TableConfig.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace TableConfig.Setup
{
    public static class SetupCors
    {
        public static void AddCorsSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddCors(c =>
            {
                c.AddPolicy("LimitRequests", policy =>
                {
                    policy
                    .WithOrigins(AppSettings.Configuration["Startup:AllowOrigins"].Split('|'))
                    .AllowAnyHeader()//Ensures that the policy allows any header.
                    .AllowAnyMethod();

                    policy.WithExposedHeaders("Content-Disposition");
                });
            });
        }

        public static void UseCorsSetup(this IApplicationBuilder app)
        {
            app.UseCors("LimitRequests");
        }
    }
}
