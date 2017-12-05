using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.WebApi.Models.Response
{
    public class WalletBalanceChangedResponse
    {
        public decimal Total { get; set; }
        public WalletOperation LastOperation { get; set; }
    }
}
