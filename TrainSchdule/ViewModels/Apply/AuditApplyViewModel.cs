using System;
using System.Collections.Generic;
using BLL.Helpers;
using DAL.DTO.Apply;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Apply
{
	/// <summary>
	/// 
	/// </summary>
	public class AuditApplyViewModel
	{
		/// <summary>
		/// 
		/// </summary>
		public AuditApplyDataModel Data { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public GoogleAuthViewModel Auth { get; set; }
	}
	
	/// <summary>
	/// 
	/// </summary>
	public class AuditApplyDataModel
	{/// <summary>
	/// 
	/// </summary>
		public IEnumerable<AuditApplyNodeDataModel> List { get; set; } 
	}
	/// <summary>
	/// 
	/// </summary>

	public class AuditApplyNodeDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		public Guid Id { get; set; }
		/// <summary>
		/// <see cref="AuditResult"/> : 0:无操作 1:同意  2:驳回
		/// </summary>
		public AuditResult Action { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Remark { get; set; }
	}

	/// <summary>
	/// 返回审核结果
	/// </summary>
	public class ApplyAuditResponseStatusViewModel:ApiDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<ApplyAuditResponseStatusDataModel> Data { get; set; }
	}
	/// <summary>
	/// 
	/// </summary>
	public class ApplyAuditResponseStatusDataModel:Status
	{
		/// <summary>
		/// 
		/// </summary>
		public Guid Id { get; set; }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="status"></param>
		public ApplyAuditResponseStatusDataModel(Guid id, Status status) : base(status.status, status.message)
		{
			this.Id = id;
		}
	}
}
