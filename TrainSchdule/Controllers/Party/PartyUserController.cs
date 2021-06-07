using Abp.Extensions;
using BLL.Extensions.Common;
using BLL.Extensions.Party;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Data;
using DAL.DTO.ZZXT;
using DAL.Entities.Permisstions;
using DAL.Entities.ZZXT;
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
    /// 党团用户信息
    /// </summary>
    [Route("[controller]")]
    public partial class PartyUserController : Controller
    {
        /// <summary>
        /// 查询人员信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("List")]
        [HttpPost]
        public IActionResult List([FromBody] QueryPartyUserViewModel model)
        {
            var currentUser = currentUserService.CurrentUser;
            var list = context.UserPartyInfosDb;
            var company = model.Company?.Value ?? currentUser.CompanyInfo.CompanyCode;
            var permit = userActionServices.Permission(currentUser, ApplicationPermissions.User.PartyInfo.Item, PermissionType.Read, company, "党团信息");
            if (!currentUser.CompanyInfo.CompanyCode.StartsWith(company) && permit) throw new ActionStatusMessageException(new GoogleAuthDataModel().PermitDenied());
            list = list.Where(u => u.CompanyCode == company);
            Guid.TryParse(model.Group?.Value, out var group);
            if (group != Guid.Empty) list = list.Where(u => u.PartyGroupId == group);
            var labor = model.Labor?.Start;
            if (labor != null) list = list.Where(u => u.DutyId == labor);
            var type = model.TypeInParty?.Start;
            if (type != null) list = list.Where(u => u.TypeInParty == (TypeInParty)type);
            var result = list.OrderByDescending(u => u.User.UserOrderRank).SplitPage(model.Page);
            return new JsonResult(new EntitiesListViewModel<UserPartyInfoDto>(result.Item1.Select(i => i.ToDto()), result.Item2));
        }
        /// <summary>
        /// 创建党团
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Info")]
        [HttpPut]
        public IActionResult PutInfo([FromBody] PartyUserViewModel model)
        {
            var user = usersService.GetById(model.Data.UserName);
            if (user == null) throw new ActionStatusMessageException(user.NotExist());
            var prev = context.UserPartyInfosDb.FirstOrDefault(p => p.UserName == model.Data.UserName);
            if (prev != null) throw new ActionStatusMessageException(prev.Exist());
            var comp = user.CompanyInfo.CompanyCode;
            var authUser = model.Auth.AuthUser(googleAuthService, usersService, currentUserService.CurrentUser);
            if (!userActionServices.Permission(authUser, ApplicationPermissions.User.PartyInfo.Item, PermissionType.Write, comp, "党团创建")) throw new ActionStatusMessageException(model.Auth.PermitDenied());
            var p = model.Data.ToModel(context);
            context.UserPartyInfos.Add(p);
            context.SaveChanges();
            return new JsonResult(ActionStatusMessage.Success);
        }
        /// <summary>
        /// 编辑党团
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Info")]
        [HttpPatch]
        public IActionResult PatchInfo([FromBody] PartyUserViewModel model)
        {
            var user = usersService.GetById(model.Data.UserName);
            if (user == null) throw new ActionStatusMessageException(user.NotExist());
            var prev = context.UserPartyInfosDb.FirstOrDefault(p => p.UserName == model.Data.UserName);
            if (prev == null) throw new ActionStatusMessageException(prev.NotExist());
            var comp = prev.User.CompanyInfo.CompanyCode;
            if (!userActionServices.Permission(currentUserService.CurrentUser, ApplicationPermissions.User.PartyInfo.Item, PermissionType.Write, comp, "党团修改")) throw new ActionStatusMessageException(model.Auth.PermitDenied());
            model.Data.ToModel(context, prev);
            context.Update(prev);
            context.SaveChanges();
            return new JsonResult(ActionStatusMessage.Success);
        }
        /// <summary>
        /// 移除党团
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [Route("Info")]
        [HttpDelete]
        public IActionResult RemoveInfo(string userid)
        {
            var prev = context.UserPartyInfosDb.FirstOrDefault(p => p.UserName == userid);
            if (prev == null) throw new ActionStatusMessageException(prev.NotExist());
            var comp = prev.User.CompanyInfo.CompanyCode;
            if (!userActionServices.Permission(currentUserService.CurrentUser, ApplicationPermissions.User.PartyInfo.Item, PermissionType.Write, comp, "党团修改")) throw new ActionStatusMessageException(new GoogleAuthDataModel().PermitDenied());
            prev.Remove();
            context.UserPartyInfos.Update(prev);
            context.SaveChanges();
            return new JsonResult(ActionStatusMessage.Success);
        }
    }
    public partial class PartyUserController
    {
        /// <summary>
        /// 获取党内职务
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult PartyDuties(string name, int pageIndex = 0, int pageSize = 20)
        {
            var duties = context.PartyDutiesDb.Where(d => d.Name.Contains(name)).SplitPage(pageIndex, pageSize);
            return new JsonResult(new EntitiesListViewModel<PartyDuty>(duties.Item1, duties.Item2));
        }
    }
    public partial class PartyUserController
    {
        private readonly ICurrentUserService currentUserService;
        private readonly IUserActionServices userActionServices;
        private readonly IUsersService usersService;
        private readonly IGoogleAuthService googleAuthService;
        private readonly ApplicationDbContext context;

        /// <summary>
        /// 
        /// </summary>
        public PartyUserController(ICurrentUserService currentUserService, IUserActionServices userActionServices, IUsersService usersService, IGoogleAuthService googleAuthService, ApplicationDbContext context)
        {
            this.currentUserService = currentUserService;
            this.userActionServices = userActionServices;
            this.usersService = usersService;
            this.googleAuthService = googleAuthService;
            this.context = context;
        }
    }
}
