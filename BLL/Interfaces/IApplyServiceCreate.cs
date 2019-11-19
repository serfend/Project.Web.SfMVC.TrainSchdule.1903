using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Helpers;
using DAL.DTO.Apply;
using DAL.Entities;
using DAL.Entities.ApplyInfo;

namespace BLL.Interfaces
{
	public interface IApplyServiceCreate
	{
		Task<ApplyBaseInfo> SubmitBaseInfoAsync(ApplyBaseInfoVdto model);
		ApplyBaseInfo SubmitBaseInfo(ApplyBaseInfoVdto model);
		Task<ApplyRequest> SubmitRequestAsync(ApplyRequestVdto model);
		ApplyRequest SubmitRequest(ApplyRequestVdto model);
		Apply Submit(ApplyVdto model);
		IEnumerable<Apply> GetApplyByToAuditCompany(string code,int page,int pageSize);
		IEnumerable<Apply> GetApplyBySubmitCompany(string code,int page,int pageSize);

		IEnumerable<Apply> GetApplyBySubmitUser(string id,int page,int pageSize);

		/// <summary>
		/// 依据用户所在单位获取审批流，后期将修改为自定义审批方式
		/// </summary>
		/// <param name="company"></param>
		/// <returns></returns>
		IEnumerable<ApplyResponse> GetAuditStream(Company company);

		void ModifyAuditStatus(Apply model, AuditStatus status);
		IEnumerable<Status> Audit(ApplyAuditVdto models);
	}
}
