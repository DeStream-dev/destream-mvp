using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeStream.WebApi.Models;
using DeStream.WebApi.Models.Response;
using DeStream.Web.Entities.Data.Services;
using DeStream.Web.Services.Models.Result;
using DeStream.Web.Services.Models;

namespace DeStream.Web.Services.Implementations
{
    internal class UserTargetDonationsService : IUserTargetDonationsService
    {
        private readonly Lazy<IUnitOfWork> _unitOfWork;
        private readonly Lazy<IUserTargetDataService> _userTargetDataService;
        private readonly Lazy<IApplicationUserDataService> _applicationUserDataService;
        private readonly Lazy<IUserTargetDonationDataService> _userTargetDonationDataService;

        private const string TargetDonationKey = "dstream";

        public UserTargetDonationsService(Lazy<IUnitOfWork> unitOfWork,
            Lazy<IUserTargetDataService> userTargetDataService,
            Lazy<IApplicationUserDataService> applicationUserDataService,
            Lazy<IUserTargetDonationDataService> userTargetDonationDataService)
        {
            _unitOfWork = unitOfWork;
            _userTargetDataService = userTargetDataService;
            _applicationUserDataService = applicationUserDataService;
            _userTargetDonationDataService = userTargetDonationDataService;
        }

        private AddDonationResult AddDonationForUserId(string userId, string fromUserId, decimal total, string targetCode, AddDonationResult addDonationResult = null)
        {
            const string CommonErrorMessage = "Please, check username or target code.";

            var result = addDonationResult ?? new AddDonationResult();
            result.Status = Common.Enums.OperationResultType.Failed;
            if (userId != fromUserId)
            {
                var fromUserExists = _applicationUserDataService.Value.Query().Any(x => x.Id == fromUserId);
                if (fromUserExists)
                {
                    var target = _userTargetDataService.Value.Query().FirstOrDefault(x => x.ApplicationUserId == userId
                      && x.Code == targetCode);
                    if (target != null)
                    {
                        _userTargetDonationDataService.Value.Create(new Entities.UserTargetDonation
                        {
                            DonationFromUserId = fromUserId,
                            DonationTotal = total,
                            UserTargetId = target.Id
                        });
                        _unitOfWork.Value.SaveChanges();
                        result.Status = Common.Enums.OperationResultType.Success;
                        result.WidgetNotificationResult = new WidgetNotificationResult
                        {
                            Donation = total,
                            Code = target.Code,
                        };
                        result.TargetUserId = target.ApplicationUserId;
                        result.WalletDestintaionUserNotificationResult = new WalletBalanceChangedResponse(total, WalletOperationType.Income);
                        result.WalletSenderUserNotificationResult = new WalletBalanceChangedResponse(total, WalletOperationType.Outcome);
                    }
                }
            }
            else
            {
                result.Errors.Add("You cannot donate to yourself.");
            }

            if (result.Status == Common.Enums.OperationResultType.Failed && !result.Errors.Any())
                result.Errors.Add(CommonErrorMessage);
            return result;
        }

        public AddDonationResult AddDonation(string toUserName, string fromUserId, decimal total, string targetCode)
        {
            var userId = _applicationUserDataService.Value.Query().Where(x => x.UserName.ToLower() == toUserName.ToLower()).Select(x => x.Id).FirstOrDefault();
            var result = new AddDonationResult();
            if (userId == null)
                result.Errors.Add("Check username/target code");
            else
            {
                AddDonationForUserId(userId, fromUserId, total, targetCode, result);
            }
            return result;
        }

        public ListResponse<WidgetUserTargetDonation> GetDonations(string userId)
        {
            var result = new ListResponse<WidgetUserTargetDonation>();
            const decimal AvailableDonationTotal = 100;

            var targets = _userTargetDataService.Value.Query().Where(x => x.ApplicationUserId == userId).ToList();
            foreach (var item in targets)
            {
                var resTarget = new WidgetUserTargetDonation
                {
                    DestinationTargetTotal = item.TargetRequiredTotal,
                    TargetName = item.Name,
                    Code = item.Code
                };
                var donations = _userTargetDonationDataService.Value.Query().ToList();
                var lastDonate = _userTargetDonationDataService.Value.Query().Where(x => x.UserTargetId == item.Id)
                    .OrderByDescending(x => x.Id).FirstOrDefault();
                if (lastDonate != null)
                {
                    var total = _userTargetDonationDataService.Value.Query().Where(x => x.UserTargetId == item.Id)
                       .Sum(x => x.DonationTotal);
                    resTarget.ActualTotal = total;
                    resTarget.LastDonateTotal = lastDonate.DonationTotal;
                }

                resTarget.AvailableDonates.Add(new AvailableDonate { DonateTotal = AvailableDonationTotal, Token = GetEncryptedTargetDonationInfo(userId, resTarget.Code, AvailableDonationTotal) });
                result.Items.Add(resTarget);
            }

            return result;
        }

        public AddDonationResult AddDonationFromWidgetByToken(string token, string fromUser)
        {
            var decripted = DecryptDonationTokenString(token);
            var result = AddDonationForUserId(decripted.TargetUserId, fromUser, decripted.DonationTotal, decripted.TargetCode);
            return result;
        }

        private string GetEncryptedTargetDonationInfo(string userId, string targetCode, decimal donatinTotal)
        {
            string str = $"{userId}||{targetCode}||{donatinTotal}";
            var result = Helpers.StringCipher.Encrypt(str, TargetDonationKey);
            return result;
        }

        private AddDonationFromWidgetDecriptedModel DecryptDonationTokenString(string encrypted)
        {
            AddDonationFromWidgetDecriptedModel res = null;
            var decryptedStr = Helpers.StringCipher.Decrypt(encrypted, TargetDonationKey);
            var parts = decryptedStr.Split(new[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 3)
            {
                res = new AddDonationFromWidgetDecriptedModel();
                res.TargetUserId = parts[0];
                res.TargetCode = parts[1];
                res.DonationTotal = decimal.Parse(parts[2]);
            }
            return res;
        }
    }


    class AddDonationFromWidgetDecriptedModel
    {
        public string TargetUserId { get; set; }
        public string TargetCode { get; set; }
        public decimal DonationTotal { get; set; }
    }

}
