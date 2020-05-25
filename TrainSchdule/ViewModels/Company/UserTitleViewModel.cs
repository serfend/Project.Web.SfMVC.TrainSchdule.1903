using BLL.Helpers;
using System.Collections.Generic;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Company
{
	/// <summary>
	/// 职务等级列表
	/// </summary>
	public class UserTitlesViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public EntitiesListDataModel<UserTitleDataModel> Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class UserTitleViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public UserTitleDataModel Data { get; set; }
	}

	/// <summary>
	/// 职务等级
	/// </summary>
	public class UserTitleDataModel
	{
		/// <summary>
		/// 职务等级代码
		/// </summary>
		public int Code { get; set; }

		/// <summary>
		/// 等级
		/// </summary>
		public int Level { get; set; }

		/// <summary>
		/// 名称
		/// </summary>
		public string Name { get; set; }
	}
}