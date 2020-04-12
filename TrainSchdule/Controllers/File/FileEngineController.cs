using BLL.Extensions;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.File;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Threading.Tasks;
using TrainSchdule.ViewModels;
using TrainSchdule.ViewModels.File;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.Controllers.File
{
	/// <summary>
	/// 文件助手
	/// </summary>
	[Route("[controller]/[action]")]
	public class FileController : Controller
	{
		private readonly IFileServices fileServices;
		private readonly IGoogleAuthService googleAuthService;

		/// <summary>
		///
		/// </summary>
		/// <param name="fileServices"></param>
		public FileController(IFileServices fileServices, IGoogleAuthService googleAuthService)
		{
			this.fileServices = fileServices;
			this.googleAuthService = googleAuthService;
		}

		/// <summary>
		/// 下载指定文件
		/// </summary>
		/// <param name="fileid"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> Download(string fileid)
		{
			var file = Guid.Parse(fileid);
			var f = await Task.Run(() => { return fileServices.Download(file); }).ConfigureAwait(true);
			if (f == null) return new JsonResult(ActionStatusMessage.Static.FileNotExist);
			var fileInfo = fileServices.FileInfo(f.Id);
			new FileExtensionContentTypeProvider().TryGetContentType(fileInfo.Name, out var contentType);
			return File(f.Data, contentType ?? "text/plain", fileInfo.Name);
		}

		/// <summary>
		/// 获取指定文件的信息
		/// </summary>
		/// <param name="filepath"></param>
		/// <param name="filename"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> Load(string filepath, string filename)
		{
			var file = await Task.Run(() => { return fileServices.Load(filepath, filename); }).ConfigureAwait(true);
			if (file == null) return new JsonResult(ActionStatusMessage.Static.FileNotExist);
			return new JsonResult(new FileInfoViewModel()
			{
				Data = new FileInfoDataModel()
				{
					File = file.ToVdto()
				}
			});
		}

		/// <summary>
		/// 上传文件
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[DisableRequestSizeLimit]
		public async Task<IActionResult> Upload([FromForm]FileUploadViewModel model)
		{
			try
			{
				await fileServices.Upload(
					model.File,
					model.FilePath,
					model.FileName,
					model.ResumeUploadId == null ? Guid.Empty : Guid.Parse(model.ResumeUploadId),
					model.ClientKey == null ? Guid.Empty : Guid.Parse(model.ClientKey));
			}
			catch (ActionStatusMessageException ex)
			{
				return new JsonResult(ex.Status);
			}
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 获取当前文件传输状态
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Status()
		{
			var r = fileServices.Stauts();
			return new JsonResult(new FileTransferStatusViewModel()
			{
				Data = r
			});
		}

		/// <summary>
		/// 查询上传码
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult ClientKey([FromBody]FileClientKeySpyViewModel model)
		{
			if (!ModelState.IsValid) return new JsonResult(ModelState.ToModel());
			if (model.Auth?.AuthByUserID != "root") return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			if (!model.Auth?.Verify(googleAuthService, null) ?? false) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var f = fileServices.FileInfo(model.Id);
			return new JsonResult(new ResponseDataTViewModel<Guid>() { Data = f?.ClientKey ?? Guid.Empty });
		}
	}
}