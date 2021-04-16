using DAL.DTO.Apply;
using DAL.Entities.ApplyInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Apply.ApplyAuditStream
{
	/// <summary>
	/// 创建方案规则
	/// </summary>
	public class StreamSolutionRuleCreateDataModel : GoogleAuthViewModel
	{
		/// <summary>
		/// 当为编辑时需要
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// 方面规则名称
		/// </summary>
		[Required(ErrorMessage = "方案规则名称不能为空")]
		[MinLength(1, ErrorMessage = "方案规则名称不能为空白")]
		public string Name { get; set; }

		/// <summary>
		/// 单位作用域
		/// </summary>
		[Required(ErrorMessage = "节点作用域不能为空")]
		[MinLength(1, ErrorMessage = "节点作用域不能为空白")]
		public string CompanyRegion { get; set; }

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
		public MembersFilterDto Filter { get; set; }
	}
}