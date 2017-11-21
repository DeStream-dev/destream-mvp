using DeStream.Web.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Web.Services
{
    public interface IUserService
    {
        Task<bool> Create(string email,string userName, string password);
        Task<bool> SaveProfile(string userId, UserProfile profile);
        Task<ClaimsIdentity> Auth(string email, string password);
        UserInfo GetUserInfo(string userId);
    }
}
