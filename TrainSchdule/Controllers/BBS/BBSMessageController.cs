using BLL.Extensions.Common;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.Common;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.Common.Message;
using DAL.Entities.Permisstions;
using DAL.Entities.UserInfo.UserAppMessage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.BBS;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.Controllers.BBS
{
    /// <summary>
	/// 站内消息
	/// </summary>
	[Route("[controller]/[action]")]
    [Authorize]
    public partial class BBSMessageController:Controller
    {
        private readonly ApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IAppMessageServices appMessageServices;
        private readonly IAppUserMessageInfoServices appUserMessageInfoServices;
        private readonly IUsersService usersService;
        private readonly IAppUserRelateServices appUserRelateServices;
        private readonly IUserActionServices userActionServices;
        private readonly IGoogleAuthService googleAuthService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="currentUserService"></param>
        /// <param name="appMessageServices"></param>
        /// <param name="appUserMessageInfoServices"></param>
        /// <param name="usersService"></param>
        /// <param name="appUserRelateServices"></param>
        /// <param name="userActionServices"></param>
        /// <param name="googleAuthService"></param>
        public BBSMessageController(ApplicationDbContext context,ICurrentUserService currentUserService, IAppMessageServices appMessageServices, IAppUserMessageInfoServices appUserMessageInfoServices,IUsersService usersService,IAppUserRelateServices appUserRelateServices, IUserActionServices userActionServices,IGoogleAuthService googleAuthService)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.appMessageServices = appMessageServices;
            this.appUserMessageInfoServices = appUserMessageInfoServices;
            this.usersService = usersService;
            this.appUserRelateServices = appUserRelateServices;
            this.userActionServices = userActionServices;
            this.googleAuthService = googleAuthService;
        }
    }


    public partial class BBSMessageController
    {
        /// <summary>
        /// 个人IM设置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AppMessageSetting()
        {
            var user = currentUserService.CurrentUser;
            var info = appUserMessageInfoServices.GetInfo(user.Id).ToViewModel();
            return new JsonResult(new EntityViewModel<UserAppMessageInfoViewModel>(info));
        }

        /// <summary>
        /// 修改个人IM设置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AppMessageSetting([FromBody] UserAppMessageInfoViewModel model) {
            var user = currentUserService.CurrentUser;
            var info = appUserMessageInfoServices.GetInfo(user.Id) ;
            info.Setting = model.Setting;
            context.UserAppMessageInfos.Update(info);
            context.SaveChanges();
            return new JsonResult(ActionStatusMessage.Success);
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Send([FromBody]NewMessageModel model) {
            var user = currentUserService.CurrentUser;
            var target = usersService.GetById(model.To);
            var _ = target ?? throw new ActionStatusMessageException(target.NotExist());
            var msg = appMessageServices.Send(user.Id, model.To, model.Content,false).ToViewModel();
            return new JsonResult(new EntityViewModel<AppMessageViewModel>(msg));
        }
        /// <summary>
        /// 获取未读消息标头
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetUnread() {
            var user = currentUserService.CurrentUser;
            var unreads = appMessageServices.GetUnread(user.Id);
            var list = unreads.Select(i => i.ToModel<BBSMessageShadowDataModel>()).ToList();
            return new JsonResult(new EntitiesListViewModel<BBSMessageShadowDataModel>(list));
        }
        /// <summary>
        /// 获取指定用户发送的信息详情
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetDetail(string from,string to) {
            var details = appMessageServices.GetDetail(from,to).Select(i=>i.ToViewModel());
            return new JsonResult(new EntitiesListViewModel<AppMessageViewModel>(details));
        }
        /// <summary>
        /// 筛选消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List([FromBody] QueryAppMessageModel model) {
            var currentUser = currentUserService.CurrentUser;
            var from = model.Item.From?.Value==null? currentUser : usersService.GetById(model.Item.From.Value);
            if (from == null) return new JsonResult(ActionStatusMessage.UserMessage.NotExist);
            if (model.Auth?.AuthByUserID != null)
                currentUser = model.Auth.AuthUser(googleAuthService,usersService, currentUser.Id);

            if(!userActionServices.Permission(from, ApplicationPermissions.Activity.AppMessage.Item, PermissionType.Read, from.CompanyInfo.CompanyCode, $"消息查询:{JsonConvert.SerializeObject(model)}"))throw new ActionStatusMessageException(model.Auth.PermitDenied());
            var result = appMessageServices.Query(model.Item);
            return new JsonResult(new EntitiesListViewModel<AppMessageViewModel>(result.Item1.Select(i=>i.ToViewModel()), result.Item2));
        }
        /// <summary>
        /// 操作消息
        /// CheckExist = 0,
        /// Recall = 1,
        /// Delete = 2
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Action([FromBody] BBSMessageActionModel model)
        {
            var user = currentUserService.CurrentUser;
            var result = appMessageServices.Action(user.Id,model.Message, model.Action).ToViewModel();
            return new JsonResult(new EntityViewModel<AppMessageViewModel>(result));
        }
    }

    public partial class BBSMessageController
    {
        /// <summary>
        /// 变更用户关系
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UserRelationAction([FromBody] BBSMessageUserRelationModel model)
        {
            var user = currentUserService.CurrentUser;
            var result = appUserRelateServices.Action(user.Id, model.Target, model.Relation, model.IsAppend).ToViewModel();
            return new JsonResult(new EntityViewModel<AppUserRelateViewModel>(result));
        }
        /// <summary>
        /// 获取关注/粉丝
        /// </summary>
        /// <param name="user"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        [Route("{user}/{direction}")]
        [HttpGet]
        public IActionResult UserRelation(string user,string direction)
        {
            var directionIsFollow = "follow"== direction;
            var list = appUserRelateServices.RelateUser(user, directionIsFollow);
            return new JsonResult(new EntitiesListViewModel<IEnumerable<string>>(list.Select(i=>new List<string> {
                directionIsFollow?i.ToId:i.FromId,
                ((int)i.Relation).ToString()
            }))) ;
        }
    }
}
