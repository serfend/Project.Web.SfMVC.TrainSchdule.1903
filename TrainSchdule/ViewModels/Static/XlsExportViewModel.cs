using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.Static
{
	/// <summary>
	/// 
	/// </summary>
	public class XlsExportViewModel
	{
		/// <summary>
		/// 使用的模板文件名称
		/// </summary>
		public string Templete { get; set; }

		/// <summary>
		/// 获取单个用户的
		/// </summary>
		public string User { get; set; }
		/// <summary>
		/// 获取单位下所有申请
		/// </summary>
		public string Company { get; set; }
		/// <summary>
		/// 获取某个申请的信息
		/// </summary>
		public string Apply { get; set; }

	}

}
