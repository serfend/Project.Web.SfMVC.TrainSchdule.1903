using DAL.Entities.UserInfo;
using DAL.Entities.ZX.Phy;
using DAL.QueryModel;
using DAL.QueryModel.ZX;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces.ZX
{
	public partial interface IPhyGradeServices
	{
		/// <summary>
		/// 操作成绩记录
		/// </summary>
		/// <returns></returns>
		GradePhyRecord Modify(GradePhyRecord model);

		Tuple<IEnumerable<GradePhyRecord>, int> GetRecords(QueryGradeRecordViewModel model);
	}
}