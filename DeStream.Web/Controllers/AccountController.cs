using Autofac;
using DeStream.Web.App_Start;
using DeStream.Web.Models;
using DeStream.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using System.Web;
namespace DeStream.Web.Controllers
{
    public class AccountController : Controller
    {
        [CustomAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        [CustomAuthorize]
        public ActionResult Targets()
        {
            return View();
        }

        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
                return RedirectToAction("Index");
            return View();
        }

        public ActionResult Logout()
        {
            System.Web.HttpContext.Current.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Login");
        }

        public async Task<ActionResult> Register(string email, string username, string password)
        {
            var result = false;
            if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                using (var scope = DependencyConfig.Container.BeginLifetimeScope())
                {
                    var service = scope.Resolve<IUserService>();
                    result = await service.Create(email, username, password);
                }
            }
            return Content(result.ToString());
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Login(LoginMoodel model)
        {
            await Task.Delay(1000);
            using (var scope = DependencyConfig.Container.BeginLifetimeScope())
            {
                var service = scope.Resolve<IUserService>();
                var result = await service.Auth(model.Email, model.Password);
                if (result != null)
                {
                    System.Web.HttpContext.Current.GetOwinContext().Authentication.SignIn(new Microsoft.Owin.Security.AuthenticationProperties
                    {
                        IsPersistent = true
                    }, result);
                    // ;

                    return JavaScript("var waitAuth=true;window.location.pathname='/Account/Index';");
                }
            }
            ModelState.AddModelError("Password", "Incorrect data.");

            return PartialView("_LoginContainer", model);
        }
    }
}
