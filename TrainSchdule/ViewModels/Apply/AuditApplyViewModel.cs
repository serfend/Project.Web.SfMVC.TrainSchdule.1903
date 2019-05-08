using System;
using DAL.DTO.Apply;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Apply
{
	public class AuditApplyViewModel
	{
		public AuditApplyDataModel Data { get; set; }
		public GoogleAuthViewModel Auth { get; set; }
	}

	public class AuditApplyDataModel
	{
		public Guid Id { get; set; }
		public AuditResult Action { get; set; }
		public string Remark { get; set; }
	}
}
