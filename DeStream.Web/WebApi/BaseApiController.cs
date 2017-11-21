using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Diagnostics;
using DeStream.Web.Helpers;

namespace DeStream.Web.WebApi
{
    public class BaseApiController:ApiController
    {
        private static readonly Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public override async Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            var watch = Stopwatch.StartNew();
            LoggingHelper.LogRequest(controllerContext.Request);
            HttpResponseMessage response;
            try
            {
                response = await base.ExecuteAsync(controllerContext, cancellationToken);
            }
            catch (Exception e)
            {
                Log.Error(e, "Error while executing request");
                throw;
            }
            LoggingHelper.LogResponse(controllerContext.Request, response, watch.Elapsed);
            return response;
        }
    }
}