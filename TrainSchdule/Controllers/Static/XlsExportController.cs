using BLL.Extensions.ApplyExtensions;
using BLL.Helpers;
using Hangfire;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.Static;

namespace TrainSchdule.Controllers
{
	public partial class StaticController
	{
		private readonly string XlsExportPath = "XlsExportPath";

		/// <summary>
		/// 依据模板导出申请列表到Xls
		/// </summary>
		/// <param name="form"></param>
		/// <returns></returns>
		[HttpPost]
		[ProducesResponseType(typeof(string), 0)]
		public async Task<IActionResult> ExportApplies([FromBody] AppliesExportDataModel form)
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
			var list = _context.AppliesDb.Where(a => form.Query.Arrays.Contains(a.Id)).ToList();
			var fileContent = _applyService.ExportExcel(filePath, list.Select(a => a.ToDetaiDto(_usersService.VocationInfo(a.BaseInfo.From))));
			if (fileContent == null) return new JsonResult(ActionStatusMessage.Static.XlsNoData);
			return await ExportXls(fileContent, $"{list.Count()}条({form.Templete})");
		}

		/// <summary>
		/// 依据模板导出单个申请到Xls
		/// </summary>
		/// <param name="form"></param>
		/// <returns></returns>
		[HttpPost]
		[ProducesResponseType(typeof(string), 0)]
		public async Task<IActionResult> ExportApply([FromBody]AppliesExportDataModel form)
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
			var singleApply = _context.AppliesDb.Where(a => a.Id == form.Query.Value).FirstOrDefault();
			if (singleApply == null) return new JsonResult(ActionStatusMessage.ApplyMessage.NotExist);
			var fileContent = _applyService.ExportExcel(filePath, singleApply.ToDetaiDto(_usersService.VocationInfo(singleApply.BaseInfo?.From)));
			if (fileContent == null) return new JsonResult(ActionStatusMessage.Static.XlsNoData);
			return await ExportXls(fileContent, $"{singleApply.BaseInfo.RealName}的申请({form.Templete})");
		}

		private string GetFilePath(string templete)
		{
			var sWebRootFolder = _hostingEnvironment.WebRootPath;
			templete = $"Templete\\{templete}";
			var tempFile = new FileInfo(Path.Combine(sWebRootFolder, templete));
			if (!tempFile.Exists) throw new ActionStatusMessageException(ActionStatusMessage.Static.TempXlsNotExist);
			return tempFile.FullName;
		}

		/// <summary>
		/// 删除临时文件
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete]
		public IActionResult RemoveTempFile(string id)
		{
			var file = _fileServices.Load(XlsExportPath, id);
			if (file == null) return new JsonResult(ActionStatusMessage.Static.FileNotExist);
			if (_fileServices.Remove(file))
				return new JsonResult(ActionStatusMessage.Success);
			else
				return new JsonResult(ActionStatusMessage.Fail);
		}

		private async Task<IActionResult> ExportXls(byte[] fileContent, string description)
		{
			var datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
			var fileName = $"TS{datetime}-{description}.xlsx";
			using (var sr = new MemoryStream(fileContent))
			{
				var file = new FormFile(sr, 0, fileContent.Length, fileName, fileName);

				var tmpFile = await _fileServices.Upload(file, XlsExportPath, fileName, Guid.Empty, Guid.Empty).ConfigureAwait(true);

				var removeTime = TimeSpan.FromDays(7);
				var jobId = BackgroundJob.Schedule(() => RemoveTempFile(tmpFile.Id.ToString()), removeTime);
				return new JsonResult(new FileReturnViewModel()
				{
					Data = new FileReturnDataModel()
					{
						FileName = fileName,
						RequestUrl = $"/file/download?fileid={tmpFile.Id}",
						ValidStamp = DateTime.Now.Add(removeTime),
						Length = tmpFile.Length
					}
				});
			}
		}
	}
}