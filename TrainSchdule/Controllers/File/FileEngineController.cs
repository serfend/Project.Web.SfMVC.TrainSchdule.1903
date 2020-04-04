using BLL.Helpers;
using BLL.Interfaces.File;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.File;

namespace TrainSchdule.Controllers.File
{
	/// <summary>
	/// 文件助手
	/// </summary>
	[Route("[controller]/[action]")]
	public class FileController : Controller
	{
		private readonly IFileServices fileServices;

		/// <summary>
		///
		/// </summary>
		/// <param name="fileServices"></param>
		public FileController(IFileServices fileServices)
		{
			this.fileServices = fileServices;
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
			return File(f.Data, "text/plain");
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
					File = file
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
	}
}