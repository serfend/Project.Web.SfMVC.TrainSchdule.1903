using BLL.Helpers;
using DAL.Entities.ZX.Phy;
using System.Collections;
using System.Collections.Generic;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.ZX
{
	/// <summary>
	/// 成绩查询
	/// </summary>
	public class PhySubjectViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public GradePhySubject Data { get; set; }
	}

	/// <summary>
	/// 批量更新
	/// </summary>
	public class PhySubjectsDataModel : GoogleAuthViewModel
	{
		/// <summary>
		/// 多条实体
		/// </summary>
		public IEnumerable<GradePhySubject> Subjects { get; set; }
	}

	/// <summary>
	/// 更新单个
	/// </summary>
	public class PhySubjectDataModel : GoogleAuthViewModel
	{
		/// <summary>
		///
		/// </summary>
		public GradePhySubject Subject { get; set; }
	}
}