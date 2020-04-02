using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Apply.ApplyAuditStream
{
	public class StreamSolutionCreateDataModel : GoogleAuthViewModel
	{
		/// <summary>
		/// 解决方案名称
		/// </summary>
		[Required(ErrorMessage = "方案名称不能为空")]
		[MinLength(1, ErrorMessage = "方案名称不能为空白")]
		public string Name { get; set; }

		/// <summary>
		/// 解决方案描述
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// 解决方案包含的所有节点名称
		/// </summary>
		public IEnumerable<string> Nodes { get; set; }
	}
}