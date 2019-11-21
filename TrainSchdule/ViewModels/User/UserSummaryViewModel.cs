using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.User
{
	/// <summary>
	/// 
	/// </summary>
	public class UserSummaryViewModel: ApiDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		public UserSummaryDataModel Data { get; set; }
	}
	/// <summary>
	/// 用户简要信息
	/// </summary>
	public class UserSummaryDataModel
	{
		/// <summary>
		/// 用户账号
		/// </summary>
		public string Id { get; set; }
		/// <summary>
		/// 真实姓名
		/// </summary>
		public string RealName { get; set; }
		/// <summary>
		/// 简介
		/// </summary>
		public string About { get; set; }

		/// <summary>
		/// 性别
		/// </summary>
		public GenderEnum Gender { get; set; }
		/// <summary>
		/// 头像文件地址
		/// </summary>
		public string Avatar { get; set; }
		/// <summary>
		/// 单位代码
		/// </summary>
		public string CompanyCode { get; set; }
		/// <summary>
		/// 职务代码
		/// </summary>
		public int? DutiesCode { get; set; }
		/// <summary>
		/// 单位名称
		/// </summary>
		public string CompanyName { get; set; }
		/// <summary>
		/// 职务名
		/// </summary>
		public string DutiesName { get; set; }
	}
}
