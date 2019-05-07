using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL.DTO.Apply;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;

namespace BLL.Interfaces
{
	public interface IApplyServiceCreate
	{
		Task<ApplyBaseInfo> SubmitBaseInfoAsync(ApplyBaseInfoVDTO model);
		ApplyBaseInfo SubmitBaseInfo(ApplyBaseInfoVDTO model);
		Task<ApplyRequest> SubmitRequestAsync(ApplyRequestVDTO model);
		ApplyRequest SubmitRequest(ApplyRequestVDTO model);
		Apply Submit(ApplyVDTO model);
		/// <summary>
		/// 依据用户所在单位获取审批流，后期将修改为自定义审批方式
		/// </summary>
		/// <param name="company"></param>
		/// <returns></returns>
		IEnumerable<ApplyResponse> GetAuditStream(Company company);
	}
}
