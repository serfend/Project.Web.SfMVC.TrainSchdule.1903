using BLL.Extensions.Common;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Data;
using DAL.Entities.ClientDevice;
using DAL.Entities.Permisstions;
using DAL.Entities.ZZXT.Conference;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.Party;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.Controllers.Party
{
    /// <summary>
    /// 会议管理
    /// </summary>
    public partial class PartyConferenceController:Controller
    {

        /// <summary>
        /// 获取终端的tags
        /// </summary>
        /// <param name="conferenceId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult List(string conferenceId)
        {
            Guid.TryParse(conferenceId, out var conferId);
            if (conferId.Equals(Guid.Empty)) throw new ActionStatusMessageException(ActionStatusMessage.StaticMessage.IdIsNull);
            var list = context.ClientWithTags.Where(c => c.ClientId == conferId).Select(c => c.ClientTags);
            return new JsonResult(new EntitiesListViewModel<ClientTags>(list.ToList()));
        }
        /// <summary>
        /// 修改会议的tags
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List([FromBody] ConferWithTagViewModel model)
        {
            Guid.TryParse(model.Data.Id, out var guid);
            if (guid.Equals(Guid.Empty)) throw new ActionStatusMessageException(ActionStatusMessage.StaticMessage.IdIsNull);
            var client = context.PartyConferences.FirstOrDefault(c => c.Id == guid);
            if (client == null) throw new ActionStatusMessageException(client.NotExist());
            var p = userActionServices.Permission(currentUserService.CurrentUser, ApplicationPermissions.Party.Execuable.Conference.Item, PermissionType.Write, client.CreateByCode, "标签列表");
            if (!p) throw new ActionStatusMessageException(new ApiResult(model.Auth.PermitDenied(), $"授权到{client.CreateByCode}", true));
            var list = context.PartyConferWithTags.Where(c => c.ClientTagsId == client.Id);
            context.PartyConferWithTags.RemoveRange(list);
            var toAdd = model.Data.Tags.Select(t =>
            {
                _ = Guid.TryParse(t, out var guid);
                return new PartyConferWithTag()
                {
                    ConferId = client.Id,
                    ClientTagsId = guid
                };
            });
            context.PartyConferWithTags.AddRange(toAdd);
            context.SaveChanges();
            return new JsonResult(ActionStatusMessage.Success);
        }


    }

    public partial class PartyConferenceController
    {
        private readonly ICurrentUserService currentUserService;
        private readonly IUserActionServices userActionServices;
        private readonly IUsersService usersService;
        private readonly IGoogleAuthService googleAuthService;
        private readonly ApplicationDbContext context;

        /// <summary>
        /// 
        /// </summary>
        public PartyConferenceController(ICurrentUserService currentUserService, IUserActionServices userActionServices, IUsersService usersService, IGoogleAuthService googleAuthService, ApplicationDbContext context)
        {
            this.currentUserService = currentUserService;
            this.userActionServices = userActionServices;
            this.usersService = usersService;
            this.googleAuthService = googleAuthService;
            this.context = context;
        }
    }
}
