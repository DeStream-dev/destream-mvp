using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeStream.WebApi.Models;
using DeStream.WebApi.Models.Response;
using DeStream.Web.Entities.Data.Services;
using DeStream.Web.Services.Models.Result;

namespace DeStream.Web.Services.Implementations
{
    internal class UserTargetDonationsService : IUserTargetDonationsService
    {
        private readonly Lazy<IUnitOfWork> _unitOfWork;
        private readonly Lazy<IUserTargetDataService> _userTargetDataService;
        private readonly Lazy<IApplicationUserDataService> _applicationUserDataService;
        private readonly Lazy<IUserTargetDonationDataService> _userTargetDonationDataService;
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

        public AddDonationResult AddDonation(string toUserName, string fromUserId, decimal total, string targetCode)
        {
            const string CommonErrorMessage = "Please, check username or target code.";
            var result = new AddDonationResult();
            result.Status = Common.Enums.OperationResultType.Failed;
            var userId = _applicationUserDataService.Value.Query().Where(x => x.UserName.ToLower() == toUserName.ToLower()).Select(x => x.Id).FirstOrDefault();

            if (userId != null)
            {
                if (/*userId != fromUserId*/true)
                {
                    var fromUserExists = _applicationUserDataService.Value.Query().Any(x => x.Id == fromUserId);
                    if (fromUserExists)
                    {
                        var target = _userTargetDataService.Value.Query().FirstOrDefault(x => x.ApplicationUserId == userId
                          && x.Code == targetCode);
                        if (target != null)
                        {
                            var currentUserTotal = _userTargetDonationDataService.Value.Query().Where(x => x.UserTarget.ApplicationUserId == userId).Sum(x => (decimal?)x.DonationTotal) ?? 0;
                            _userTargetDonationDataService.Value.Create(new Entities.UserTargetDonation
                            {
                                DonationFromUserId = fromUserId,
                                DonationTotal = total,
                                UserTargetId = target.Id
                            });
                            _unitOfWork.Value.SaveChanges();
                            result.Status = Common.Enums.OperationResultType.Success;
                            result.SignalRResult = new WidgetNotificationResult
                            {
                                Donation = total,
                                Code = target.Code,
                                UserId =fromUserId //target.ApplicationUserId
                            };
                            result.WalletSignalRResult = new WalletBalanceChangedResponse
                            {
                                LastOperation = new WalletOperation
                                {
                                    OperationType = WalletOperationType.Income,
                                    Total = total
                                },
                                Total = currentUserTotal + total
                            };
                        }
                    }
                }
                else
                {
                    result.Errors.Add("You cannot donate to yourself.");
                }
            }
            if (result.Status == Common.Enums.OperationResultType.Failed && !result.Errors.Any())
                result.Errors.Add(CommonErrorMessage);
            return result;
        }

        public ListResponse<UserTargetDonation> GetDonations(string userId)
        {
            var result = new ListResponse<UserTargetDonation>();

            var targets = _userTargetDataService.Value.Query().Where(x => x.ApplicationUserId == userId).ToList();
            foreach (var item in targets)
            {
                var resTarget = new UserTargetDonation
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
                result.Items.Add(resTarget);
            }

            return result;
        }
    }
}
