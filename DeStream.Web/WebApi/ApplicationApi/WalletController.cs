using Autofac;
using DeStream.Web.Services;
using DeStream.WebApi.Models.Request;
using DeStream.WebApi.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace DeStream.Web.WebApi.ApplicationApi
{
    [RoutePrefix("api/demo/app/wallet")]
    public class WalletController:ApiController// : BaseApiController
    {
        [HttpGet]
        [Route("{userUid}")]
        [ResponseType(typeof(WalletResponse))]
        public IHttpActionResult Get(string userUid)
        {
            if(!string.IsNullOrWhiteSpace(userUid))
            {
                using (var scope = DependencyConfig.Container.BeginLifetimeScope())
                {
                    var service = scope.Resolve<IWalletService>();
                    var wallet = service.GetWallet(userUid);
                    if (wallet != null)
                        return Ok(wallet);
                    else
                        return BadRequest();
                }
            }
            return BadRequest();

        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Authorize(AuthorizeRequest request)
        {
            if (request != null)
            {
                using (var scope = DependencyConfig.Container.BeginLifetimeScope())
                {
                    var service = scope.Resolve<IWalletService>();
                    var user = await service.Authorize(request.Username, request.Password);
                    if (user != null)
                    {
                        return Ok(new AuthorizeResponse { User = user });
                    }
                    else
                        return Content(HttpStatusCode.BadRequest, new { ErrorMessage = "Please, verify your username/password." });
                }
            }
            else
                return BadRequest();
        }
    }
}
