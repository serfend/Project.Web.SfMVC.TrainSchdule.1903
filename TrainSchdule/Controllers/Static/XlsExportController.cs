using BLL.Extensions;
using BLL.Extensions.ApplyExtensions;
using BLL.Helpers;
using DAL.DTO.User;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TrainSchdule.Extensions.Common;
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
			var user_infos = GetUserInfosDict(list);
			var fileContent = _applyService.ExportExcel(filePath, list.Select(a => a.ToDetaiDto(user_infos[a.BaseInfo.From.Id], _context)));
			if (fileContent == null) return new JsonResult(ActionStatusMessage.StaticMessage.XlsNoData);
			return await ExportXls(fileContent, $"{list.Count()}条({form.Templete})");
		}
		/// <summary>
		/// 获取用户休假信息
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		private Dictionary<string, UserVacationInfoVDto> GetUserInfosDict(List<DAL.Entities.ApplyInfo.Apply>list)
        {
			var nowY = DateTime.Now.XjxtNow().Year;
			Dictionary<string, UserVacationInfoVDto> user_infos = new Dictionary<string, UserVacationInfoVDto>();
			foreach (var a in list)
			{
				var uid = a.BaseInfo.From.Id;
				if (user_infos.ContainsKey(uid)) continue;
				var info = _usersService.VacationInfo(a.BaseInfo.From, a.RequestInfo.StampLeave?.Year ?? nowY,a.MainStatus);
				user_infos[uid] = info;
			}
			return user_infos;
		}
		/// <summary>
		/// 依据模板导出单个申请到Xls
		/// </summary>
		/// <param name="form"></param>
		/// <returns></returns>
		[HttpPost]
		[ProducesResponseType(typeof(string), 0)]
		public async Task<IActionResult> ExportApply([FromBody] AppliesExportDataModel form)
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
			var user_infos = GetUserInfosDict(new List<DAL.Entities.ApplyInfo.Apply>() { singleApply });
			var fileContent = _applyService.ExportExcel(filePath, singleApply.ToDetaiDto(user_infos[singleApply.BaseInfo?.From.Id], _context));
			if (fileContent == null) return new JsonResult(ActionStatusMessage.StaticMessage.XlsNoData);
			return await ExportXls(fileContent, $"{singleApply.BaseInfo.RealName}的申请({form.Templete})");
		}

		private string GetFilePath(string templete)
		{
			var sWebRootFolder = env.WebRootPath;
			templete = $"Templete\\{templete}";
			var tempFile = new FileInfo(Path.Combine(sWebRootFolder, templete));
			if (!tempFile.Exists) throw new ActionStatusMessageException(ActionStatusMessage.StaticMessage.TempXlsNotExist);
			return tempFile.FullName;
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
				return new JsonResult(new FileReturnViewModel()
				{
					Data = new FileReturnDataModel()
					{
						FileName = fileName,
						RequestUrl = tmpFile.DownloadUrl(FileExtensions.DownloadType.ByStatic),
						ValidStamp = DateTime.Now.Add(removeTime),
						Length = tmpFile.Length
					}
				});
			}
		}
	}
}