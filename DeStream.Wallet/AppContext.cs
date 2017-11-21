using DeStream.Wallet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Wallet
{
    internal class AppContext
    {
        public static Config Config { get; set; }
        public static User CurrentUser { get; set; }
    }
}
