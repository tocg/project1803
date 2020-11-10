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
        /// Autofac������ӷ���
        /// ��������Autofac ��������
        /// ���������ķ���--> ����Autofac -->�ɿ���Զ�ȥ����
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(Autofac.ContainerBuilder builder)
        {
            //Autofac֧������ע�����(.net core���õ�DI��֧��, ֻ��һ��һ����ע��)
            //����ɨ��ע��(����.Services������)
            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Namespace == assembly.GetName().Name + ".Services")
                .AsImplementedInterfaces().InstancePerLifetimeScope();

            //�����������еĿ�������������ע�����Ĺ���
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => typeof(ControllerBase).IsAssignableFrom(t) && t != typeof(ControllerBase))
                .PropertiesAutowired();

            //��ָ���Ŀ�������������ע�����Ĺ���
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


            //AddControllersAsServices֧������ע��
            //1.�������л�ȡ������ʵ��
            //2.��Asp.Net Core�ܷ��ֵĿ��������Ͷ�ע�ᵽ������
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
