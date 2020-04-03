using BLL.Helpers;
using DAL.Entities;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Company
{
	/// <summary>
	/// 职务列表
	/// </summary>
	public class DutiesViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public DutiesDataModel Data { get; set; }
	}

	/// <summary>
	/// 返回职务列表
	/// </summary>
	public class DutiesDataModel
	{
		/// <summary>
		/// 当前查询的分页中的数据
		/// </summary>
		public IEnumerable<DutyDataModel> List { get; set; }

		/// <summary>
		/// 当前查询的总数
		/// </summary>
		public int TotalCount { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class DutyViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public DutyDataModel Data { get; set; }
	}

	/// <summary>
	/// 职务
	/// </summary>
	public class DutyDataModel
	{
		/// <summary>
		/// 职务代码
		/// </summary>
		public int Code { get; set; }

		/// <summary>
		/// 职务的名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 是否是主官
		/// </summary>
		public bool IsMajorManager { get; set; }

		/// <summary>
		/// 职务标签
		/// </summary>
		public IEnumerable<string> Tags { get; set; }
	}
}