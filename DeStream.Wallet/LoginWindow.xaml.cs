using DeStream.Wallet.Helpers;
using DeStream.Wallet.VM;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DeStream.Wallet
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly NotifyIcon _notifyIcon;
        public LoginWindow()
        {
            InitializeComponent();
            UsernameTextBox.Focus();
            _notifyIcon = UIHelper.CreateNotifyIconForTray();
            _notifyIcon.MouseDoubleClick += _notifyIcon_MouseDoubleClick;
        }

        private void _notifyIcon_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
        }

        private void PasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            LoginVM vm = (sender as PasswordBox).DataContext as LoginVM;
            vm.Password = PasswordTextBox.Password;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if(WindowState==WindowState.Minimized)
            {
                Hide();
                _notifyIcon.Visible = true;
            }
            else
            {
                _notifyIcon.Visible = false;
            }
        }
    }
}
