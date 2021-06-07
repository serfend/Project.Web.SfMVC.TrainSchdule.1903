using BLL.Extensions.Common;
using BLL.Extensions.Party;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Data;
using DAL.DTO.ZZXT;
using DAL.Entities.ZZXT;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.Controllers.Party
{
    /// <summary>
    /// 党组织
    /// </summary>
    public partial class PartyGroupController
    {
        private readonly ICurrentUserService currentUserService;
        private readonly IUserActionServices userActionServices;
        private readonly IUsersService usersService;
        private readonly IGoogleAuthService googleAuthService;
        private readonly ApplicationDbContext context;

        /// <summary>
        /// 
        /// </summary>
        public PartyGroupController(ICurrentUserService currentUserService, IUserActionServices userActionServices, IUsersService usersService, IGoogleAuthService googleAuthService, ApplicationDbContext context)
        {
            this.currentUserService = currentUserService;
            this.userActionServices = userActionServices;
            this.usersService = usersService;
            this.googleAuthService = googleAuthService;
            this.context = context;
        }
    }

    public partial class PartyGroupController
    {
        /// <summary>
        /// 获取单位的党组织
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CompanyGroup(string code) {
            var currentUser = currentUserService.CurrentUser;
            var group = context.PartyGroups.Where(g => g.CompanyCode == code);
            return new JsonResult(new EntitiesListViewModel<PartyGroupDto>(group.Select(g=>g.ToDto())));
        }
        /// <summary>
        /// 获取党组织的成员
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Members(string id, int pageIndex = 0, int pageSize = 20)
        {
            Guid.TryParse(id, out var guid);
            if (guid.Equals(Guid.Empty)) throw new ActionStatusMessageException(ActionStatusMessage.StaticMessage.IdIsNull);
            var users = context.UserPartyInfosDb.Where(u => u.PartyGroupId == guid).SplitPage(pageIndex,pageSize);
            return new JsonResult(new EntitiesListViewModel<UserPartyInfoDto>(users.Item1.Select(i=>i.ToDto()),users.Item2));
        } 
    }
}