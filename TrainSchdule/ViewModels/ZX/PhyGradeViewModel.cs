using BLL.Helpers;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.ZX
{
	public class PhySingleGradeViewModel: ApiDataModel
	{
		public PhySingleGradeDataModel Data { get; set; }
		public  PhySingleGradeViewModel(ApiResult message) : base(message){ }
	}
	public class PhyGradesViewModel : ApiDataModel
	{
		public PhyGradeDataModel Data { get; set; }
	}
	/// <summary>
	/// 一次查询多个成绩
	/// </summary>
	public class PhyGradeDataModel
	{
		[Required]
		public IEnumerable<PhySingleGradeDataModel> Queries { get; set; }
	}
	/// <summary>
	/// 查询单个成绩
	/// </summary>
	public class PhySingleGradeDataModel
	{
		/// <summary>
		/// 科目信息
		/// </summary>
		[Required]
		public IEnumerable<PhyGradeQueryDataModel> Subjects { get; set; }
		/// <summary>
		/// 用户信息
		/// </summary>
		[Required]
		public PhyGradeUserDataModel User { get; set; }
	}
	/// <summary>
	/// 成绩查询
	/// </summary>
	public class PhyGradeQueryDataModel
	{
		/// <summary>
		/// 查询的科目名
		/// </summary>
		[Required]
		public string Subject { get; set; }
		/// <summary>
		/// 成绩原始
		/// </summary>
		public string RawValue { get; set; }
		/// <summary>
		/// 由系统计算输出的成绩
		/// </summary>
		
  public int Grade { get; set; }
		/// <summary>
		/// 由系统计算出的合格所需成绩
		/// </summary>
		public string Standard { get; set; }
	
 }
	/// <summary>
	/// 用户信息
	/// </summary>
	public class PhyGradeUserDataModel
	{
		/// <summary>
		/// 可以是手动传入一个User
		/// </summary>
		public UserBaseInfo User { get; set; }
		/// <summary>
		/// 也可以是传入用户ID
		/// </summary>
		public string UserName { get; set; }
	}
}
