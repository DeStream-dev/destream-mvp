using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
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
        public WalletWindow()
        {
            InitializeComponent();
            UsernameTextBox.Focus();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.MainWindow.Show();
        }

        const decimal MinValue = 0;

        private decimal PreviousValue = 0;

        private void DonationTotalTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal inputVal = 0;
            if (!string.IsNullOrWhiteSpace(DonationTotalTextBox.Text))
            {
                if (decimal.TryParse(DonationTotalTextBox.Text, out inputVal))
                {
                    if (inputVal >= MinValue && inputVal != PreviousValue)
                    {
                        PreviousValue = inputVal;
                    }
                    else
                        DonationTotalTextBox.Text = PreviousValue.ToString();
                }
                else
                {
                    DonationTotalTextBox.Text = PreviousValue.ToString();
                }
            }
            else
            {
                PreviousValue = MinValue;
                DonationTotalTextBox.Text = PreviousValue.ToString();
            }

        }
    }
}
