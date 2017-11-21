using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Wallet.Models
{
    internal class Config
    {
        public List<User> Users { get; set; }
        public string SiteUrl { get; set; }
    }

    internal class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

}
