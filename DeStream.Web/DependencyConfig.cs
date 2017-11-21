using Autofac;
using DeStream.Web.Entities.Data.Services;
using DeStream.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeStream.Web
{
    public static class DependencyConfig
    {
        public static IContainer Container { get; private set; }
        public static void Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<WebDataServicesModule>();
            builder.RegisterModule<WebServicesModule>();
            Container = builder.Build();

            Services.ModelsConfig.Configure();

        }
    }
}