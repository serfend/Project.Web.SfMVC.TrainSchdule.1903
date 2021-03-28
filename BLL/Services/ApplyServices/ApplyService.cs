using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
using DAL.Entities.UserInfo;
using DAL.Entities.Vacations;
using DAL.QueryModel;
using ExcelReport;
using ExcelReport.Driver.NPOI;

namespace BLL.Services.ApplyServices
{
	public partial class ApplyService : IApplyVacationService
	{
		#region Fileds

		private readonly ApplicationDbContext context;
        private readonly IUsersService usersService;
        private readonly ICompanyManagerServices companyManagerServices;
		private readonly IVacationCheckServices vacationCheckServices;
        private readonly IAuditStreamServices auditStreamServices;

        #endregion Fileds

        public ApplyService(ApplicationDbContext context, IUsersService usersService, ICompanyManagerServices companyManagerServices, IVacationCheckServices vacationCheckServices, IAuditStreamServices auditStreamServices)
		{
			this.context = context;
            this.usersService = usersService;
            new Configurator()[".xlsx"] = new WorkbookLoader();
			this.companyManagerServices = companyManagerServices;
			this.vacationCheckServices = vacationCheckServices;
            this.auditStreamServices = auditStreamServices;
        }

		public Apply GetById(Guid id) => context.AppliesDb.Where(a => a.Id == id).FirstOrDefault();

		public Apply Create(Apply item)
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
			context.Applies.Add(item);
			context.AppUserApplicationSettings.Update(appSetting);
			context.SaveChanges();
			return item;
		}

        private const string Const_LawVacationDescription = "法定节假日";

		public ApplyRequest SubmitRequestAsync(User targetUser, ApplyRequestVdto model)
		{
			if (model == null) return null;
			var vacationInfo = usersService.VacationInfo(targetUser, model?.StampLeave?.Year ?? DateTime.Now.XjxtNow().Year, model.IsPlan ? MainStatus.IsPlan : MainStatus.Normal);
			if (vacationInfo.Description == null || vacationInfo.Description.Contains("无休假")) throw new ActionStatusMessageException(new ApiResult(ActionStatusMessage.ApplyMessage.Request.HaveNoVacationSinceExcept, vacationInfo.Description ?? "休假信息生效中", true));
			model = CaculateVacation(model);
			var type = model.VacationType;
			if (type?.Disabled ?? true) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Request.InvalidVacationType);
			if (type.Primary)
			{
				// 剩余天数判断
				if (model.VacationLength > vacationInfo.LeftLength) throw new ActionStatusMessageException(new ApiResult(ActionStatusMessage.ApplyMessage.Request.NoEnoughVacation, $"超出{model.VacationLength - vacationInfo.LeftLength}天", true));
				// 剩余路途次数判断
				if (model.OnTripLength < 0) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Request.Default);
			}
			// 不允许未休完正休假前休某些假
			else if (!type.AllowBeforePrimary && vacationInfo.LeftLength > 0)
				throw new ActionStatusMessageException(new ApiResult(ActionStatusMessage.ApplyMessage.Request.BeforePrimaryNotAllow, $"({type.Alias})", true));
			// 路途判断
			if (type.CanUseOnTrip)
			{
				if (model.OnTripLength > 0 && vacationInfo.MaxTripTimes <= vacationInfo.OnTripTimes)
					throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Request.TripTimesExceed);
			}
			else model.OnTripLength = 0;
			// 休假天数范围判断
			if (model.VacationLength < type.MinLength) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Request.VacationLengthTooShort);
			if (model.VacationLength > type.MaxLength) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Request.VacationLengthTooLong);
			// 福利假判断
			if (!type.CaculateBenefit)
				model.VacationAdditionals = null;
			// 跨年假判断
			if (type.NotPermitCrossYear)
				if (model.StampReturn.Value.Year != model.StampLeave.Value.Year) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Request.NotPermitCrossYear);

			var r = new ApplyRequest()
			{
				OnTripLength = model.OnTripLength,
				Reason = model.Reason,
				StampLeave = model.StampLeave,
				StampReturn = model.StampReturn,
				VacationLength = model.VacationLength,
				VacationPlace = model.VacationPlace,
				VacationPlaceName = model.VacationPlaceName,
				VacationType = model.VacationType.Name,
				CreateTime = DateTime.Now,
				ByTransportation = model.ByTransportation,
				AdditialVacations = model.VacationAdditionals,
				VacationDescription = vacationInfo.Description
			};
			context.ApplyRequests.Add(r);
			context.SaveChanges();
			return r;
		}

		public Apply Submit(ApplyVdto model)
		{
			if (model == null) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Default);
			var apply = new Apply()
			{
				BaseInfo = context.ApplyBaseInfos.Find(model.BaseInfoId),
				Create = DateTime.Now,
				RequestInfo = context.ApplyRequests.Find(model.RequestInfoId),
				Status = AuditStatus.NotSave,
				MainStatus = model.IsPlan ? MainStatus.IsPlan : MainStatus.Normal
			};
			if (apply.RequestInfo == null) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Operation.Submit.NoRequestInfo);
			if (apply.BaseInfo == null) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Operation.Submit.NoBaseInfo);
			var company = apply.BaseInfo?.Company;
			if (company == null) throw new ActionStatusMessageException(ActionStatusMessage.CompanyMessage.NotExist);
			AuditStreamModel auditItem = apply.ToModel();
			auditStreamServices.InitAuditStream(ref auditItem, model.EntityType, apply.BaseInfo?.From);
			apply = auditItem.ToModel(apply);
			apply =  Create(apply); // 创建成功，记录本次创建详情
			return apply;
		}

		/// <summary>
		/// 检查是否存在重复的时间范围的申请
		/// </summary>
		public IQueryable<Apply> CheckIfHaveSameRangeVacation(Apply apply)
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

			var exp = PredicateBuilder.New<Apply>(false);
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
			var userVacationsInTime = context.AppliesDb
				.Where(a => a.BaseInfo.FromId == userid)
				.Where(exp)
				.Where(a => list.Contains(a.Status));
			return userVacationsInTime;
		}

		public ApplyRequestVdto CaculateVacation(ApplyRequestVdto model)
		{
			if (model == null) return null;
			var type = model.VacationType;
			if (type == null) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Request.VacationTypeNotExist);
			// 检查是否合法
			model.VacationAdditionals?.All(v =>
			{
				if (v.Description == Const_LawVacationDescription)
					throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Request.LawVacationCantCreateByUser);
				return true;
			});
			if (model.StampLeave != null)
			{
				var vacationLength = model.VacationLength;
				if (type.CanUseOnTrip) vacationLength += model.OnTripLength;
				// 仅计算正休假包含的法定节假日
				if (type.CaculateBenefit)
				{
					// 得到所有福利假后需要加入路途和原假并再次计算
					model.StampReturn = vacationCheckServices.CrossVacation(model.StampLeave.Value, vacationLength, true, model.LawVacationSet);
					List<VacationAdditional> lawVacations = vacationCheckServices.VacationDesc.Select(v => new VacationAdditional()
					{
						Name = v.Name,
						Start = v.Start,
						Length = v.UseLength,
						Description = Const_LawVacationDescription
					}).ToList();
					if (model.VacationAdditionals != null) lawVacations.AddRange(model.VacationAdditionals);
					model.VacationAdditionals = lawVacations;//执行完crossVacation后已经处于加载完毕状态可直接使用
					vacationLength += model.VacationAdditionals.Sum(v => v.Length);
				}
				// 到归队日期前一天的23:59:59
				model.StampReturn = model.StampLeave.Value.AddDays(vacationLength).AddSeconds(-1);

				model.VacationDescriptions = vacationCheckServices.VacationDesc.CombineVacationDescription();
			}
			return model;
		}
		public bool Edit(string id, Action<Apply> editCallBack) => EditAsync(id, editCallBack).Result;

		public async Task<bool> EditAsync(string id, Action<Apply> editCallBack)
		{
			if (editCallBack == null) return false;
			if (!Guid.TryParse(id, out var guid)) return false;
			var target = context.AppliesDb.Where(a => a.Id == guid).FirstOrDefault();
			if (target == null) return false;
			await Task.Run(() => editCallBack.Invoke(target)).ConfigureAwait(true);
			context.Applies.Update(target);
			await context.SaveChangesAsync().ConfigureAwait(true);
			return true;
		}


		public IEnumerable<Apply> QueryApplies(QueryApplyDataModel model, bool getAllAppliesPermission, out int totalCount)
		{
			var db = context.AppliesDb;
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
				var exp = PredicateBuilder.New<Apply>(false);
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
				list =db.Where(a => a.BaseInfo.CreateById == model.CreateBy.Value || a.BaseInfo.CreateBy.BaseInfo.RealName == (model.CreateBy.Value));
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

		public async Task<int> RemoveAllRemovedUsersApply()
		{
			var applies = context.Applies;
			var to_remove = applies.Where(a =>
				 ((int)a.BaseInfo.From.AccountStatus & (int)AccountStatus.Abolish) > 0
				  //|| ((int)a.BaseInfo.From.AccountStatus & (int)AccountStatus.DisableVacation) > 0
				  || ((int)a.BaseInfo.From.AccountStatus & (int)AccountStatus.PrivateAccount) > 0
			//a.BaseInfo.From.CompanyInfo.Title.DisableVacation
			);
			await RemoveApplies(to_remove).ConfigureAwait(false);

			var list = context.AppliesDb.Where(a => a.BaseInfo.From == null);
			await RemoveApplies(list).ConfigureAwait(true);
			return to_remove.Count();
		}
		public async Task<int> RemoveAllUnSaveApply(TimeSpan interval)
		{
			var outofDate = DateTime.Now.Subtract(interval);
			//寻找所有找过1天未保存的申请
			var list = context.AppliesDb
						 .Where(a => a.Status == AuditStatus.NotSave)
						 .Where(a => a.Create.HasValue && a.Create.Value < outofDate).ToList();
			await RemoveApplies(list).ConfigureAwait(true);
			return list.Count;
		}

		public async Task RemoveApplies(IEnumerable<Apply> list)
		{
			if (list == null) return;
			bool anyRemove = false;
			foreach (var s in list)
			{
				s.Remove();
				context.Applies.Update(s);
				anyRemove = true;
			}
			if (anyRemove)
				await context.SaveChangesAsync().ConfigureAwait(true);
		}
		public async Task Delete(Apply item)=>	await RemoveApplies(new List<Apply>() { item }).ConfigureAwait(true);

		public IEnumerable<Apply> Find(Func<Apply, bool> predict)
		{
			var list = context.AppliesDb.Where(predict);
			return list;
		}
	}
}