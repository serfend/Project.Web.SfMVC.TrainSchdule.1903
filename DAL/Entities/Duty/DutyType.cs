using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Entities.Duty
{
	public class DutyType
	{
		[Key]
		public int Code{ get; set; }
		/// <summary>
		/// 此职务类型指向的职务
		/// </summary>
		public virtual Duties Duties { get; set; }
		/// <summary>
		/// 职务类型名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 需要审批的层级数量，当为0时则按默认 zs=2，gb=3
		/// </summary>
		public int AuditLevelNum { get; set; }
	}
	public enum DutiesRawType
	{
		none=0,
		zs=1,
		gb=2
	}
}
