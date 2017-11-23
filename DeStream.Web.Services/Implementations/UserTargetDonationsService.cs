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

        public AddDonationResult AddDonation(string toUserName, string fromUserName, decimal total, string targetCode)
        {
            const string CommonErrorMessage = "Please, check username or target code.";
            var result = new AddDonationResult();
            result.Status = Common.Enums.OperationResultType.Failed;
            var userId = _applicationUserDataService.Value.Query().Where(x => x.UserName.ToLower() == toUserName.ToLower()).Select(x => x.Id).FirstOrDefault();
            if(userId!=null)
            {
                var target = _userTargetDataService.Value.Query().FirstOrDefault(x => x.ApplicationUserId == userId
                  && x.Code == targetCode);
                if(target!=null)
                {
                        _userTargetDonationDataService.Value.Create(new Entities.UserTargetDonation
                        {
                            DonationFrom = fromUserName,
                            DonationTotal = total,
                            UserTargetId = target.Id
                        });
                        _unitOfWork.Value.SaveChanges();
                        result.Status = Common.Enums.OperationResultType.Success;
                        result.SignalRResult = new SignalRAddDonationNotificationResult
                        {
                            Donation = total,
                            Code = target.Code,
                            UserId = target.ApplicationUserId
                        };
                }
            }
            if (result.Status == Common.Enums.OperationResultType.Failed)
                result.Errors.Add(CommonErrorMessage);
            return result;
        }

        public ListResponse<UserTargetDonation> GetDonations(string userId)
        {
            var result = new ListResponse<UserTargetDonation>();
                
            var targets = _userTargetDataService.Value.Query().Where(x => x.ApplicationUserId == userId).ToList();
            foreach(var item in targets)
            {
                var resTarget = new UserTargetDonation
                {
                    DestinationTargetTotal = item.TargetRequiredTotal,
                    TargetName = item.Name,
                    Code=item.Code
                };
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
