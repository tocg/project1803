﻿using Autofac;
using Autofac.Extras.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PH6_1803.Lib.AutoFac.CoreApi
{
    /// <summary>
    /// Autofac新添加类
    /// </summary>
    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(Autofac.ContainerBuilder builder)
        {

            //注册Service
            var assemblysServices = System.Reflection.Assembly.Load("Webapi.Core.Service");
            builder.RegisterAssemblyTypes(assemblysServices)
                .InstancePerDependency()//瞬时单例
               .AsImplementedInterfaces()////自动以其实现的所有接口类型暴露（包括IDisposable接口）
               .EnableInterfaceInterceptors(); //引用Autofac.Extras.DynamicProxy;

            ////注册Repository
            //var assemblysRepository = Assembly.Load("Webapi.Core.Repository");
            //builder.RegisterAssemblyTypes(assemblysRepository)
            //    .InstancePerDependency()//瞬时单例
            //   .AsImplementedInterfaces()////自动以其实现的所有接口类型暴露（包括IDisposable接口）
            //   .EnableInterfaceInterceptors(); //引用Autofac.Extras.DynamicProxy;

        }
    }
}
