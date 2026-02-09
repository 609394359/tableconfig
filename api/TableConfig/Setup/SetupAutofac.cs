/*
* ==============================================================================
*
* FileName: SetupAutofac.cs
* Created: 2025/12/25 10:36:59
* Author: yangqingde
* Description: 
*
* ==============================================================================
*/
using Autofac;
using Autofac.Extras.DynamicProxy;
using System.Reflection;

namespace TableConfig.Setup
{
    public class SetupAutofac
    {
        public static void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(t => t.Name.EndsWith("Repository") || t.Name.EndsWith("Service") || t.Name.EndsWith("UnitOfWork"))
                .InstancePerDependency()//瞬时单例
               .AsImplementedInterfaces()////自动以其实现的所有接口类型暴露（包括IDisposable接口）
               .EnableInterfaceInterceptors(); //引用Autofac.Extras.DynamicProxy;
        }
    }
}
