using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace DeStream.Web.WebApi.SignalR
{
    public class DonationHub:Hub
    {
        private static readonly NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();
        public void Subscribe(string userId)
        {
            Log.Info("Connect");

            Guid uid = Guid.Empty;
            if (Guid.TryParse(userId, out uid))
            {
                Groups.Add(Context.ConnectionId, uid.ToString());
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Log.Info("Disconnect");
            return base.OnDisconnected(stopCalled);
        }

    }
}