using System;
using DAL.DTO.Apply;
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
}
