using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Helpers;
using DAL.DTO.Apply;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;

namespace BLL.Interfaces
{
	public interface IApplyServiceCreate
	{
		Task<ApplyBaseInfo> SubmitBaseInfoAsync(ApplyBaseInfoVdto model);

		Task<ApplyRequest> SubmitRequestAsync(User targetUser, ApplyRequestVdto model);

		Task<ApplyRequestVdto> CaculateVacation(ApplyRequestVdto model);

		Apply Submit(ApplyVdto model);

		/// <summary>
		/// 初始化申请
		/// </summary>
		/// <param name="model"></param>
		void InitAuditStream(Apply model);

		/// <summary>
		/// 修改休假状态
		/// </summary>
		/// <param name="model"></param>
		/// <param name="status"></param>
		/// <param name="authUser"></param>
		void ModifyAuditStatus(Apply model, AuditStatus status, string authUser = null);

		IEnumerable<ApiResult> Audit(ApplyAuditVdto models);
	}
}