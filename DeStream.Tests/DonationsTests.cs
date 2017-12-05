using Autofac;
using DeStream.Web.Services;
using DeStream.Web.Services.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Tests
{
    [TestClass]
    public class DonationsTests
    {
        private const string UserId= "1d6a4e4a-d83e-44c6-a16c-01ed3bc9b286";
        public DonationsTests()
        {
            DependencyConfig.Configure();
        }

        [TestMethod]
        public void GetDonations()
        {
            var service = DependencyConfig.Container.Resolve<IUserTargetDonationsService>();
            var res = service.GetDonations(UserId);
        }

        [TestMethod]
        public void AddDonation()
        {
            var service = DependencyConfig.Container.Resolve<IUserTargetDonationsService>();
            service.AddDonation("test@test.ru", "alex", 135, "123456");
        }

        [TestMethod]
        public void GetTargets()
        {
            var service = DependencyConfig.Container.Resolve<IUserTargetService>();
            var targets = service.GetUserTargets(UserId);
        }

        [TestMethod]
        public void SaveTarget()
        {
            var service = DependencyConfig.Container.Resolve<IUserTargetService>();
            var newTarget = new UserTarget();
            newTarget.Code = "123456";
            newTarget.Name = "Target1";
            newTarget.TargetRequiredTotal = 100;
            var target = service.SaveUserTarget(UserId,newTarget);
        }

        [TestMethod]
        public void UpdateTarget()
        {
            var service = DependencyConfig.Container.Resolve<IUserTargetService>();
            var targets = service.GetUserTargets(UserId);
            if(targets.Any())
            {
                var firstTarget = targets.First();
                firstTarget.Name = "lololo";
                firstTarget.TargetRequiredTotal = 2000;
                var res= service.SaveUserTarget(UserId, firstTarget);
            }
        }
    }
}
