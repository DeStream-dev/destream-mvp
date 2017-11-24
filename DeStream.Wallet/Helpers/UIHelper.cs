using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeStream.Wallet.Helpers
{
    public static class UIHelper
    {
        public static NotifyIcon CreateNotifyIconForTray()
        {
            var notifyIcon = new NotifyIcon();

            notifyIcon.Icon = Properties.Resources.wallet_trayIcon;
            notifyIcon.Text = "DeStream Wallet";
            return notifyIcon;
        }
    }
}
