using System;
using System.Collections.Generic;
using System.Text;
using DAL.DTO.Apply;
using DAL.DTO.Company;
using DAL.Entities.ApplyInfo;
using DAL.QueryModel;

namespace BLL.Interfaces
{
	public interface IApplyServiceManage
	{
		/// <summary>
		/// 按筛选查询
		/// </summary>
		/// <param name="model"></param>
		/// <param name="getAllAppliesPermission">是否允许获取所有申请</param>
		/// <param name="totalCount"></param>
		/// <returns></returns>
		IEnumerable<Apply> QueryApplies(QueryApplyDataModel model,bool getAllAppliesPermission, out int totalCount);
		void RemoveAllUnSaveApply();
		byte[] ExportExcel(string templete,  ApplyDetailDto model);
		byte[] ExportExcel(string templete,  IEnumerable<ApplyDetailDto> model, CompanyDto currentCompany);
	}
}
