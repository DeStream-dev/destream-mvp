using Autofac;
using DeStream.Web.App_Start;
using DeStream.Web.Services;
using DeStream.Web.Services.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DeStream.Web.WebApi.ClientApi
{
    [CustomAuthorize]
    [RoutePrefix("api/demo/client/userInfo")]
    public class UserInfoController : BaseApiController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            using (var scope = DependencyConfig.Container.BeginLifetimeScope())
            {
                var service = scope.Resolve<IUserService>();
                var info= service.GetUserInfo(User.Identity.GetUserId());
                if (info == null) info = new UserInfo();
                return Ok(info);
            }
        }

        [Route("")]
        public IHttpActionResult Post(UserProfile model)
        {
            using (var scope = DependencyConfig.Container.BeginLifetimeScope())
            {
                var service = scope.Resolve<IUserService>();
                service.SaveProfile(User.Identity.GetUserId(), model);
            }
            return Ok();
        }
    }
}
