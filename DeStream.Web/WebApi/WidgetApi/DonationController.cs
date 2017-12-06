using Autofac;
using DeStream.Web.Helpers;
using DeStream.Web.Models;
using DeStream.Web.Services;
using DeStream.Web.Services.Models;
using DeStream.Web.WebApi.SignalR;
using DeStream.WebApi.Models;
using DeStream.WebApi.Models.Response;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace DeStream.Web.WebApi.WidgetApi
{
    [RoutePrefix("api/demo/widget/donation")]
    public class DonationController : HubApiControllerBase<DonationHub>
    {
        [Route("GetAll/{id}")]
        [ResponseType(typeof(ListResponse<WidgetUserTargetDonation>))]
        public IHttpActionResult GetAll(string id)
        {
            ListResponse<WidgetUserTargetDonation> res = null;
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

        [HttpPost, Route("Authorize")]
        public async Task<IHttpActionResult> Authorize(LoginMoodel model)
        {
            if (model != null)
            {
                using (var scope = DependencyConfig.Container.BeginLifetimeScope())
                {
                    var service = scope.Resolve<IUserService>();
                    var authResult = await service.Auth(model.Email, model.Password);
                    if (authResult != null)
                    {
                        HttpContext.Current.GetOwinContext().Authentication.SignIn(new Microsoft.Owin.Security.AuthenticationProperties { IsPersistent = true }, authResult);
                        return Ok();
                    }
                }
            }
            return Content( HttpStatusCode.BadRequest, new { ErrorMessage = "Incorrect data." });
        }

        [HttpPost, Route("")]
        public IHttpActionResult Donate(WidgetAddDonationModel model)
        {
            using (var scope = DependencyConfig.Container.BeginLifetimeScope())
            {
                var curUserId = User.Identity.GetUserId();
                var service = scope.Resolve<IUserTargetDonationsService>();
                var res = service.AddDonationFromWidgetByToken(model.Token, curUserId);
                if(res.Status==Common.Enums.OperationResultType.Success && res.WidgetNotificationResult!=null)
                {
                    WidgetSignalNotificator.DonationAdded(res.WidgetNotificationResult, res.TargetUserId, Hub);
                    WalletSignalNotificator.WalletBalanceChanged(curUserId, res.WalletNotificationResult, Hub);
                    //todo добавить изменение баланса для отправителя
                }
                        
            }
            return Ok();
        }
    }
}
