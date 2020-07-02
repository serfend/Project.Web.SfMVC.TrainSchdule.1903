using BLL.Helpers;
using DAL.Entities.UserInfo;
using DAL.QueryModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrainSchdule.ViewModels.ZX
{
	/// <summary>
	/// 成绩查询结果
	/// </summary>
	public class PhySingleGradeViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public PhySingleGradeDataModel Data { get; set; }
	}

	/// <summary>
	/// 多成绩查询结果
	/// </summary>
	public class PhyGradesViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public PhyGradeDataModel Data { get; set; }
	}

	/// <summary>
	/// 一次查询多个成绩
	/// </summary>
	public class PhyGradeDataModel
	{
		/// <summary>
		/// 成绩查询实体
		/// </summary>
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
		/// 是否需要计算成绩，默认为否
		/// </summary>
		public bool NeedCaculateGrade { get; set; } = false;

		/// <summary>
		/// 用户信息
		/// </summary>
		[Required]
		public PhyGradeUserDataModel User { get; set; }

		/// <summary>
		///
		/// </summary>
		public QueryByPage Pages { get; set; }
	}

	/// <summary>
	/// 成绩查询
	/// </summary>
	public class PhyGradeQueryDataModel
	{
		/// <summary>
		/// 查询的科目名【可选】
		/// </summary>
		public string Subject { get; set; }

		/// <summary>
		/// 查询的科目分组【可选】
		/// </summary>
		public string Group { get; set; }

		/// <summary>
		/// 科目名称
		/// </summary>
		public string Name { get; set; }

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