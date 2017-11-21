using DeStream.Wallet;
using DeStream.Wallet.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Wallet
{
    public class VMLocator
    {
        public LoginVM LoginVM
        {
            get
            {
                return new LoginVM(new WS());
            }
        }

        public WalletVM WalletWM
        {
            get { return new WalletVM(); }
        }

    }
}
