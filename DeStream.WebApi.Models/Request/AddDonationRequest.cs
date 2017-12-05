using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.WebApi.Models.Request
{
    public class AddDonationRequest
    {
        public decimal DonationValue { get; set; }
        public string FromUser { get; set; }
        public string TargetCode { get; set; }
        public string UserName { get; set; }
    }
}
