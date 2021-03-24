using BLL.Helpers;
using DAL.DTO.Apply;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.Audit
{
	public interface IAuditStreamServices
    {

		/// <summary>
		/// 初始化申请
		/// </summary>
		/// <param name="model"></param>
		/// <param name="user"></param>
		void InitAuditStream<T>(ref T model,User user)where T:IAuditable;

		/// <summary>
		/// 修改审批状态
		/// </summary>
		/// <param name="model"></param>
		/// <param name="status"></param>
		/// <param name="authUser"></param>
		void ModifyAuditStatus<T>(ref T model, AuditStatus status, string authUser = null)where T: IAuditable;
		/// <summary>
		/// 审批
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		IEnumerable<ApiResult> Audit<T>(ref ApplyAuditVdto<T> model)where T:IAuditable;
	}
}
