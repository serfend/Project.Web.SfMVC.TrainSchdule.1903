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
		GradePhyRecord ModifyRecord(GradePhyRecord model);

		/// <summary>
		/// 查询成绩记录
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Tuple<IEnumerable<GradePhyRecord>, int> GetRecords(QueryGradeRecordViewModel model);
	}
}