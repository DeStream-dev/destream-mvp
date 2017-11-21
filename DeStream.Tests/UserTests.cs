using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac;
using DeStream.Web.Entities.Data.Services;
using System.Linq;
using DeStream.Web.Services;

namespace DeStream.Tests
{
    [TestClass]
    public class UserTests
    {
        public UserTests()
        {
            DependencyConfig.Configure();
            
        }

        [TestMethod]
        public void TestMethod1()
        {
            using (var scope = DependencyConfig.Container.BeginLifetimeScope())
            {
                var service = scope.Resolve<IUserTargetDataService>();
                var all = service.Query().ToList();
                var usersService = scope.Resolve<IApplicationUserDataService>();
                var usrs = usersService.Query(x=>x.UserProfile).ToList();
            }
        }

        [TestMethod]
        public void RegisterUser()
        {
            using (var scope = DependencyConfig.Container.BeginLifetimeScope())
            {
                var pswd = "123123123";
                var us = scope.Resolve<IUserService>();
                var newUSer = new Web.Entities.Identity.ApplicationUser
                {
                    Email = "test2@test.ru",
                    UserName = "test2"
                };
                    var result = us.Create(newUSer.Email, newUSer.UserName, pswd).GetAwaiter().GetResult();
                
            }
        }

        [TestMethod]
        public void AuthUser()
        {
            using (var scope = DependencyConfig.Container.BeginLifetimeScope())
            {
                var pswd = "123123123";
                var us = scope.Resolve<IUserService>();
                var newUSer = new Web.Entities.Identity.ApplicationUser
                {
                    Email = "test2@test.ru",
                    UserName = "test2"
                };
                var result = us.Auth(newUSer.Email, pswd).GetAwaiter().GetResult();

            }
        }

        [TestMethod]
        public void GetProfile()
        {
            var service = DependencyConfig.Container.Resolve<IUserService>();
            var prof = service.GetUserInfo("ae518296-bfae-4eb1-ace4-49576fc4a77e");
        }

        [TestMethod]
        public void SaveProfile()
        {

            /*var service = DependencyConfig.Container.Resolve<IUserService>();
            var prof = service.GetUserInfo("5ff71c7e-6f7b-4d36-a903-f2b14f2cc82a");
            if(prof!=null)
                prof.DisplayName = "new name";
            else
            {
                prof = new Web.Services.Models.UserProfile();
                prof.DisplayName = "test test1";
                prof.PurseNumber = "123456789";
            }
            var res= service.SaveProfile("5ff71c7e-6f7b-4d36-a903-f2b14f2cc82a", prof).Result;*/
        }

    }
}
