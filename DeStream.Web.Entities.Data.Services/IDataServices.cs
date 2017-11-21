using DeStream.Web.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Web.Entities.Data.Services
{
    public interface IUserTargetDataService:IDataServiceExtended<UserTarget,long>
    {
    }
    public interface IUserTargetDonationDataService : IDataServiceExtended<UserTargetDonation, long>
    {
    }
    public interface IUserProfileDataService : IDataServiceBase<UserProfile>
    {
    }
    public interface IApplicationUserDataService:IDataServiceBase<ApplicationUser>
    {
    }
}
