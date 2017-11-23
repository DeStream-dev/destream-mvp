using DeStream.Web.Services.Models.Result;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeStream.Web.Helpers
{
    public static class WidgetSignalNotificator
    {
        public static void DonationAdded(SignalRAddDonationNotificationResult donationInfo, IHubContext hub)
        {
            var group= hub.Clients.Group(donationInfo.UserId);
            group.donationAdded(donationInfo);
        }
    }
}