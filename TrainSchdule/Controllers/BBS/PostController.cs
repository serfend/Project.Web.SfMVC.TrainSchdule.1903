using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.BBS;
using BLL.Interfaces.File;
using DAL.QueryModel;
using Microsoft.AspNetCore.Mvc;
using System;
using TrainSchdule.ViewModels.BBS;

namespace TrainSchdule.Controllers.BBS
{
	/// <summary>
	/// 动态（朋友圈模型）管理
	/// </summary>
	[Route("[controller]/[action]")]
	public class PostController : Controller
	{
		private readonly IPostServices postServices;
		private readonly IUsersService usersService;
		private readonly IGoogleAuthService googleAuthService;
		private readonly IFileServices fileServices;

		/// <summary>
		///
		/// </summary>
		/// <param name="postServices"></param>
		/// <param name="usersService"></param>
		/// <param name="googleAuthService"></param>
		/// <param name="fileServices"></param>
		public PostController(IPostServices postServices, IUsersService usersService, IGoogleAuthService googleAuthService, IFileServices fileServices)
		{
			this.postServices = postServices;
			this.usersService = usersService;
			this.googleAuthService = googleAuthService;
			this.fileServices = fileServices;
		}

		/// <summary>
		/// 发布动态
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[ProducesResponseType(typeof(ActionStatusMessage), 0)]
		public IActionResult Post([FromBody]PostCreateDataModel model)
		{
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 按条件筛选动态列表
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(typeof(ActionStatusMessage), 0)]
		public IActionResult Post([FromBody]QueryPostViewModel model)
		{
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 删除动态
		/// 使用当前登录用户权限进行删除
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete]
		[ProducesResponseType(typeof(ActionStatusMessage), 0)]
		public IActionResult Post(Guid id)
		{
			return new JsonResult(ActionStatusMessage.Success);
		}
	}
}