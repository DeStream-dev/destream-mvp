﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Web.Services.Models
{
    public class UserInfo
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}