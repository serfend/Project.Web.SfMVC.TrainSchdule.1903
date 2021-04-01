using Abp.Linq.Expressions;
using BLL.Extensions;
using BLL.Extensions.ApplyExtensions.ApplyAuditStreamExtension;
using BLL.Extensions.Common;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.ApplyInfo;
using BLL.Interfaces.Audit;
using DAL.Data;
using DAL.DTO.Apply;
using DAL.Entities.ApplyInfo;
using DAL.Entities.ApplyInfo.DailyApply;
using DAL.Entities.UserInfo;
using DAL.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.ApplyServices.DailyApply
{
    public partial class ApplyIndayService
    {
        public IEnumerable<ApplyInday> QueryApplies(QueryApplyDataModel model, bool getAllAppliesPermission, out int totalCount)
        {
            var db = context.AppliesIndayDb;
            var list = db.AsQueryable();
            totalCount = 0;
            if (model == null) return null;
            if (model.Status != null) list = list.Where(a => (model.Status.Arrays != null && model.Status.Arrays.Contains((int)a.Status)) || (model.Status.Start <= (int)a.Status && model.Status.End >= (int)a.Status));
            if (model.MainStatus != null) list = list.Where(a => (int)a.MainStatus == model.MainStatus.Start);
            if (model.ExecuteStatus?.Value != null)
            {
                var success = int.TryParse(model.ExecuteStatus.Value, out var executeStatusInt);
                list = list.Where(a => (int)a.ExecuteStatus == executeStatusInt);
            }
            if (model.NowAuditBy != null) list = list.Where(a => a.NowAuditStep.MembersFitToAudit.Contains(model.NowAuditBy.Value));
            if (model.AuditBy != null) list = list.Where(a => a.ApplyAllAuditStep.Any(s => s.MembersFitToAudit.Contains(model.AuditBy.Value)));
            if (model.UserStatus != null)
            {
                var status = model.UserStatus.Arrays.FirstOrDefault();
                list = list.Where(a => (a.BaseInfo.From.SocialInfo.Status & status) > 0);
            }
            if (model.CompanyStatus != null)
            {
                var status = model.CompanyStatus.Arrays.FirstOrDefault();
                list = list.Where(a => ((int)a.BaseInfo.Company.CompanyStatus & status) > 0);
            }
            if (model.CompanyType != null)
                list = list.Where(a => a.BaseInfo.Company.Tag.Contains(model.CompanyType.Value));
            if (model.DutiesType != null)
                list = list.Where(a => a.BaseInfo.Duties.Tags.Contains(model.DutiesType.Value));
            if (model.CreateCompany != null)
            {
                var arr = model.CreateCompany?.Arrays;
                var exp = PredicateBuilder.New<ApplyInday>(false);
                foreach (var item in arr)
                    exp = exp.Or(p => p.BaseInfo.CompanyCode.StartsWith(item));
                if (arr != null)
                    list = list.Where(a => a.BaseInfo != null)
                        .Where(a => a.BaseInfo.From != null)
                        .Where(a => a.BaseInfo.Company != null)
                        .Where(exp);
            }

            bool anyDateFilterIsLessThan30Days = false;
            if (model.Create != null)
            {
                list = list.Where(a => (a.Create >= model.Create.Start && a.Create <= model.Create.End));
                anyDateFilterIsLessThan30Days |= model.Create.End.Subtract(model.Create.Start).Days <= 360;
            }

            ////  默认查询到下周六前的的申请
            //if (model.StampLeave == null)
            //{
            //	var thisFri = DayOfWeek.Friday;
            //	var nowDay = DateTime.Now;
            //	model.StampLeave = new QueryByDate()
            //	{
            //		Start = nowDay,
            //		End = nowDay.AddDays(thisFri - nowDay.DayOfWeek).AddDays(8)
            //	};
            //}
            if (model.StampLeave?.Start != null) list = list.Where(a => (a.RequestInfo.StampLeave >= model.StampLeave.Start && a.RequestInfo.StampLeave <= model.StampLeave.End));
            anyDateFilterIsLessThan30Days |= (model.StampLeave == null || model.StampLeave.End.Subtract(model.StampLeave.Start).Days <= 360);

            if (model.StampReturn != null)
            {
                list = list.Where(a => (a.RequestInfo.StampReturn >= model.StampReturn.Start && a.RequestInfo.StampReturn <= model.StampReturn.End));
                anyDateFilterIsLessThan30Days |= model.StampReturn.End.Subtract(model.StampReturn.Start).Days <= 360;
            }
            if (!getAllAppliesPermission && !anyDateFilterIsLessThan30Days)
            {
                var yearFirstDay = new DateTime(DateTime.Now.XjxtNow().Year, 1, 1);
                list = list.Where(a => a.RequestInfo.StampLeave >= yearFirstDay); //默认返回今年以来所有假期
            }
            // 若精确按id或按人查询，则直接导出
            if (model.CreateBy != null)
            {
                list = db.Where(a => a.BaseInfo.CreateById == model.CreateBy.Value || a.BaseInfo.CreateBy.BaseInfo.RealName == (model.CreateBy.Value));
            }
            else if (model.CreateFor != null)
            {
                list = db.Where(a => a.BaseInfo.FromId == model.CreateFor.Value || a.BaseInfo.From.BaseInfo.RealName == (model.CreateFor.Value));
            }
            list = list.OrderByDescending(a => a.Create).ThenByDescending(a => a.Status);
            var result = list.SplitPage(model.Pages);
            totalCount = result.Item2;
            return result.Item1;
        }
    }
    public partial class ApplyIndayService : IApplyInDayService
    {
        private readonly ApplicationDbContext context;
        private readonly IUsersService usersService;
        private readonly IAuditStreamServices auditStreamServices;

        public ApplyIndayService(ApplicationDbContext context, IUsersService usersService, IAuditStreamServices auditStreamServices)
        {
            this.context = context;
            this.usersService = usersService;
            this.auditStreamServices = auditStreamServices;
        }
        public IQueryable<ApplyInday> CheckIfHaveSameRangeVacation(ApplyInday apply)
        {
            var r = apply.RequestInfo;
            var list = new List<AuditStatus>() {
                    AuditStatus.Accept,
                    AuditStatus.AcceptAndWaitAdmin,
                    AuditStatus.Auditing
            };
            var userid = apply.BaseInfo.FromId;
            var recallDb = context.RecallOrders;
            var execDb = context.ApplyExcuteStatus;

            var exp = PredicateBuilder.New<ApplyInday>(false);
            // 存在确认时间，则判断确认时间
            exp = exp.Or(a => a.RecallId == null && a.ExecuteStatusDetailId != null && !(execDb.FirstOrDefault(exec => exec.Id == a.ExecuteStatusDetailId).ReturnStamp <= r.StampLeave || a.RequestInfo.StampLeave >= r.StampReturn));
            // 不存在召回时间，则判断应归队时间（必定不晚于确认时间）
            exp = exp.Or(a => a.ExecuteStatusDetailId == null && a.RecallId == null && !(a.RequestInfo.StampLeave >= r.StampReturn || a.RequestInfo.StampReturn <= r.StampLeave));
            // 如果存在召回，则判断召回时间
            exp = exp.Or(a => a.RecallId != null && !(recallDb.FirstOrDefault(rec => rec.Id == a.RecallId).ReturnStamp <= r.StampLeave || a.RequestInfo.StampLeave >= r.StampReturn));
            /* 20200917@胡琪blanche881
			 * 两个日期范围存在冲突的条件：
			!(A2<=B1||B2<=A1)
			*
			*/
            var userVacationsInTime = context.AppliesIndayDb
                .Where(a => a.BaseInfo.FromId == userid)
                .Where(exp)
                .Where(a => list.Contains(a.Status));
            return userVacationsInTime;
        }

        public ApplyInday Create(ApplyInday item)
        {
            if (item == null) return null;
            var appSetting = item.BaseInfo.From.Application.ApplicationSetting;
            if (appSetting != null)
            {
                var time = appSetting.LastSubmitApplyTime;
                // 若1分钟内连续提交两次，则下次提交限定到10分钟后
                if (time == null) appSetting.LastSubmitApplyTime = DateTime.Now;
                else if (time > DateTime.Now.AddMinutes(10)) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Operation.Submit.Crash);
                else if (time?.AddMinutes(1) > DateTime.Now)
                    appSetting.LastSubmitApplyTime = DateTime.Now.AddMinutes(20);
            }
            context.AppliesInday.Add(item);
            context.AppUserApplicationSettings.Update(appSetting);
            context.SaveChanges();
            return item;
        }

        public async Task Delete(ApplyInday item) => await RemoveApplies(new List<ApplyInday>() { item }).ConfigureAwait(true);


        public bool Edit(string id, Action<ApplyInday> editCallBack) => EditAsync(id, editCallBack).Result;
        public async Task<bool> EditAsync(string id, Action<ApplyInday> editCallBack)
        {
            if (editCallBack == null) return false;
            if (!Guid.TryParse(id, out var guid)) return false;
            var target = context.AppliesIndayDb.Where(a => a.Id == guid).FirstOrDefault();
            if (target == null) return false;
            await Task.Run(() => editCallBack.Invoke(target)).ConfigureAwait(true);
            context.AppliesInday.Update(target);
            await context.SaveChangesAsync().ConfigureAwait(true);
            return true;
        }

       

        public ApplyInday GetById(Guid id) => context.AppliesIndayDb.Where(a => a.Id == id).FirstOrDefault();


        public async Task RemoveApplies(IEnumerable<ApplyInday> list)
        {
            if (list == null) return;
            bool anyRemove = false;
            foreach (var s in list)
            {
                s.Remove();
                context.AppliesInday.Update(s);
                anyRemove = true;
            }
            if (anyRemove)
                await context.SaveChangesAsync().ConfigureAwait(true);
        }

        public ApplyInday Submit(ApplyVdto model)
        {
            if (model == null) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Default);
            var apply = new ApplyInday()
            {
                BaseInfo = context.ApplyBaseInfos.Find(model.BaseInfoId),
                Create = DateTime.Now,
                RequestInfo = context.ApplyIndayRequests.Find(model.RequestInfoId),
                Status = AuditStatus.NotSave,
                MainStatus = model.IsPlan ? MainStatus.IsPlan : MainStatus.Normal
            };
            if (apply.RequestInfo == null) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Operation.Submit.NoRequestInfo);
            if (apply.BaseInfo == null) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Operation.Submit.NoBaseInfo);
            var company = apply.BaseInfo?.Company;
            if (company == null) throw new ActionStatusMessageException(ActionStatusMessage.CompanyMessage.NotExist);
            AuditStreamModel auditItem = apply.ToModel();
            auditStreamServices.InitAuditStream(ref auditItem, $"{apply.RequestInfo.RequestType}|{model.EntityType}", apply.BaseInfo?.From);
            apply = auditItem.ToModel(apply);
            apply = Create(apply); // 创建成功，记录本次创建详情
            return apply;
        }

        public ApplyIndayRequest SubmitRequestAsync(User targetUser, ApplyIndayRequestVdto model)
        {

            if (model == null) return null;
			var type = model.RequestType ?? throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Request.VacationTypeNotExist);
            if (type.Disabled) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Request.InvalidVacationType);
            if (type.PermitCrossDay < (int)(model.StampReturn?.Subtract(model.StampLeave ?? DateTime.MinValue).TotalDays)) {
                var desc = type.PermitCrossDay <= 0 ? "不允许跨日请假" : $"最多允许请假{type.PermitCrossDay}天";
                throw new ActionStatusMessageException(new ApiResult(ActionStatusMessage.ApplyMessage.Request.CrossDayNotPermit, desc, true));
            }
            // TODO Trace type.NeedTrace
            var r = new ApplyIndayRequest()
            {
                Reason = model.Reason,
                RequestType = model.RequestType.Name,
                StampLeave = model.StampLeave,
                StampReturn = model.StampReturn,
                VacationPlace = model.VacationPlace,
                VacationPlaceName = model.VacationPlaceName,
                CreateTime = DateTime.Now,
                ByTransportation = model.ByTransportation
            };
            context.ApplyIndayRequests.Add(r);
            context.SaveChanges();
            return r;
        }
    }
    public partial class ApplyIndayService
    {
        public byte[] ExportExcel(string templete, ApplyDetailDto<ApplyIndayRequest> model)
        {
            throw new NotImplementedException();
        }

        public byte[] ExportExcel(string templete, IEnumerable<ApplyDetailDto<ApplyIndayRequest>> model)
        {
            throw new NotImplementedException();
        }
    }
    public partial class ApplyIndayService
    {

        public async Task<int> RemoveAllRemovedUsersApply()
        {
            var applies = context.AppliesIndayDb;
            var to_remove = applies.Where(a =>
                 ((int)a.BaseInfo.From.AccountStatus & (int)AccountStatus.Abolish) > 0
                  //|| ((int)a.BaseInfo.From.AccountStatus & (int)AccountStatus.DisableVacation) > 0
                  || ((int)a.BaseInfo.From.AccountStatus & (int)AccountStatus.PrivateAccount) > 0
            //a.BaseInfo.From.CompanyInfo.Title.DisableVacation
            );
            await RemoveApplies(to_remove).ConfigureAwait(false);

            var list = applies.Where(a => a.BaseInfo.From == null);
            await RemoveApplies(list).ConfigureAwait(true);
            return to_remove.Count();
        }

        public async Task<int> RemoveAllUnSaveApply(TimeSpan interval)
        {
            var applies = context.AppliesIndayDb;
            var outofDate = DateTime.Now.Subtract(interval);
            //寻找所有找过1天未保存的申请
            var list = applies
                         .Where(a => a.Status == AuditStatus.NotSave)
                         .Where(a => a.Create.HasValue && a.Create.Value < outofDate).ToList();
            await RemoveApplies(list).ConfigureAwait(true);
            return list.Count;
        }

    }
}
