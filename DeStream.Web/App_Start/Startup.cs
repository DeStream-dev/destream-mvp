using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using NLog;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: OwinStartup(typeof(DeStream.Web.App_Start.Startup))]

namespace DeStream.Web.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            DependencyConfig.Configure();

            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                CookieName="dsauth"
            });

        }

    }
}