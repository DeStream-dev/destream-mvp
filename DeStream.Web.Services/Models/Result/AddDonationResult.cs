using DeStream.Common.Enums;
using DeStream.WebApi.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Web.Services.Models.Result
{
    public class AddDonationResult
    {
        public WidgetNotificationResult WidgetNotificationResult { get; set; }
        public WalletBalanceChangedResponse WalletDestintaionUserNotificationResult { get; set; }
        public WalletBalanceChangedResponse WalletSenderUserNotificationResult { get; set; }
        public OperationResultType Status { get; set; }
        public string TargetUserId { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

    public class WidgetNotificationResult
    {
        public string Code { get; set; }
        public decimal Donation { get; set; }
    }
}
