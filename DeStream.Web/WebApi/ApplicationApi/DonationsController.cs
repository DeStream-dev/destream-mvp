using Autofac;
using DeStream.Web.Services;
using DeStream.WebApi.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DeStream.Web.WebApi.ApplicationApi
{
    [RoutePrefix("api/demo/app/donations")]
    public class DonationsController : BaseApiController
    {
        [Route("")]
        public IHttpActionResult Post(AddDonationRequest donation)
        {
            using (var scope = DependencyConfig.Container.BeginLifetimeScope())
            {
                var service = scope.Resolve<IUserTargetDonationsService>();
                var result = service.AddDonation(donation.UserName, donation.FromUserName, donation.DonationValue, donation.TargetCode);
                if (result.Status == Common.Enums.OperationResultType.Success)
                    return Ok();
                else
                    return Content(HttpStatusCode.BadRequest, new { ErrorMessage = string.Join("; ", result.Errors) });
            }
        }
    }
}
