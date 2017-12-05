using DeStream.Web.Entities.Data.Services;
using DeStream.Web.Entities.Data.Services.Identity;
using DeStream.WebApi.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Web.Services.Implementations.ApplicationServices
{
    internal class WalletService:IWalletService
    {
        private readonly Lazy<ApplicationUserManager> _applicationUserManager;
        private readonly Lazy<IApplicationUserDataService> _applicationUserDataService;
        private readonly Lazy<IUserTargetDonationDataService> _userTargetDonationDataService;
        private readonly Lazy<IUnitOfWork> _unitOfWork;
        public WalletService(Lazy<ApplicationUserManager> applicationUserManager,
            Lazy<IUnitOfWork> unitOfWork,
            Lazy<IApplicationUserDataService> applicationUserDataService,
            Lazy<IUserTargetDonationDataService> userTargetDonationDataService)
        {
            _applicationUserManager = applicationUserManager;
            _unitOfWork = unitOfWork;
            _applicationUserDataService = applicationUserDataService;
            _userTargetDonationDataService = userTargetDonationDataService;
        }
        public async Task<ApplicationApiUser> Authorize(string username, string password)
        {
            ApplicationApiUser user = null;
            if(!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                var dbUser= await _applicationUserManager.Value.FindAsync(username, password);
                if(dbUser!=null)
                    user = new ApplicationApiUser { Uid = dbUser.Id, Username = dbUser.UserName };
            }
            return user;
        }

        public WalletResponse GetWallet(string userId)
        {
            WalletResponse wallet = null;
            if(!string.IsNullOrWhiteSpace(userId))
            {
                if(_applicationUserDataService.Value.Query().Any(x=>x.Id==userId))
                {
                    wallet = new WalletResponse();
                    var incomeTotal = _userTargetDonationDataService.Value.Query().Where(x => x.UserTarget.ApplicationUserId == userId).Sum(x =>(decimal?) x.DonationTotal)??0;
                    var outcome = 0;
                    if (incomeTotal >= outcome)
                        wallet.Total = incomeTotal - outcome;
                }
            }
            return wallet;
        }
    }
}
