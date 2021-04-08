using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.BBS;
using BLL.Interfaces.File;
using DAL.Entities;
using DAL.Entities.BBS;
using DAL.Entities.Permisstions;
using DAL.QueryModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System;
using TrainSchdule.ViewModels.BBS;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.Controllers.BBS
{
	/// <summary>
	/// 动态（朋友圈模型）管理
	/// </summary>
	[Route("[controller]/[action]")]
	[Authorize]
	public class PostController : Controller
	{
		private readonly IPostServices postServices;
		private readonly IUsersService usersService;
		private readonly IUserActionServices userActionServices;
		private readonly ICurrentUserService currentUserService;
        /// <summary>
        /// 动态发布
        /// </summary>
        /// <param name="postServices"></param>
        /// <param name="usersService"></param>
        /// <param name="currentUserService"></param>
        /// <param name="userActionServices"></param>
        public PostController(IPostServices postServices, IUsersService usersService, ICurrentUserService currentUserService, IUserActionServices userActionServices)
        {
            this.postServices = postServices;
            this.usersService = usersService;
            this.currentUserService = currentUserService;
            this.userActionServices = userActionServices;
        }
		/// <summary>
		/// 点赞
		/// </summary>
		/// <param name="id"></param>
		/// <param name="like"></param>
		/// <returns></returns>
		[Authorize]
		[HttpPost]
		public IActionResult Like([FromBody]Guid id,[FromBody]bool like)
        {
			var u = currentUserService.CurrentUser;
			var post = postServices.GetPostById(id);
			postServices.LikeContent(post,u, like);
			return new JsonResult(ActionStatusMessage.Success);
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
			var user = currentUserService.CurrentUser;
			postServices.CreatePost(new PostContent() {
				Title = model.Title,
				Contents = model.Content,
				Images = model.Images,
				CreateBy = user,
				ReplySubject = postServices.GetPostById(model.ReplySubject),
				ReplyTo = usersService.GetById(model.ReplyTo)
			});
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 按条件筛选动态列表
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(typeof(ActionStatusMessage), 0)]
		public IActionResult Post([FromBody]QueryContentViewModel model)
		{
			var list = postServices.QueryPost(model);
			// TODO 应按人员单位及好友关系划分是否可见
			return new JsonResult(new EntitiesListViewModel<PostContent>(list.Item1,list.Item2));
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
			var post = postServices.GetPostById(id);
			var targetUser = post.CreateBy;
			var user = currentUserService.CurrentUser;
			var permit = userActionServices.Permission(targetUser, ApplicationPermissions.Activity.Post.Item,PermissionType.Write,targetUser.CompanyInfo.CompanyCode);
			if (permit) postServices.RemoveContent(post);
			return  new JsonResult(ActionStatusMessage.Success);
		}
	}
}