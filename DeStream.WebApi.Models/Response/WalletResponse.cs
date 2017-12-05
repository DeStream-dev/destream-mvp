using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.WebApi.Models.Response
{
    public class WalletResponse
    {
        public decimal Total { get; set; }
        public WalletOperation LastOperation { get; set; }

    }

    public enum WalletOperationType
    {
        Income,
        Outcome
    }

    public class WalletOperation
    {
        public decimal Total { get; set; }
        public WalletOperationType OperationType { get; set; }
    }

}
