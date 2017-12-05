using DeStream.Wallet.Helpers;
using DeStream.WebApi.Models.Request;
using DeStream.WebApi.Models.Response;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Http;
using Microsoft.AspNet.SignalR.Client.Hubs;
using RestSharp;
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

        private readonly TaskScheduler Scheduler = TaskScheduler.FromCurrentSynchronizationContext();

        private void EnterCommand_Handler()
        {
            LoginError = string.Empty;
            var req = new RestRequest(Constants.WalletWebApi, Method.POST);
            req.AddObject(new AuthorizeRequest { Username = Username, Password = Password });
            var client = new RestClient(AppContext.Config.SiteUrl);
            Task.Factory.StartNew(() =>
            {
                var response = client.Post<RestResponse>(req);
                return response;

            }).ContinueWith(x =>
            {
                var response = x.Result;
                if (response.IsSuccessful)
                {
                    if (!string.IsNullOrWhiteSpace(response.Content))
                    {
                        var userResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<AuthorizeResponse>(response.Content);
                        if (userResponse != null && userResponse.User != null)
                        {
                            AppContext.CurrentUser = userResponse.User;
                            _ws.ShowWindow<WalletWindow>();
                            //
                        }
                    }
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest && !string.IsNullOrWhiteSpace(response.Content))
                    {
                        var errorResponse = ResponseHelper.ParseError(response.Content);
                        if (errorResponse != null)
                            LoginError = errorResponse.ErrorMessage;
                    }
                }

                if (AppContext.CurrentUser == null && string.IsNullOrWhiteSpace(LoginError))
                {
                    LoginError = "Error happened.";
                }
            }, Scheduler);


        }

        private bool CanEnterCommand_Handler()
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }

        
    }
}
