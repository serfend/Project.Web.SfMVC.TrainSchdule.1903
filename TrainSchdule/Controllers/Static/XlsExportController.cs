using BLL.Extensions.ApplyExtensions;
using BLL.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.Static;

namespace TrainSchdule.Controllers
{
	public partial class StaticController
	{
		/// <summary>
		/// 依据模板导出申请列表到Xls
		/// </summary>
		/// <param name="form"></param>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(typeof(string), 0)]
		[Route("exportApplies")]
		public IActionResult ExportApplies(AppliesExportDataModel form)
		{
			string filePath = null;
			try
			{
				filePath = GetFilePath(form.Templete);
			}
			catch (ActionStatusMessageException ex)
			{
				return new JsonResult(ex.Status);
			}
			var list = _applyService.QueryApplies(form.Query, true, out var totalCount);
			var fileContent = _applyService.ExportExcel(filePath, list.Select(a => a.ToDetaiDto(_usersService.VocationInfo(a.BaseInfo.From), false)));
			if (fileContent == null) return new JsonResult(ActionStatusMessage.Static.XlsNoData);
			return ExportXls(fileContent, $"{list.Count()}/{totalCount}条({form.Templete}");
		}
		/// <summary>
		/// 依据模板导出单个申请到Xls
		/// </summary>
		/// <param name="form"></param>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(typeof(string), 0)]
		[Route("exportApply")]
		public IActionResult ExportApply(AppliesExportDataModel form)
		{
			string filePath = null;
			try
			{
				filePath = GetFilePath(form.Templete);
			}
			catch (ActionStatusMessageException ex)
			{
				return new JsonResult(ex.Status);
			}
			var singleApply = _applyService.QueryApplies(form.Query, true, out var totalCount).FirstOrDefault();
			var fileContent = _applyService.ExportExcel(filePath, singleApply.ToDetaiDto(_usersService.VocationInfo(singleApply.BaseInfo.From), false));
			if (fileContent == null) return new JsonResult(ActionStatusMessage.Static.XlsNoData);
			return ExportXls(fileContent, $"{singleApply.BaseInfo.RealName}的申请({form.Templete})");
		}
		private string GetFilePath(string templete)
		{
			var sWebRootFolder = _hostingEnvironment.WebRootPath;
			templete = $"Templete\\{templete}";
			var tempFile = new FileInfo(Path.Combine(sWebRootFolder, templete));
			if (!tempFile.Exists) throw new ActionStatusMessageException(ActionStatusMessage.Static.TempXlsNotExist);
			return tempFile.FullName;
		}
		private IActionResult ExportXls(byte[] fileContent, string description)
		{
			var datetime = DateTime.Now.ToString("yyyy年mm月dd日");
			var fileName = $"{datetime}导出{description}.xlsx";
			return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
		}
	}
}
