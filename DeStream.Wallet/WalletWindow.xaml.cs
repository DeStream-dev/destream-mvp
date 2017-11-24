using DeStream.Wallet.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DeStream.Wallet
{
    /// <summary>
    /// Interaction logic for WalletWindow.xaml
    /// </summary>
    public partial class WalletWindow : Window
    {
        private const decimal MinDonateValue = 0;

        private readonly NotifyIcon _notifyIcon;
        private decimal PreviousDonateValue = 0;
        public WalletWindow()
        {
            InitializeComponent();
            UsernameTextBox.Focus();
            _notifyIcon = UIHelper.CreateNotifyIconForTray();
            _notifyIcon.DoubleClick += _notifyIcon_DoubleClick;
        }

        private void _notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.MainWindow.Show();
        }

       

        private void DonationTotalTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal inputVal = 0;
            if (!string.IsNullOrWhiteSpace(DonationTotalTextBox.Text))
            {
                if (decimal.TryParse(DonationTotalTextBox.Text, out inputVal))
                {
                    if (inputVal >= MinDonateValue && inputVal != PreviousDonateValue)
                    {
                        PreviousDonateValue = inputVal;
                    }
                    else
                        DonationTotalTextBox.Text = PreviousDonateValue.ToString();
                }
                else
                {
                    DonationTotalTextBox.Text = PreviousDonateValue.ToString();
                }
            }
            else
            {
                PreviousDonateValue = MinDonateValue;
                DonationTotalTextBox.Text = PreviousDonateValue.ToString();
            }

        }


        private void Window_StateChanged(object sender, EventArgs e)
        {
            var state = this.WindowState;
            if (state == WindowState.Minimized)
            {
                Hide();
                _notifyIcon.Visible = true;
            }
            else
                _notifyIcon.Visible = false;
        }
    }
}
