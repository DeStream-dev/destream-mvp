using Autofac;
using DeStream.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DeStream.Web.WebApi.WidgetApi
{
    [RoutePrefix("api/demo/widget/donation")]
    public class DonationController : BaseApiController
    {
        [Route("GetAll/{id}")]
        public IHttpActionResult GetAll(string id)
        {
            using (var scope = DependencyConfig.Container.BeginLifetimeScope())
            {
                var service = scope.Resolve<IUserTargetDonationsService>();
                return Ok(service.GetDonations(id));
            }

        }
    }
}
