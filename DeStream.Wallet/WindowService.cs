using DeStream.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DeStream.Wallet
{
    internal class WS : IWindowService
    {
        public void ShowWindow<TWindow>() where TWindow : Window,new()
        {
            var w = new TWindow();
            w.Show();

            Application.Current.MainWindow.Hide();
        }
    }

    public interface IWindowService
    {
        void ShowWindow<TWindow>() where TWindow : Window,new();
    }
}
