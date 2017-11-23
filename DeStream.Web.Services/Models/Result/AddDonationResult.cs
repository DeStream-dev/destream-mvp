using DeStream.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Web.Services.Models.Result
{
    public class AddDonationResult
    {
        public SignalRAddDonationNotificationResult SignalRResult { get; set; }
        public OperationResultType Status { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

    public class SignalRAddDonationNotificationResult
    {
        public string UserId { get; set; }
        public string Code { get; set; }
        public decimal Donation { get; set; }
    }
}
