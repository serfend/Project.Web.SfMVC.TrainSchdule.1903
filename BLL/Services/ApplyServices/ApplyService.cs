using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp.Linq.Expressions;
using BLL.Extensions;
using BLL.Extensions.ApplyExtensions.ApplyAuditStreamExtension;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.ApplyInfo;
using BLL.Interfaces.Audit;
using DAL.Data;
using DAL.DTO.Apply;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using DAL.Entities.Vacations;
using ExcelReport;
using ExcelReport.Driver.NPOI;

namespace BLL.Services.ApplyServices
{
	public partial class ApplyService : IApplyVacationService
	{
		#region Fileds

		private readonly ApplicationDbContext _context;
        private readonly IUsersService usersService;
        private readonly ICompanyManagerServices companyManagerServices;
		private readonly IVacationCheckServices vacationCheckServices;
        private readonly IAuditStreamServices auditStreamServices;

        #endregion Fileds

        public ApplyService(ApplicationDbContext context, IUsersService usersService, ICompanyManagerServices companyManagerServices, IVacationCheckServices vacationCheckServices, IAuditStreamServices auditStreamServices)
		{
			_context = context;
            this.usersService = usersService;
            new Configurator()[".xlsx"] = new WorkbookLoader();
			this.companyManagerServices = companyManagerServices;
			this.vacationCheckServices = vacationCheckServices;
            this.auditStreamServices = auditStreamServices;
        }

		public Apply GetById(Guid id) => _context.AppliesDb.Where(a => a.Id == id).FirstOrDefault();

		public IEnumerable<Apply> GetAll(int page, int pageSize) => GetAll((item) => true, page, pageSize);

		public IEnumerable<Apply> GetAll(string userid, int page, int pageSize)
		{
			return GetAll((item) => item.BaseInfo.FromId == userid, page, pageSize);
		}

		public IEnumerable<Apply> GetAll(string userid, AuditStatus status, int page, int pageSize)
		{
			return GetAll((item) => item.BaseInfo.FromId == userid && status == item.Status, page, pageSize);
		}

		public IEnumerable<Apply> GetAll(Expression<Func<Apply, bool>> predicate, int page, int pageSize)
		{
			var items = _context.AppliesDb.Where(predicate).OrderByDescending(a => a.Status).ThenByDescending(a => a.Create).Skip(page * pageSize).Take(pageSize);
			return items;
		}

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
			_context.Applies.Add(item);
			_context.AppUserApplicationSettings.Update(appSetting);
			_context.SaveChanges();
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
			_context.ApplyRequests.Add(r);
			_context.SaveChanges();
			return r;
		}

		public Apply Submit(ApplyVdto model)
		{
			if (model == null) return null;
			var apply = new Apply()
			{
				BaseInfo = _context.ApplyBaseInfos.Find(model.BaseInfoId),
				Create = DateTime.Now,
				RequestInfo = _context.ApplyRequests.Find(model.RequestInfoId),
				Status = AuditStatus.NotSave,
				MainStatus = model.IsPlan ? MainStatus.IsPlan : MainStatus.Normal
			};
			if (apply.BaseInfo == null || apply.RequestInfo == null) return apply;
			var company = apply.BaseInfo?.Company;
			if (company == null) return apply;
			AuditStreamModel auditItem = apply.ToModel();
			auditStreamServices.InitAuditStream(ref auditItem, model.EntityType, apply.BaseInfo?.From);
			apply = auditItem.ToModel(apply);
			return Create(apply); // 创建成功，记录本次创建详情
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
			var recallDb = _context.RecallOrders;
			var execDb = _context.ApplyExcuteStatus;

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
			var userVacationsInTime = _context.AppliesDb
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
		public bool Edit(string id, Action<Apply> editCallBack)
		{
			if (editCallBack == null) return false;
			if (!Guid.TryParse(id, out var guid)) return false;
			var target = _context.AppliesDb.Where(a => a.Id == guid).FirstOrDefault();
			if (target == null) return false;
			editCallBack.Invoke(target);
			_context.Applies.Update(target);
			_context.SaveChanges();
			return true;
		}

		public async Task<bool> EditAsync(string id, Action<Apply> editCallBack)
		{
			if (editCallBack == null) return false;
			if (!Guid.TryParse(id, out var guid)) return false;
			var target = _context.AppliesDb.Where(a => a.Id == guid).FirstOrDefault();
			if (target == null) return false;
			await Task.Run(() => editCallBack.Invoke(target)).ConfigureAwait(true);
			_context.Applies.Update(target);
			await _context.SaveChangesAsync().ConfigureAwait(true);
			return true;
		}

		public async Task Delete(Apply item)=>	await RemoveApplies(new List<Apply>() { item }).ConfigureAwait(true);

		public IEnumerable<Apply> Find(Func<Apply, bool> predict)
		{
			var list = _context.AppliesDb.Where(predict);
			return list;
		}
	}
}