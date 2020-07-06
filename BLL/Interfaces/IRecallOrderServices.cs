using DAL.DTO.Recall;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces
{
	public interface IRecallOrderServices
	{
		RecallOrder Create(RecallOrderVDto recallOrder);

		/// <summary>
		/// 编辑 落实情况
		/// </summary>
		/// <param name="apply"></param>
		/// <param name="status"></param>
		/// <returns></returns>
		ApplyExecuteStatus Create(Apply apply, ExecuteStatusVDto status);
	}
}