using Autofac;
using DeStream.Web.Services;
using DeStream.Web.Services.Models;
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
            UserInfo info = null;
            using (var scope = DependencyConfig.Container.BeginLifetimeScope())
            {
                Guid guid = Guid.Empty;
                if (Guid.TryParse(id, out guid))
                {
                    var service = scope.Resolve<IUserService>();
                    info = service.GetUserInfo(guid.ToString());
                }
            }
            return View(info);
        }
    }
}
