using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeStream.Web.Services.Models;
using DeStream.Web.Entities.Data.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DeStream.Web.Services.Models.Result;

namespace DeStream.Web.Services.Implementations
{
    internal class UserTargetService : IUserTargetService
    {
        private readonly Lazy<IUnitOfWork> _unitOfWork;
        private readonly Lazy<IUserTargetDataService> _userTargetDataService;
        private readonly Lazy<IApplicationUserDataService> _applicationUserDataService;


        public UserTargetService(Lazy<IUnitOfWork> unitOfWork,
            Lazy<IUserTargetDataService> userTargetDataService,
            Lazy<IApplicationUserDataService> applicationUserDataService)
        {
            _unitOfWork = unitOfWork;
            _userTargetDataService = userTargetDataService;
            _applicationUserDataService = applicationUserDataService;
        }

        public bool DeleteUserTarget(string userId, long targetId)
        {
            var result = false;
            var dbTarget = _userTargetDataService.Value.Query().FirstOrDefault(x => x.ApplicationUserId == userId
            && x.Id == targetId);
            if (dbTarget != null)
            {
                _userTargetDataService.Value.Delete(dbTarget);
                result = _unitOfWork.Value.SaveChanges() > 0;
            }
            return result;
        }

        public UserTarget SaveUserTarget(string userId, UserTarget target)
        {
            Entities.UserTarget dbTarget = target.Id > 0 ? _userTargetDataService.Value.Query().FirstOrDefault(x => x.ApplicationUserId == userId
                  && x.Id == target.Id) : Mapper.Map<Entities.UserTarget>(target);
            if (dbTarget.Id == 0)
            {
                dbTarget.ApplicationUserId = userId;
                _userTargetDataService.Value.Create(dbTarget);
            }
            else
            {
                Mapper.Map(target, dbTarget);
                _userTargetDataService.Value.Update(dbTarget);
            }
            _unitOfWork.Value.SaveChanges();
            return Mapper.Map<UserTarget>(dbTarget);
        }

        public SaveUserTargetsResult SaveUserTargets(string userId, List<UserTarget> targets)
        {
            var res = new SaveUserTargetsResult();
            var grouppedByCode = targets.GroupBy(x => x.Code.ToLower().Trim()).ToList();
            var dups = grouppedByCode.Where(x => x.Count() > 1).ToList();
            if (dups.Any())
            {
                foreach (var item in dups)
                    res.Errors.Add($"Duplicate target codes detected: {item.Key}");
            }
            else
            {
                var dbTargets = _userTargetDataService.Value.Query().Where(x => x.ApplicationUserId == userId).ToList();
                foreach (var item in dbTargets)
                {
                    var itemFromClient = targets.FirstOrDefault(x => x.Id == item.Id);
                    if (itemFromClient != null)
                        Mapper.Map(itemFromClient, item);
                    else
                        _userTargetDataService.Value.Delete(item);
                }

                foreach (var item in targets.Where(x => x.Id == 0))
                {
                    var newDbItem = Mapper.Map<Entities.UserTarget>(item);
                    newDbItem.ApplicationUserId = userId;
                    _userTargetDataService.Value.Create(newDbItem);
                }
                _unitOfWork.Value.SaveChanges();
            }
            if (res.Errors.Any())
                res.Status = Common.Enums.OperationResultType.Failed;
            return res;
        }

        public List<UserTarget> GetUserTargets(string userId)
        {
            var targets = _userTargetDataService.Value.Query().Where(x => x.ApplicationUserId == userId).ProjectTo<UserTarget>().ToList();
            return targets;
        }
    }
}
