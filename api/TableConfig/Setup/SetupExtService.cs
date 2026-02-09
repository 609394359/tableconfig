/*
* ==============================================================================
*
* FileName: SetupExtService.cs
* Created: 2025/12/25 16:51:01
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using Autofac.Core;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using TableConfig.Services;

namespace TableConfig.Setup
{
    public static class SetupExtService
    {
        public static void AddExtServiceSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //注入实体映射服务
            services.AddScoped<IMapper, ServiceMapper>();


            services.AddSingleton<CacheContext>();
            services.AddScoped<TokenManager>();

        }
    }
}
