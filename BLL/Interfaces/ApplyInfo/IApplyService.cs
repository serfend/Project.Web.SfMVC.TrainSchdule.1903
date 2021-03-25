using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.DTO.Apply;
using DAL.DTO.Company;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;

namespace BLL.Interfaces
{
	public interface IApplyService<T,Q> :  IApplyServiceManage<T,Q> where T:IAppliable where Q : IApplyRequestBase
	{
		T GetById(Guid id);

		/// <summary>
		/// 加载所有申请
		/// </summary>
		IEnumerable<T> GetAll(int page, int pageSize);

		/// <summary>
		/// 通过userName加载申请
		/// </summary>
		IEnumerable<T> GetAll(string userid, int page, int pageSize);

		/// <summary>
		/// 通过userName和status加载申请
		/// </summary>
		IEnumerable<T> GetAll(string userid, AuditStatus status, int page, int pageSize);

		/// <summary>
		/// 任意条件获取申请
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="page"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate, int page, int pageSize);

		/// <summary>
		/// 创建申请.
		/// </summary>
		T Create(T item);

		bool Edit(string id, Action<T> editCallBack);

		Task<bool> EditAsync(string id, Action<T> editCallBack);

		Task Delete(T item);

		IEnumerable<T> Find(Func<T, bool> predict);
		T Submit(ApplyVdto model);
		IQueryable<T> CheckIfHaveSameRangeVacation(T apply);
	}
}