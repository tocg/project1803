using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PH6_1803.Lib.AutoFac.CoreApi.Controllers;
using PH6_1803.Lib.AutoFac.CoreApi.Services;

namespace PH6_1803.Lib.AutoFac.CoreApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }



        /// <summary>
        /// Autofac的新添加方法
        /// 用来配置Autofac 服务容器
        /// 配置容器的方法--> 集成Autofac -->由框架自动去调用
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(Autofac.ContainerBuilder builder)
        {
            //Autofac支持批量注册服务(.net core内置的DI不支持, 只能一个一个的注册)
            //程序集扫描注入(包含.Services的类名)
            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Namespace == assembly.GetName().Name + ".Services")
                .AsImplementedInterfaces().InstancePerLifetimeScope();

            //给程序集下所有的控制器开启属性注入服务的功能
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => typeof(ControllerBase).IsAssignableFrom(t) && t != typeof(ControllerBase))
                .PropertiesAutowired();

            //给指定的控制器开启属性注入服务的功能
            builder.RegisterType<MessageController>().PropertiesAutowired();
        }

        //public void ConfigureContainer(ContainerBuilder builder)
        //{
        //    builder.RegisterModule(new AutofacModuleRegister());
        //    builder.RegisterModule(new ServiceMoudle(name: "Services"));
        //}

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            //AddControllersAsServices支持属性注入
            //1.从容器中获取控制器实例
            //2.将Asp.Net Core能发现的控制器类型都注册到容器中
            services.AddControllers().AddControllersAsServices();
            //services.AddControllers();
            //services.AddTransient<IAccount, Account>();
            //services.AddTransient<IMessage, Message>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
