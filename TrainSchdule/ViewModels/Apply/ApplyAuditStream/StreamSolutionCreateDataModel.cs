using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Apply.ApplyAuditStream
{
	/// <summary>
	///
	/// </summary>
	public class StreamSolutionCreateDataModel : GoogleAuthViewModel
	{
		/// <summary>
		/// 当为编辑时需要
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// 解决方案名称
		/// </summary>
		[Required(ErrorMessage = "方案名称不能为空")]
		[MinLength(1, ErrorMessage = "方案名称不能为空白")]
		public string Name { get; set; }

		/// <summary>
		/// 单位作用域
		/// </summary>
		[Required(ErrorMessage = "节点作用域不能为空")]
		[MinLength(1, ErrorMessage = "节点作用域不能为空白")]
		public string CompanyRegion { get; set; }

		/// <summary>
		/// 解决方案描述
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// 解决方案包含的所有节点名称
		/// </summary>
		public IEnumerable<string> Nodes { get; set; }

		/// <summary>
		/// 作用节点
		/// </summary>
		[Required(ErrorMessage = "作用类型未填写")]
		[MinLength(1, ErrorMessage = "作用类型未填写")]
		public string EntityType { get; set; }
	}
}