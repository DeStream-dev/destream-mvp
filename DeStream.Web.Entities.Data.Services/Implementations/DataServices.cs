using DeStream.Web.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Web.Entities.Data.Services.Implementations
{
    internal class UserProfileDataService:DataServiceBase<UserProfile>, IUserProfileDataService
    {
        public UserProfileDataService(DeStreamWebDbContext dbContext) : base(dbContext) { }

    }
    internal class UserTargetDataService : DataServiceExtended<UserTarget,long>, IUserTargetDataService
    {
        public UserTargetDataService(DeStreamWebDbContext dbContext) : base(dbContext) { }
        public override void Create(UserTarget entity)
        {
            if (entity.CreatedOn == DateTime.MinValue)
                entity.CreatedOn = DateTime.Now;
            base.Create(entity);
        }
    }

    internal class UserTargetDonationDataService : DataServiceExtended<UserTargetDonation, long>, IUserTargetDonationDataService
    {
        public UserTargetDonationDataService(DeStreamWebDbContext dbContext) : base(dbContext) { }
        public override void Create(UserTargetDonation entity)
        {
            if (entity.CreatedOn == DateTime.MinValue)
                entity.CreatedOn = DateTime.Now;
            base.Create(entity);
        }
    }
    internal class ApplicationUserDataService : DataServiceBase<ApplicationUser>, IApplicationUserDataService
    {
        public ApplicationUserDataService(DeStreamWebDbContext dbContext) : base(dbContext)
        {
        }
    }
}
