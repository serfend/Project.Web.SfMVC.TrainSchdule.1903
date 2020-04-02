using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.DTO.Apply;
using DAL.DTO.Company;
using DAL.Entities.ApplyInfo;

namespace BLL.Interfaces
{
	public interface IApplyService :IApplyServiceCreate, IApplyServiceManage
	{
		Apply GetById(Guid id);

		/// <summary>
		/// 加载所有申请
		/// </summary>
		IEnumerable<Apply> GetAll(int page, int pageSize);

		/// <summary>
		/// 通过userName加载申请
		/// </summary>
		IEnumerable<Apply> GetAll(string userid, int page, int pageSize);

		/// <summary>
		/// 通过userName和status加载申请
		/// </summary>
		IEnumerable<Apply> GetAll(string userid, AuditStatus status, int page, int pageSize);
		/// <summary>
		/// 任意条件获取申请
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="page"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		IEnumerable<Apply> GetAll(Expression<Func<Apply, bool>> predicate, int page, int pageSize);


		/// <summary>
		/// 创建申请.
		/// </summary>
		Apply Create(Apply item);

		/// <summary>
		/// 异步创建申请.
		/// </summary>
		Task<Apply> CreateAsync(Apply item);


		bool Edit(string id, Action<Apply> editCallBack);

		Task<bool> EditAsync(string id, Action<Apply> editCallBack);

		void Delete(Apply item);

		IEnumerable<Apply> Find(Func<Apply, bool> predict);
	}
}
