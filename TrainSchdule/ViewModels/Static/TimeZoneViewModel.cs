using BLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.Static
{
	/// <summary>
	/// 时间设置
	/// </summary>
	public class TimeZoneViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public TimeZoneDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class TimeZoneDataModel
	{
		/// <summary>
		/// 左侧信息
		/// </summary>
		public ValueNameDataModel<DateTime> Left { get; set; }

		/// <summary>
		/// 右侧信息
		/// </summary>
		public ValueNameDataModel<DateTime> Right { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class ValueNameDataModel<T>
	{
		/// <summary>
		///
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///
		/// </summary>
		public T Value { get; set; }
	}
}