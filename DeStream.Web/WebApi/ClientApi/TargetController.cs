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
    [RoutePrefix("api/demo/client/target")]
    public class TargetController : BaseApiController
    {
        [Route("getAll")]
        public IHttpActionResult GetAll()
        {
            using (var scope = DependencyConfig.Container.BeginLifetimeScope())
            {
                var service = scope.Resolve<IUserTargetService>();
                var result = service.GetUserTargets(User.Identity.GetUserId());
                return Ok(result);
            }
        }

        [Route("saveAll")]
        public IHttpActionResult SaveAll(List<UserTarget>targets)
        {
            using (var scope = DependencyConfig.Container.BeginLifetimeScope())
            {
                var service = scope.Resolve<IUserTargetService>();
                var res = service.SaveUserTargets(User.Identity.GetUserId(), targets);
                if (res.Status == Common.Enums.OperationResultType.Success)
                    return Ok();
                else
                    return Content(HttpStatusCode.BadRequest, new { ErrorMessage = string.Join("; ", res.Errors) });
            }
        }
    }
}
