using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.Apply.ApplyAuditStream
{
	public class StreamSolutionCreateDataModel
	{
		/// <summary>
		/// 解决方案名称
		/// </summary>
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