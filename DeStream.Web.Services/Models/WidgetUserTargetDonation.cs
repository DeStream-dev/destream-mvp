using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Web.Services.Models
{
    public class WidgetUserTargetDonation
    {
        public string TargetName { get; set; }
        public string Code { get; set; }
        public decimal DestinationTargetTotal { get; set; }
        public decimal ActualTotal { get; set; }
        public decimal LastDonateTotal { get; set; }
        public List<AvailableDonate> AvailableDonates { get; set; } = new List<AvailableDonate>();
        
    }

    public class AvailableDonate
    {
        public decimal DonateTotal { get; set; }
        public string Token { get; set; }
    }
}
