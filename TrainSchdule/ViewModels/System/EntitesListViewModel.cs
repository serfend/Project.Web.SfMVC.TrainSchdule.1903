using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.System
{
	/// <summary>
	/// 任意实体列表
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class EntitiesListDataModel<T>
	{
		/// <summary>
		/// 列表
		/// </summary>
		public IEnumerable<T> List { get; set; }

		/// <summary>
		/// 数据库总量，配合SplitPage查询
		/// </summary>
		public int TotalCount { get; set; }
	}
}