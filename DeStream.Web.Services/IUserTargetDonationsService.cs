using DeStream.Web.Services.Models;
using DeStream.Web.Services.Models.Result;
using DeStream.WebApi.Models;
using DeStream.WebApi.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Web.Services
{
    public interface IUserTargetDonationsService
    {
        AddDonationResult AddDonation(string toUserName, string fromUserId, decimal total, string targetCode);
        ListResponse<WidgetUserTargetDonation> GetDonations(string userId);
        AddDonationResult AddDonationFromWidgetByToken(string token, string fromUser);
    }
}
