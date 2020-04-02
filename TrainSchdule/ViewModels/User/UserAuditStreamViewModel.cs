using System.Collections.Generic;
using BLL.Helpers;
using DAL.DTO.Apply.ApplyAuditStreamDTO;
using DAL.DTO.Company;
using DAL.Entities.ApplyInfo;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.User
{
	/// <summary>
	///
	/// </summary>
	public class UserAuditStreamViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public UserAuditStreamDataModel Data { get; set; }
	}

	/// <summary>
	/// 审批流全流程
	/// </summary>
	public class UserAuditStreamDataModel
	{
		/// <summary>
		/// 所有步骤
		/// </summary>
		public IEnumerable<ApplyAuditStepDto> Steps { get; set; }
		/// <summary>
		/// 命中 的审批流方案名称
		/// </summary>
		public string SolutionName { get; set; }
	}
}