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
	/// 审批节点创建
	/// </summary>
	public class StreamNodeCreateDataModel : GoogleAuthViewModel
	{
		/// <summary>
		/// 当为编辑时需要
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// 节点选择器
		/// </summary>
		public MembersFilterDto Filter { get; set; }

		/// <summary>
		/// 节点名称
		/// </summary>
		[Required(ErrorMessage = "节点名称不能为空")]
		[MinLength(1, ErrorMessage = "节点名称不能为空白")]
		public string Name { get; set; }

		/// <summary>
		/// 单位作用域
		/// </summary>
		[Required(ErrorMessage = "节点作用域不能为空")]
		[MinLength(1, ErrorMessage = "节点作用域不能为空白")]
		public string CompanyRegion { get; set; }

		/// <summary>
		/// 节点描述
		/// </summary>
		public string Description { get; set; }
	}
}