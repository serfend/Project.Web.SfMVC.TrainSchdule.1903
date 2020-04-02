using DAL.Entities.ApplyInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.Apply.ApplyAuditStream
{
	/// <summary>
	/// 创建方案规则
	/// </summary>
	public class StreamSolutionRuleCreateDataModel
	{
		/// <summary>
		/// 方面规则名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 方案规则描述
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// 满足条件后使用何解决方案
		/// </summary>
		public string SolutionName { get; set; }

		/// <summary>
		/// 方案的优先级，值越大优先级越高
		/// </summary>
		public int Priority { get; set; }

		/// <summary>
		/// 方案是否启用
		/// </summary>
		public bool Enable { get; set; }

		/// <summary>
		/// 方案规则候选人
		/// </summary>
		public MembersFilter Filter { get; set; }
	}
}