using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using RestSharp;
using System;
using System.Collections.Generic;
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

        public RelayCommand SendDonationCommand { get; private set; }

        public WalletVM()
        {
            if (IsInDesignMode)
                ErrorMessage = "Error error test";
            AllowUserInteraction = true;
            decimal baseDec = 0;
            while (baseDec == 0)
                baseDec = new Random().Next(10);
            WalletTotal = (baseDec * 100000) + new Random().Next(99999);
            SendDonationCommand = new RelayCommand(SendDonationCommand_Handler, CanSendDonationCommand_Handler);
        }

        private void SendDonationCommand_Handler()
        {   
            AllowUserInteraction = false;

            Task.Factory.StartNew(async() =>
            {
                await Task.Delay(1000);
                SuccessMessage = null;
                ErrorMessage = null;
                var req = new RestRequest("/api/demo/app/donations");
                req.AddObject(new AddDonationRequest
                {
                    DonationValue = DonationTotal,
                    FromUserName = AppContext.CurrentUser.Username,
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
                AllowUserInteraction= true;
            }/*, CancellationToken.None, TaskCreationOptions.None, Scheduler*/);

        }

        private bool CanSendDonationCommand_Handler()
        {
            return !string.IsNullOrWhiteSpace(DestinationUsername) && !string.IsNullOrWhiteSpace(TargetCode)
                && DonationTotal > 0 && DonationTotal < WalletTotal;
        }


    }
}
