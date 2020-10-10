using BLL.Extensions;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.File;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.Extensions.Common;
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
		private readonly IUsersService usersService;
		private readonly ICurrentUserService currentUserService;
		private readonly IUserActionServices userActionServices;

		/// <summary>
		///
		/// </summary>
		/// <param name="fileServices"></param>
		/// <param name="googleAuthService"></param>
		/// <param name="usersService"></param>
		/// <param name="currentUserService"></param>
		public FileController(IFileServices fileServices, IGoogleAuthService googleAuthService, IUsersService usersService, ICurrentUserService currentUserService, IUserActionServices userActionServices)
		{
			this.fileServices = fileServices;
			this.googleAuthService = googleAuthService;
			this.usersService = usersService;
			this.currentUserService = currentUserService;
			this.userActionServices = userActionServices;
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
			if (f == null) return new JsonResult(ActionStatusMessage.StaticMessage.FileNotExist);
			var fileInfo = fileServices.FileInfo(f.Id);
			new FileExtensionContentTypeProvider().TryGetContentType(fileInfo.Name, out var contentType);
			return File(f.Data, contentType ?? "text/plain", fileInfo.Name);
		}

		/// <summary>
		/// 通过路由指定文件id进行下载
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[Route("{id}")]
		[HttpGet]
		public async Task<IActionResult> StaticFile([FromRoute] string id)
		{
			return await Download(id);
		}

		/// <summary>
		/// 通过路径获取文件
		/// </summary>
		/// <param name="path"></param>
		/// <param name="filename"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> FromPath(string path, string filename)
		{
			var file = await Task.Run(() =>
			{
				return fileServices.Load(path, filename);
			}).ConfigureAwait(true);
			if (file == null) return new JsonResult(ActionStatusMessage.StaticMessage.FileNotExist);
			return await Download(file.Id.ToString()).ConfigureAwait(false);
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
			var file = await Task.Run(() =>
			{
				return fileServices.Load(filepath, filename);
			}).ConfigureAwait(true);
			if (file == null) return new JsonResult(ActionStatusMessage.StaticMessage.FileNotExist);
			var result = file.ToVdto();
			if (result.Path == null) result.Path = file.FullPath();
			return new JsonResult(new FileInfoViewModel()
			{
				Data = new FileInfoDataModel()
				{
					File = result
				}
			});
		}

		/// <summary>
		/// 获取子文件夹
		/// </summary>
		/// <param name="path"></param>
		/// <param name="pageSize"></param>
		/// <param name="pageIndex"></param>
		/// <returns></returns>
		[Authorize]
		[HttpGet]
		public IActionResult Folders(string path, int pageSize = 20, int pageIndex = 0)
		{
			var result = fileServices.Folders(path, new DAL.QueryModel.QueryByPage() { PageIndex = pageIndex, PageSize = pageSize });
			return new JsonResult(new FoldersViewModel()
			{
				Data = new FoldersDataModel()
				{
					Folders = result?.Item1,
					TotalCount = result?.Item2 ?? 0
				}
			});
		}

		/// <summary>
		/// 获取文件夹下的文件
		/// </summary>
		/// <param name="path"></param>
		/// <param name="createBy">文件创建者的用户名，需权限支持</param>
		/// <param name="pageSize"></param>
		/// <param name="pageIndex"></param>
		/// <returns></returns>
		[Authorize]
		[HttpGet]
		public IActionResult FolderFiles(string path, string createBy, int pageSize = 20, int pageIndex = 0)
		{
			var user = createBy == null ? null : usersService.GetById(createBy);
			if (user == null && createBy != null) return new JsonResult(ActionStatusMessage.UserMessage.NotExist);
			if (user != null)
			{
				// 权限判断，后续应改成全局过滤器判断
				var currentUser = currentUserService.CurrentUser;
				var permission = usersService.CheckAuthorizedToUser(currentUser, user);
				var ua = userActionServices.Log(DAL.Entities.UserInfo.UserOperation.FileInspect, user.Id, $"通过 {currentUser?.Id} -> {permission}");
				if (permission == -1) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
				userActionServices.Status(ua, true);
			}
			var result = fileServices.FolderFiles(path, new DAL.QueryModel.QueryByPage() { PageIndex = pageIndex, PageSize = pageSize });
			return new JsonResult(new FileInfosViewModel()
			{
				Data = new FileInfosDataModel()
				{
					Files = result?.Item1?.ToList()?.Select(f => f.ToVdto()),
					TotalCount = result?.Item2 ?? 0
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
		public async Task<IActionResult> Upload([FromForm] FileUploadViewModel model)
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
		public IActionResult ClientKey([FromBody] FileClientKeySpyViewModel model)
		{
			if (model.Auth?.AuthByUserID != "root") return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			if (!model.Auth?.Verify(googleAuthService, null) ?? false) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			var f = fileServices.FileInfo(model.Id);
			return new JsonResult(new ResponseDataTViewModel<Guid>() { Data = f?.ClientKey ?? Guid.Empty });
		}

		/// <summary>
		/// 删除指定文件
		/// </summary>
		/// <param name="path"></param>
		/// <param name="filename"></param>
		/// <param name="clientKey"></param>
		/// <returns></returns>
		[HttpDelete]
		public IActionResult Remove(string path, string filename, string clientKey)
		{
			var file = fileServices.Load(path, filename);
			if (file == null) return new JsonResult(ActionStatusMessage.StaticMessage.FileNotExist);
			var guid = Guid.Parse(clientKey);
			if (file.ClientKey != guid) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			if (fileServices.Remove(file))
				return new JsonResult(ActionStatusMessage.Success);
			return new JsonResult(ActionStatusMessage.Fail);
		}
	}
}