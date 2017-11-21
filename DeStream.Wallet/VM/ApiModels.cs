using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Wallet.VM
{
    internal class AddDonationRequest
    {
        public decimal DonationValue { get; set; }
        public string FromUserName { get; set; }
        public string TargetCode { get; set; }
        public string UserName { get; set; }
    }

    internal class ErrorResponse
    {
        public string ErrorMessage { get; set; }
    }

}
