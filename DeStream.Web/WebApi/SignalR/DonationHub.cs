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
        public void Subscribe(string userId)
        {
            NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
            log.Info(userId);
            Guid uid = Guid.Empty;
            if (Guid.TryParse(userId, out uid))
            {
                Groups.Add(Context.ConnectionId, uid.ToString());
            }
        }
        
    }
}