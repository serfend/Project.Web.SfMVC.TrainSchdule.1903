using System;
using System.Collections.Generic;
using System.Text;
using DAL.DTO.Apply;

namespace BLL.Interfaces
{
	public interface IApplyServiceManage
	{
		void RemoveAllUnSaveApply();
		byte[] ExportExcel(string templete, string outPath, ApplyDetailDto model);
		byte[] ExportExcel(string templete, string outPath, IEnumerable<ApplyDetailDto> model);
	}
}
