using Abp.Extensions;
using BLL.Extensions.Common;
using BLL.Extensions.Party;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.Common;
using DAL.Data;
using DAL.DTO.ZZXT;
using DAL.Entities.Permisstions;
using DAL.Entities.ZZXT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.Extensions.Common;
using TrainSchdule.ViewModels.Party;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.Controllers.Party
{
    /// <summary>
    /// 党组织
    /// </summary>

    [Authorize]
    [Route("[controller]/[action]")]
    public partial class PartyGroupController
    {
        private readonly ICurrentUserService currentUserService;
        private readonly IUserActionServices userActionServices;
        private readonly IUsersService usersService;
        private readonly IGoogleAuthService googleAuthService;
        private readonly IDataUpdateServices dataUpdateServices;
        private readonly ApplicationDbContext context;

        /// <summary>
        /// 
        /// </summary>
        public PartyGroupController(ICurrentUserService currentUserService, IUserActionServices userActionServices, IUsersService usersService, IGoogleAuthService googleAuthService, IDataUpdateServices dataUpdateServices, ApplicationDbContext context)
        {
            this.currentUserService = currentUserService;
            this.userActionServices = userActionServices;
            this.usersService = usersService;
            this.googleAuthService = googleAuthService;
            this.dataUpdateServices = dataUpdateServices;
            this.context = context;
        }
    }

    public partial class PartyGroupController
    {

        /// <summary>
        /// 党组织编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Info([FromBody] PartyGroupViewModel model)
        {
            var authUser = model.Auth.AuthUser(googleAuthService, usersService, currentUserService.CurrentUser);
            var item = model.Data.ToModel(context);
            dataUpdateServices.Update(new EntityModifyExtensions.DataUpdateModel<PartyGroup>()
            {
                AuthUser = authUser,
                BeforeAdd = v =>
                {
                    v.Create = DateTime.Now;
                },
                BeforeModify = (cur, prev) =>
                {
                    prev.Alias = cur.Alias;
                    prev.Company = cur.Company;
                    prev.GroupType = cur.GroupType;
                },
                Db = context.PartyGroups,
                Item = model.Data.ToModel(context),
                PermissionJudgeItem = new EntityModifyExtensions.PermissionJudgeItem<PartyGroup>()
                {
                    CompanyGetter = c => c.CompanyCode,
                    Description = "党小组",
                    Permission = ApplicationPermissions.Party.Group.Item
                },
                QueryItemGetter = v => v.Id == model.Data.Id
            });
            return new JsonResult(ActionStatusMessage.Success);
        }
        /// <summary>
        /// 获取单位的党组织
        /// </summary>
        /// <param name="code"></param>
        /// <param name="groupType"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CompanyGroup(string code, int groupType = -1)
        {
            var currentUser = currentUserService.CurrentUser;
            if (code.IsNullOrEmpty()) code = currentUser.CompanyInfo.CompanyCode;
            var group = context.PartyGroups.Where(g => g.CompanyCode == code);
            if (groupType != -1) group = group.Where(g => (int)g.GroupType == groupType);
            return new JsonResult(new EntitiesListViewModel<PartyGroupDto>(group.Select(g => g.ToDto())));
        }
        /// <summary>
        /// 获取党组织的成员
        /// </summary>
        /// <param name="groupid"></param>
        /// <param name="company"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Members(string groupid, string company, int pageIndex = 0, int pageSize = 20)
        {
            var list = GetMembers(groupid, company);
            var users = list.SplitPage(pageIndex, pageSize);
            return new JsonResult(new EntitiesListDataModel<UserPartyInfoDto>(users.Item1.Select(i => i.ToDto()), users.Item2));
        }

        /// <summary>
        /// 获取党组织成员类别统计
        /// </summary>
        /// <param name="groupid"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult MemberStatistics(string groupid, string company)
        {
            var list = GetMembers(groupid, company);
            var result = list
                .GroupBy(i => i.TypeInParty, (a, b) => new { a, c = b.Count() })
                .ToDictionary(x => x.a, x => x.c);
            return new JsonResult(new EntityViewModel<Dictionary<int, int>>(result));
        }
        private IQueryable<UserPartyInfo> GetMembers(string groupid, string company)
        {
            var list = context.UserPartyInfosDb;
            Guid.TryParse(groupid, out var groupGuid);
            if (!groupGuid.Equals(Guid.Empty)) list = list.Where(u => u.PartyGroupId == groupGuid);
            if (!company.IsNullOrEmpty()) list = list.Where(u => u.CompanyCode == company);
            return list;
        }
    }
}