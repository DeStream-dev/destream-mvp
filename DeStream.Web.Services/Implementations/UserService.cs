using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DeStream.Web.Services.Models;
using DeStream.Web.Entities.Data.Services.Identity;
using DeStream.Web.Entities.Data.Services;
using AutoMapper;
using Microsoft.AspNet.Identity;

namespace DeStream.Web.Services.Implementations
{
    internal class UserService : IUserService
    {
        private readonly Lazy<ApplicationUserManager> _applicationUserManager;
        private readonly Lazy<IUserProfileDataService> _userProfileService;
        private readonly Lazy<IApplicationUserDataService> _applicationUserDataService;
        private readonly Lazy<IUnitOfWork> _unitOfWork;
        public UserService(Lazy<ApplicationUserManager> applicationUserManager,
            Lazy<IUserProfileDataService> userProfileService,
            Lazy<IUnitOfWork> unitOfWork, 
            Lazy<IApplicationUserDataService> applicationUserDataService)
        {
            _applicationUserManager = applicationUserManager;
            _userProfileService = userProfileService;
            _unitOfWork = unitOfWork;
            _applicationUserDataService = applicationUserDataService;
        }
        public async Task<ClaimsIdentity> Auth(string email, string password)
        {
            ClaimsIdentity claims = null;
            var appUser =_applicationUserDataService.Value.Query().FirstOrDefault(x=>x.Email==email);
            if (appUser != null && !appUser.LockoutEnabled)
            {
                var succ = _applicationUserManager.Value.PasswordHasher.VerifyHashedPassword(appUser.PasswordHash, password);
                if(succ==PasswordVerificationResult.Success)
                {
                    claims = await _applicationUserManager.Value.CreateIdentityAsync(appUser,
                        DefaultAuthenticationTypes.ApplicationCookie);
                }
            }
            return claims;
        }

        public async Task<bool> Create(string email, string userName, string password)
        {
            var result = false;
            var existed = _applicationUserDataService.Value.Query().FirstOrDefault(x => x.Email.ToLower() == email.ToLower()
            || x.UserName == userName.ToLower());
            if (existed == null)
            {
                var newUserResult= await _applicationUserManager.Value.CreateAsync(new Entities.Identity.ApplicationUser
                {
                    Email = email,
                    UserName = userName,
                }, password);
                result = newUserResult.Succeeded;
            }
            return result;
        }

        public async Task<bool> SaveProfile(string userId, UserProfile profile)
        {
            var result = false;
            var user = await _applicationUserManager.Value.FindByIdAsync(userId);
            if (user != null)
            {
                Entities.Identity.UserProfile dbprofile = _userProfileService.Value.Query().FirstOrDefault(x => x.ApplicationUserId == userId);
                if (dbprofile != null)
                    Mapper.Map(profile, dbprofile);
                else
                {
                    dbprofile = Mapper.Map<Entities.Identity.UserProfile>(profile);
                    dbprofile.ApplicationUserId = userId;
                    _userProfileService.Value.Create(dbprofile);
                }
                result = _unitOfWork.Value.SaveChanges() > 0;
            }
            return result;
        }

        public UserInfo GetUserInfo(string userId)
        {
            UserInfo info = null;
            
            var user = _applicationUserDataService.Value.Query(x => x.UserProfile).FirstOrDefault(x => x.Id == userId);
            if(user!=null)
            {
                info = new UserInfo();
                info.Email = user.Email;
                info.UserName = user.UserName;
                if (user.UserProfile != null)
                    info.UserProfile= Mapper.Map<UserProfile>(user.UserProfile);
            }
            return info;
        }
    }
}
