using Autofac;
using DeStream.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace DeStream.Web.Controllers
{
    public class WidgetController : Controller
    {
        public ActionResult Index(string id)
        {
            using (var scope = DependencyConfig.Container.BeginLifetimeScope())
            {
                var service = scope.Resolve<IUserService>();
                var info = service.GetUserInfo(id);
                return View(info);
            }
        }
    }
}
