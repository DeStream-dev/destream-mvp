using DeStream.Web.Services.Models;
using DeStream.Web.Services.Models.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Web.Services
{
    public interface IUserTargetService
    {
        UserTarget SaveUserTarget(string userId, UserTarget target);
        List<UserTarget> GetUserTargets(string userId);
        bool DeleteUserTarget(string userId, long targetId);
        SaveUserTargetsResult SaveUserTargets(string userId, List<UserTarget> targets);
    }
}
