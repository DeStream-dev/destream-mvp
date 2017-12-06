using DeStream.Web.Services.Models.Result;
using DeStream.WebApi.Models.Response;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeStream.Web.Helpers
{
    public static class WidgetSignalNotificator
    {
        public static void DonationAdded(WidgetNotificationResult donationInfo, string targetUserId, IHubContext hub)
        {
            var group = hub.Clients.Group(targetUserId);
            group.donationAdded(donationInfo);
        }
    }

    public static class WalletSignalNotificator
    {
        public static void WalletBalanceChanged(string userId, WalletBalanceChangedResponse balanceChanged, IHubContext hub)
        {
            var group = hub.Clients.Group(userId);
            group.walletBalanceChanged(balanceChanged);
        }
    }
}