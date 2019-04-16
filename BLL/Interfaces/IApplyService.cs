using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;
using DAL.Entities;
using TrainSchdule.BLL.DTO;
using TrainSchdule.DAL.Entities;

namespace BLL.Interfaces
{
	public interface IApplyService : IDisposable
	{
		ApplyDTO Get(Guid id);
		/// <summary>
		/// 加载所有申请
		/// </summary>
		IEnumerable<ApplyDTO> GetAll(int page, int pageSize);

		/// <summary>
		/// 通过userName加载申请
		/// </summary>
		IEnumerable<ApplyDTO> GetAll(string userName, int page, int pageSize);

		/// <summary>
		/// 通过userName和status加载申请
		/// </summary>
		IEnumerable<ApplyDTO> GetAll(string userName,AuditStatus status, int page, int pageSize);
		/// <summary>
		/// 任意条件获取申请
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="page"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		IEnumerable<ApplyDTO> GetAll(Func<Apply, bool> predicate, int page, int pageSize);

		/// <summary>
		/// 创建申请.
		/// </summary>
		Apply Create(Apply item);

		/// <summary>
		/// 异步创建申请.
		/// </summary>
		Task<Apply> CreateAsync(Apply item);

		/// <summary>
		/// Edits user.
		/// </summary>
		bool Edit(string userName, Action<Apply> editCallBack);

		/// <summary>
		/// Async edits user.
		/// </summary>
		Task<bool> EditAsync(string userName, Action<Apply> editCallBack);
	}
}
