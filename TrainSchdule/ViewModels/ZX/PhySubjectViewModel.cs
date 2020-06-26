using BLL.Helpers;
using DAL.Entities.ZX.Phy;
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
	///
	/// </summary>
	public class PhySubjectDataModel : GoogleAuthViewModel
	{
		/// <summary>
		///
		/// </summary>
		public GradePhySubject Subject { get; set; }
	}
}