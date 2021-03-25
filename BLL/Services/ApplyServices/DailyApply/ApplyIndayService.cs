using BLL.Interfaces.ApplyInfo;
using DAL.DTO.Apply;
using DAL.Entities.ApplyInfo;
using DAL.Entities.ApplyInfo.DailyApply;
using DAL.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.ApplyServices.DailyApply
{
    public class ApplyIndayService : IApplyInDayService
    {
        public IQueryable<ApplyInday> CheckIfHaveSameRangeVacation(ApplyInday apply)
        {
            throw new NotImplementedException();
        }

        public ApplyInday Create(ApplyInday item)
        {
            throw new NotImplementedException();
        }

        public Task Delete(ApplyInday item)
        {
            throw new NotImplementedException();
        }

        public bool Edit(string id, Action<ApplyInday> editCallBack)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EditAsync(string id, Action<ApplyInday> editCallBack)
        {
            throw new NotImplementedException();
        }

        public byte[] ExportExcel(string templete, ApplyDetailDto<ApplyIndayRequest> model)
        {
            throw new NotImplementedException();
        }

        public byte[] ExportExcel(string templete, IEnumerable<ApplyDetailDto<ApplyIndayRequest>> model)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ApplyInday> Find(Func<ApplyInday, bool> predict)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ApplyInday> GetAll(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ApplyInday> GetAll(string userid, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ApplyInday> GetAll(string userid, AuditStatus status, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ApplyInday> GetAll(Expression<Func<ApplyInday, bool>> predicate, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public ApplyInday GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ApplyInday> QueryApplies(QueryApplyDataModel model, bool getAllAppliesPermission, out int totalCount)
        {
            throw new NotImplementedException();
        }

        public Task<int> RemoveAllNoneFromUserApply(TimeSpan interval)
        {
            throw new NotImplementedException();
        }

        public Task<int> RemoveAllRemovedUsersApply()
        {
            throw new NotImplementedException();
        }

        public Task<int> RemoveAllUnSaveApply(TimeSpan interval)
        {
            throw new NotImplementedException();
        }

        public Task RemoveApplies(IEnumerable<ApplyInday> Applies)
        {
            throw new NotImplementedException();
        }

        public ApplyInday Submit(ApplyVdto model)
        {
            throw new NotImplementedException();
        }
    }
}
