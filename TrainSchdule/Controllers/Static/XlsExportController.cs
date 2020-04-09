using BLL.Extensions.ApplyExtensions;
using BLL.Helpers;
using Hangfire;
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
			var list = _context.Applies.Where(a => form.Query.Arrays.Contains(a.Id));
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
			var singleApply = _context.Applies.Where(a => a.Id == form.Query.Value).FirstOrDefault();
			if (singleApply == null) return new JsonResult(ActionStatusMessage.Apply.NotExist);
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
			var tmpFile = new FileInfo(GetTempFile(id));
			tmpFile.Delete();
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 下载指定临时文件
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Download(string id)
		{
			var tmpFile = new FileInfo(GetTempFile(id));
			if (!tmpFile.Exists) return Redirect("/#/404");
			_httpContext.HttpContext.Session.TryGetValue(id, out var filenameRaw);
			var filename = Encoding.UTF8.GetString(filenameRaw);
			using (var f = tmpFile.OpenRead())
			{
				var buffer = new byte[f.Length];
				f.Read(buffer);
				return File(buffer, "application/octet-stream", filename);
			}
		}

		private async Task<IActionResult> ExportXls(byte[] fileContent, string description)
		{
			var datetime = DateTime.Now.ToString("MMdd");
			var fileName = $"TS{datetime}-{description}.xlsx";
			var guid = Guid.NewGuid().ToString();
			var tmpFile = new FileInfo(GetTempFile(guid));
			using (var f = tmpFile.OpenWrite())
			{
				await f.WriteAsync(fileContent);
			}
			var removeTime = TimeSpan.FromMinutes(10);
			_httpContext.HttpContext.Session.Set(guid, Encoding.UTF8.GetBytes(fileName));
			var jobId = BackgroundJob.Schedule(() => RemoveTempFile(guid), removeTime);
			return new JsonResult(new FileReturnViewModel()
			{
				Data = new FileReturnDataModel()
				{
					FileName = fileName,
					RequestUrl = $"/static/download?id={guid}",
					ValidStamp = (long)(DateTime.UtcNow.Add(removeTime) - new DateTime(1970, 1, 1)).TotalMilliseconds,
					Length = tmpFile.Length
				}
			});
		}

		private string GetTempFile(string filename) => Path.Combine(_hostingEnvironment.WebRootPath, "tmp", filename);
	}
}