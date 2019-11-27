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
		IEnumerable<Apply> QueryApplies(QueryApplyDataModel model);
		void RemoveAllUnSaveApply();
		byte[] ExportExcel(string templete,  ApplyDetailDto model);
		byte[] ExportExcel(string templete,  IEnumerable<ApplyDetailDto> model, CompanyDto currentCompany);
	}
}
