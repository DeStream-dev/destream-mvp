using Autofac;
using DeStream.Web.Entities.Data.Services.Identity;
using DeStream.Web.Entities.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Web.Entities.Data.Services
{
    public class WebDataServicesModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DeStreamWebDbContext>().AsSelf()/*.As<IIdentityDbContext>()*/.InstancePerLifetimeScope().PropertiesAutowired();
            builder.Register(x => new UserStore<ApplicationUser>(x.Resolve<DeStreamWebDbContext>())).AsImplementedInterfaces().PropertiesAutowired().InstancePerLifetimeScope();
            builder.Register(x => new RoleStore<ApplicationRole>(x.Resolve<DeStreamWebDbContext>())).AsImplementedInterfaces().PropertiesAutowired().InstancePerLifetimeScope();

            builder.Register<IdentityFactoryOptions<ApplicationUserManager>>(c => new IdentityFactoryOptions<ApplicationUserManager>
            { DataProtectionProvider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionPr‌​ovider("DeStream") });

            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerLifetimeScope().PropertiesAutowired();
            builder.RegisterType<ApplicationRoleManager>().AsSelf().InstancePerLifetimeScope().PropertiesAutowired();

            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces().PropertiesAutowired().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
