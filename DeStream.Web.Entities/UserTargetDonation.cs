﻿using DeStream.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Web.Entities
{
    public class UserTargetDonation:Entity<long>
    {
        public decimal DonationTotal { get; set; }
        public DateTime CreatedOn { get; set; }

        [Required,MaxLength(Constants.ShortStringMaxLength)]
        public string DonationFrom { get; set; }

        public long UserTargetId { get; set; }
        public UserTarget UserTarget { get; set; }
        
    }
}
