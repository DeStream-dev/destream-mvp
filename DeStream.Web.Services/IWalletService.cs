using DeStream.WebApi.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Web.Services
{
    public interface IWalletService
    {
        Task<ApplicationApiUser> Authorize(string username, string password);
        WalletResponse GetWallet(string userId);
    }
}
