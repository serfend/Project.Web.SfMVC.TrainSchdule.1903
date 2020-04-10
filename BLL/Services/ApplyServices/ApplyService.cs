using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

		#endregion Fileds

		public ApplyService(ApplicationDbContext context, IUsersService usersService, ICurrentUserService currentUserService, IApplyAuditStreamServices applyAuditStreamServices)
		{
			_context = context;
			_usersService = usersService;
			_currentUserService = currentUserService;
			new Configurator()[".xlsx"] = new WorkbookLoader();
			_applyAuditStreamServices = applyAuditStreamServices;
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
			_context.Applies.Add(item);
			if (item.BaseInfo.From.Application.ApplicationSetting?.LastSubmitApplyTime != null && item.BaseInfo.From.Application.ApplicationSetting.LastSubmitApplyTime.Value.AddMinutes(1) >
				DateTime.Now) return null;
			if (item.BaseInfo.From.Application.ApplicationSetting != null)
				item.BaseInfo.From.Application.ApplicationSetting.LastSubmitApplyTime = DateTime.Now;

			_context.AppUsers.Update(item.BaseInfo.From);
			_context.SaveChanges();
			return item;
		}

		public async Task<Apply> CreateAsync(Apply item)
		{
			await _context.Applies.AddAsync(item).ConfigureAwait(true);
			await _context.SaveChangesAsync().ConfigureAwait(true);
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