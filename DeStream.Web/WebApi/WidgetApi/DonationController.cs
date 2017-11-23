using Autofac;
using DeStream.Web.Services;
using DeStream.Web.WebApi.SignalR;
using DeStream.WebApi.Models;
using DeStream.WebApi.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace DeStream.Web.WebApi.WidgetApi
{
    [RoutePrefix("api/demo/widget/donation")]
    public class DonationController : HubApiControllerBase<DonationHub>
    {
        [Route("GetAll/{id}")]
        [ResponseType(typeof(ListResponse<UserTargetDonation>))]
        public IHttpActionResult GetAll(string id)
        {
            ListResponse<UserTargetDonation> res = null;
            using (var scope = DependencyConfig.Container.BeginLifetimeScope())
            {
                Guid uid = Guid.Empty;
                if (Guid.TryParse(id, out uid))
                {
                    var service = scope.Resolve<IUserTargetDonationsService>();
                    res = service.GetDonations(uid.ToString());
                }
            }
            return Ok(res);
        }
    }
}
