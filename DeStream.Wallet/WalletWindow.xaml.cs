using DeStream.Wallet.Helpers;
using DeStream.Wallet.Models;
using GalaSoft.MvvmLight.Messaging;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DeStream.Wallet
{
    /// <summary>
    /// Interaction logic for WalletWindow.xaml
    /// </summary>
    public partial class WalletWindow : Window
    {
        private const decimal MinDonateValue = 0;

        private readonly NotifyIcon _notifyIcon;
        private readonly Storyboard _statusBarOperationMessageStoryBoard;
        private decimal PreviousDonateValue = 0;

        public WalletWindow()
        {
            InitializeComponent();
            UsernameTextBox.Focus();
            _notifyIcon = UIHelper.CreateNotifyIconForTray();
            _notifyIcon.DoubleClick += _notifyIcon_DoubleClick;
            _statusBarOperationMessageStoryBoard = Resources["operationStatusStoryboard"] as Storyboard;
            Messenger.Default.Register<BalanceChangedMessage>(this, BalanceChanged_Handler);
        }

        private void BalanceChanged_Handler(BalanceChangedMessage message)
        {
            Dispatcher.Invoke(() =>
            {
                _statusBarOperationMessageStoryBoard.Begin();
                if (this.WindowState == WindowState.Minimized)
                {
                    _notifyIcon.Visible = false;
                    WindowState = WindowState.Normal;
                }

                Show();
            });
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
