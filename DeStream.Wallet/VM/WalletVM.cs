using DeStream.Wallet.Helpers;
using DeStream.Wallet.Models;
using DeStream.WebApi.Models.Request;
using DeStream.WebApi.Models.Response;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeStream.Wallet.VM
{
    public class WalletVM : ViewModelBase
    {

        private readonly TaskScheduler Scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        private decimal _walleTotal;
        public decimal WalletTotal
        {
            get
            {
                return _walleTotal;
            }
            set
            {
                Set(() => WalletTotal, ref _walleTotal, value);
            }
        }


        private string _destinationUsername;
        public string DestinationUsername
        {
            get
            {
                return _destinationUsername;
            }
            set
            {
                Set(() => DestinationUsername, ref _destinationUsername, value);
            }
        }

        private decimal _donationTotal;
        public decimal DonationTotal
        {
            get
            {
                return _donationTotal;
            }
            set
            {
                Set(() => DonationTotal, ref _donationTotal, value);
            }
        }

        private string _targetCode;
        public string TargetCode
        {
            get
            {
                return _targetCode;
            }
            set
            {
                Set(() => TargetCode, ref _targetCode, value);
            }

        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                Set(() => ErrorMessage, ref _errorMessage, value);
            }
        }

        private string _successMessage;
        public string SuccessMessage
        {
            get
            {
                return _successMessage;
            }
            set
            {
                Set(() => SuccessMessage, ref _successMessage, value);
            }
        }

        private bool _allowUserInteraction;
        public bool AllowUserInteraction
        {
            get
            {
                return _allowUserInteraction;
            }
            set
            {
                Set(() => AllowUserInteraction, ref _allowUserInteraction, value);
            }
        }

        private string _statusBarLastOperationMessage;
        public string StatusBarLastOperationMessage
        {
            get
            {
                return _statusBarLastOperationMessage;
            }
            set
            {
                Set(() => StatusBarLastOperationMessage, ref _statusBarLastOperationMessage, value);
            }
        }

        public RelayCommand SendDonationCommand { get; private set; }

        public WalletVM()
        {
            if (IsInDesignMode)
            {
                ErrorMessage = "Error error test";
                SuccessMessage = "olololo lolol";
                StatusBarLastOperationMessage = "+1000000";
            }
            AllowUserInteraction = false;
            SendDonationCommand = new RelayCommand(SendDonationCommand_Handler, CanSendDonationCommand_Handler);
            LoadWallet();
            Connect();
        }

        private void LoadWallet()
        {
            AllowUserInteraction = false;
            var req = new RestRequest(Constants.WalletWebApi + $"/{AppContext.CurrentUser.Uid}", Method.GET);
            var client = new RestClient(AppContext.Config.SiteUrl);
            Task.Factory.StartNew(async () =>
            {
                await Task.Delay(1000);
                var response = client.Get<RestResponse>(req);
                //var response = x.Result;
                if (response.IsSuccessful)
                {
                    var wallet = JsonConvert.DeserializeObject<WalletResponse>(response.Content);
                    if (wallet != null)
                    {
                        WalletTotal = wallet.Total;

                    }
                    AllowUserInteraction = true;
                }
                else
                {
                    SuccessMessage = null;
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest && !string.IsNullOrWhiteSpace(response.Content))
                    {
                        var err = ResponseHelper.ParseError(response.Content);
                        ErrorMessage = err?.ErrorMessage;
                    }
                    if (ErrorMessage == null)
                        ErrorMessage = "Error happened.";
                }
                //return response;
            });
        }

        private void SendDonationCommand_Handler()
        {
            AllowUserInteraction = false;

            Task.Factory.StartNew(async () =>
            {
                await Task.Delay(1000);
                SuccessMessage = null;
                ErrorMessage = null;
                var req = new RestRequest("/api/demo/app/donations");
                req.AddObject(new AddDonationRequest
                {
                    DonationValue = DonationTotal,
                    FromUser = AppContext.CurrentUser.Uid,
                    TargetCode = TargetCode,
                    UserName = DestinationUsername
                });
                var client = new RestClient(AppContext.Config.SiteUrl);
                var resp = client.Post(req);
                if (resp.IsSuccessful)
                {
                    ErrorMessage = null;
                    SuccessMessage = "Donation sent.";
                    WalletTotal -= DonationTotal;
                }
                else
                {
                    SuccessMessage = null;
                    if (resp.StatusCode == System.Net.HttpStatusCode.BadRequest && !string.IsNullOrWhiteSpace(resp.Content))
                    {
                        var err = Newtonsoft.Json.JsonConvert.DeserializeObject<ErrorResponse>(resp.Content);
                        ErrorMessage = err?.ErrorMessage;
                    }
                    if (ErrorMessage == null)
                        ErrorMessage = "Error happened.";
                }
                AllowUserInteraction = true;
            }/*, CancellationToken.None, TaskCreationOptions.None, Scheduler*/);

        }

        private bool CanSendDonationCommand_Handler()
        {
            return !string.IsNullOrWhiteSpace(DestinationUsername) && !string.IsNullOrWhiteSpace(TargetCode)
                && DonationTotal > 0 && DonationTotal < WalletTotal;
        }

        private HubConnection _connection;

        private void Connect()
        {
            Task.Factory.StartNew(async () =>
            {
                NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
                try
                {
                    _connection = new HubConnection(AppContext.Config.SiteUrl);
                    _connection.Received += x =>
                     {
                         log.Info(x);
                     };
                    _connection.Closed += () => { log.Info("closed"); };
                    _connection.ConnectionSlow += () => { log.Info("slow connection"); };
                    _connection.Error += x => { log.Info("error"); };
                    _connection.Reconnected += () => { log.Info("reconnected"); };
                    _connection.Reconnecting += () => { log.Info("reconnecting"); };
                    _connection.StateChanged += (x) => { log.Info($"state changed {x.NewState.ToString()}"); };

                    var hubProxy = _connection.CreateHubProxy("donationhub");
                    log.Info("start");
                    await _connection.Start();
                    await hubProxy.Invoke("subscribe", AppContext.CurrentUser.Uid);
                    hubProxy.On<WalletBalanceChangedResponse>("walletBalanceChanged", res =>
                    {
                        var sign = res.LastOperation.OperationType == WalletOperationType.Income ? "+" : "-";
                        StatusBarLastOperationMessage = $"{sign}{res.LastOperation.Total}";
                        LoadWallet();
                        SuccessMessage = string.Empty;
                        MessengerInstance.Send(new BalanceChangedMessage());
                    });

                }
                catch (Exception e)
                {
                    log.Error(e, "Error on hub connection");
                }

            });
        }

        public override void Cleanup()
        {
            if (_connection != null)
            {
                Task.Factory.StartNew(() =>
                {
                    _connection.Dispose();
                });
            }
            //base.Cleanup();
        }


    }
}
