using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Wallet.Models
{
    internal class BalanceChangedMessage
    {
        public decimal Total { get; set; }
    }
}
