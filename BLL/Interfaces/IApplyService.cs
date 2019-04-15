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
		/// <summary>
		/// 加载所有申请
		/// </summary>
		IEnumerable<ApplyDTO> GetAll(int page, int pageSize);

		/// <summary>
		/// 通过userName加载申请
		/// </summary>
		IEnumerable<ApplyDTO> GetAll(string userName);

		/// <summary>
		/// 通过userName和status加载申请
		/// </summary>
		IEnumerable<ApplyDTO> GetAll(string userName,AuditStatus status);



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
