using Autofac;
using DeStream.Web.Entities.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Web.Services
{
    public class WebServicesModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<WebDataServicesModule>();
            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces().InstancePerLifetimeScope().PropertiesAutowired();
            base.Load(builder);
        }
    }
}
