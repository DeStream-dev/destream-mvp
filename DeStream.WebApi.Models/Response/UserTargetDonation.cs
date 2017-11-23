﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.WebApi.Models.Response
{
    public class UserTargetDonation
    {
        public string TargetName { get; set; }
        public string Code { get; set; }
        public decimal DestinationTargetTotal { get; set; }
        public decimal ActualTotal { get; set; }
        public decimal LastDonateTotal { get; set; }
    }
}
