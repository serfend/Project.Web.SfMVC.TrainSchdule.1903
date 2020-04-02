using DAL.Entities.ApplyInfo;
using System;

namespace DAL.DTO.Apply
{
	public class ApplyResponseDto
	{
		/// <summary>
		/// 当前在全流程中的序号
		/// </summary>
		public int Index { get; set; }

		public string AuditingUserRealName { get; set; }
		public Auditing Status { get; set; }
		public DateTime? HandleStamp { get; set; }
		public string Remark { get; set; }
	}
}