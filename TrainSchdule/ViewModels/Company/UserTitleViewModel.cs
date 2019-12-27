using BLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
		public UserTitlesDataModel Data { get; set; }
	}
	/// <summary>
	/// 返回职务等级列表
	/// </summary>
	public class UserTitlesDataModel
	{
		/// <summary>
		/// 当前查询的分页中的数据
		/// </summary>
		public IEnumerable<UserTitleDataModel> List { get; set; }
		/// <summary>
		/// 当前查询的总数
		/// </summary>
		public int TotalCount { get; set; }
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
