using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.Data;
using DAL.Entities.ApplyInfo;

namespace BLL.Services.ApplyServices
{
	public partial class ApplyService:IApplyService
	{
		#region Fileds

		private readonly ApplicationDbContext _context;
		#endregion


		public ApplyService(ApplicationDbContext context)
		{
			_context = context;
		}
		


		public Apply Get(Guid id) => _context.Applies.Find(id);
		public IEnumerable<Apply> GetAll(int page, int pageSize)
		{
			return GetAll((item) => true,page,pageSize);
		}

		public IEnumerable<Apply> GetAll(string userid,int page,int pageSize)
		{
			return GetAll((item) => item.BaseInfo.From.Id == userid, page,pageSize);
		}

		public IEnumerable<Apply> GetAll(string userid, AuditStatus status,int page,int pageSize)
		{
			return GetAll((item) => item.BaseInfo.From.Id == userid && status == item.Status,page,pageSize);
		}

		public IEnumerable<Apply> GetAll(Expression<Func<Apply, bool> > predicate, int page, int pageSize)
		{
			var items = _context.Applies.Where(predicate).OrderByDescending(a=>a.Status).ThenByDescending(a=>a.Create).Skip(page * pageSize).Take(pageSize);
			return items;
		}
		public Apply Create(Apply item)
		{
			_context.Applies.Add(item);
			_context.SaveChanges();
			return item;
		}

		public async Task<Apply> CreateAsync(Apply item)
		{
			await _context.Applies.AddAsync(item);
			await _context.SaveChangesAsync();
			return item;
		}

		public bool Edit(string id, Action<Apply> editCallBack)
		{
			var target = _context.Applies.Find(id);
			if (target == null) return false;
			editCallBack.Invoke(target);
			_context.Applies.Update(target);
			_context.SaveChanges();
			return true;
		}

		public async Task<bool> EditAsync(string id, Action<Apply> editCallBack)
		{
			var target = _context.Applies.Find(id);
			if (target == null) return false;
			await Task.Run(() => editCallBack.Invoke(target));
			_context.Applies.Update(target);
			await _context.SaveChangesAsync();
			return true;
		}

		public void Delete(Apply item)
		{
			foreach (var applyResponse in item.Response)
			{
				_context.ApplyResponses.Remove(applyResponse);
			}
			_context.Applies.Remove(item);
			_context.SaveChanges();
		}

		public IEnumerable<Apply> Find(Func<Apply, bool> predict)
		{
			var list = _context.Applies.Where(predict);
			return list;
		}
	}
}
