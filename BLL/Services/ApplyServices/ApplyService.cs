using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Data;
using DAL.Entities.ApplyInfo;
using ExcelReport;
using ExcelReport.Driver.NPOI;

namespace BLL.Services.ApplyServices
{
	public partial class ApplyService : IApplyService
	{
		#region Fileds

		private readonly ApplicationDbContext _context;
		private readonly ICompanyManagerServices companyManagerServices;
		private readonly IVacationCheckServices vacationCheckServices;

		#endregion Fileds

		public ApplyService(ApplicationDbContext context, IUsersService usersService, ICurrentUserService currentUserService, IApplyAuditStreamServices applyAuditStreamServices, ICompanyManagerServices companyManagerServices, IVacationCheckServices vacationCheckServices)
		{
			_context = context;
			_usersService = usersService;
			_currentUserService = currentUserService;
			new Configurator()[".xlsx"] = new WorkbookLoader();
			_applyAuditStreamServices = applyAuditStreamServices;
			this.companyManagerServices = companyManagerServices;
			this.vacationCheckServices = vacationCheckServices;
		}

		public Apply GetById(Guid id) => _context.AppliesDb.Where(a => a.Id == id).FirstOrDefault();

		public IEnumerable<Apply> GetAll(int page, int pageSize)
		{
			return GetAll((item) => true, page, pageSize);
		}

		public IEnumerable<Apply> GetAll(string userid, int page, int pageSize)
		{
			return GetAll((item) => item.BaseInfo.From.Id == userid, page, pageSize);
		}

		public IEnumerable<Apply> GetAll(string userid, AuditStatus status, int page, int pageSize)
		{
			return GetAll((item) => item.BaseInfo.From.Id == userid && status == item.Status, page, pageSize);
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

		public async Task Delete(Apply item)
		{
			await RemoveApplies(new List<Apply>() { item }).ConfigureAwait(true);
		}

		public IEnumerable<Apply> Find(Func<Apply, bool> predict)
		{
			var list = _context.AppliesDb.Where(predict);
			return list;
		}
	}
}