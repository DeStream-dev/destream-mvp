using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Wallet.VM
{
    public class LoginVM : ViewModelBase
    {
        private readonly IWindowService _ws;

        private string _username;
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                Set(() => Username, ref _username, value);
            }
        }

        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                Set(() => Password, ref _password, value);
            }
        }

        private string _loginError;
        public string LoginError
        {
            get
            {
                return _loginError;
            }
            set
            {
                Set(() => LoginError, ref _loginError, value);
            }
        }

        public RelayCommand EnterCommand { get; private set; }

        public LoginVM(IWindowService ws)
        {
            if (IsInDesignModeStatic)
                LoginError = "Error error.";
            EnterCommand = new RelayCommand(EnterCommand_Handler, CanEnterCommand_Handler);
            _ws = ws;
        }

       

        private void EnterCommand_Handler()
        {
            LoginError = string.Empty;

            var user = AppContext.Config.Users.FirstOrDefault(x => x.Username == Username && x.Password==Password);
            if(user!=null)
            {
                AppContext.CurrentUser = user;
                _ws.ShowWindow<WalletWindow>();
            }
            else
            {
                LoginError = "Incorrect username or password.";
            }
        }

        private bool CanEnterCommand_Handler()
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }
    }
}
