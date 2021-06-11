using Abp.Extensions;
using BLL.Extensions.Common;
using BLL.Extensions.Party;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Data;
using DAL.DTO.ZZXT;
using DAL.Entities.ClientDevice;
using DAL.Entities.Permisstions;
using DAL.Entities.ZZXT.Conference;
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
    /// 会议管理
    /// </summary>
    public partial class PartyConferenceController : Controller
    {

        /// <summary>
        /// 获取会议的tags
        /// </summary>
        /// <param name="conferenceId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Tags(string conferenceId)
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
        public IActionResult Tags([FromBody] ConferWithTagViewModel model)
        {
            Guid.TryParse(model.Data.Id, out var guid);
            if (guid.Equals(Guid.Empty)) throw new ActionStatusMessageException(ActionStatusMessage.StaticMessage.IdIsNull);
            var client = context.PartyConferences.FirstOrDefault(c => c.Id == guid);
            if (client == null) throw new ActionStatusMessageException(client.NotExist());
            var p = userActionServices.Permission(currentUserService.CurrentUser, ApplicationPermissions.Party.Confer.NormalConfer.Item, PermissionType.Write, client.CreateByCode, "标签列表");
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
        /// <summary>
        /// 获取单位所办会议列表
        /// </summary>
        /// <param name="company">所属单位代码</param>
        /// <param name="partyGroup">所属党组织 填写时覆盖<paramref name="company"/>字段
        /// <param name="conferType">会议类型值</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CompanyConferList(string company,string partyGroup, int conferType, int pageIndex = 0, int pageSize = 10)
        {
            var list = context.PartyConferencesDb;
            var useCompany = partyGroup.IsNullOrEmpty();
            string toAuthCompany;
            if (!useCompany)
            {
                Guid.TryParse(partyGroup, out var groupGuid);
                var group = context.PartyGroups.FirstOrDefault(g => g.Id == groupGuid);
                if (group == null) throw new ActionStatusMessageException(group.NotExist());
                toAuthCompany = group.CompanyCode;
            }
            else
                toAuthCompany = company;
            list = list.Where(u => u.CreateByCode == toAuthCompany);
            list = list.Where(u => u.Type == conferType);
            var p = userActionServices.Permission(currentUserService.CurrentUser, ApplicationPermissions.Party.Confer.NormalConfer.Item, PermissionType.Read, company, "会议列表");
            if (!p) throw new ActionStatusMessageException(new ApiResult(new GoogleAuthDataModel().PermitDenied(), $"授权到{company}", true));

            var result = list.SplitPage(pageIndex, pageSize);
            return new JsonResult(new EntitiesListViewModel<PartyBaseConference>(result.Item1, result.Item2));
        }
        /// <summary>
        /// 更新会议
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Confer([FromBody] PartyConferenceViewModel model)
        {
            var authUser = model.Auth.AuthUser(googleAuthService, usersService, currentUserService.CurrentUser);
            var action = model.Data.UpdateGuidEntity(context.PartyConferences, c => c.Id == model.Data.Id, c => c.CreateByCode, model.Auth, ApplicationPermissions.Party.Confer.NormalConfer.Item, PermissionType.Write, "会议", googleAuthService, usersService, currentUserService, userActionServices);
            if (action == EntityModifyExtensions.ActionType.Update && !model.AllowOverwrite) return new JsonResult(ActionStatusMessage.CheckOverwrite);
            context.SaveChanges();
            return new JsonResult(ActionStatusMessage.Success);
        }
    }
    /// <summary>
    /// 参会记录管理
    /// </summary>
    public partial class PartyConferenceController
    {

        /// <summary>
        /// 查询指定会议的参加情况
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ConferRecord(string id, int pageIndex = 0, int pageSize = 20)
        {
            Guid.TryParse(id, out var guid);
            if (guid.Equals(Guid.Empty)) throw new ActionStatusMessageException(ActionStatusMessage.StaticMessage.IdIsNull);
            var confer = context.PartyConferences.FirstOrDefault(c => c.Id == guid);
            if (confer == null) throw new ActionStatusMessageException(confer.NotExist());
            var companyCode = confer.CreateByCode;
            var p = userActionServices.Permission(currentUserService.CurrentUser, ApplicationPermissions.Party.Confer.NormalConfer.Item, PermissionType.Read, companyCode, "用户参会记录列表");
            if (!p) throw new ActionStatusMessageException(new ApiResult(new GoogleAuthDataModel().PermitDenied(), $"授权到{companyCode}", true));
            var list = context.PartyUserRecordsDb.Where(r => r.ConferenceId == guid);
            var result = list.SplitPage(pageIndex, pageSize);
            return new JsonResult(new EntitiesListViewModel<PartyUserRecordDto>(result.Item1.Select(r => r.ToDto()), result.Item2));
        }
        /// <summary>
        /// 更新会议参加情况
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]

        public IActionResult ConferRecord([FromBody] PartyUserRecordViewModel model)
        {
            var authUser = model.Auth.AuthUser(googleAuthService, usersService, currentUserService.CurrentUser);
            var action = model.Data.ToModel().UpdateGuidEntity(context.PartyUserRecords, c => c.Id == model.Data.Id, c => c.User.CompanyInfo.CompanyCode, model.Auth, ApplicationPermissions.Party.Confer.ConferRecord.Item, PermissionType.Write, "用户操作记录", googleAuthService, usersService, currentUserService, userActionServices);
            if (action == EntityModifyExtensions.ActionType.Update && !model.AllowOverwrite) return new JsonResult(ActionStatusMessage.CheckOverwrite);
            context.SaveChanges();
            return new JsonResult(ActionStatusMessage.Success);
        }

    }
    /// <summary>
    /// 参会记录的内容管理
    /// </summary>
    public partial class PartyConferenceController
    {
        /// <summary>
        /// 更新会议参加情况的内容
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ConferRecordContent([FromBody] PartyConferRecordContentViewModel model)
        {
            var authUser = model.Auth.AuthUser(googleAuthService, usersService, currentUserService.CurrentUser);
            var action = model.Data.ToModel().UpdateGuidEntity(context.PartyUserRecordContents, c => c.Id == model.Data.Id, c => c.Record.User.CompanyInfo.CompanyCode, model.Auth, ApplicationPermissions.Party.Confer.ConferRecord.Item, PermissionType.Write, "操作记录内容", googleAuthService, usersService, currentUserService, userActionServices);
            if (action == EntityModifyExtensions.ActionType.Update && !model.AllowOverwrite) return new JsonResult(ActionStatusMessage.CheckOverwrite);
            context.SaveChanges();
            return new JsonResult(ActionStatusMessage.Success);
        }
        /// <summary>
        /// 获取指定会议或会议记录的内容
        /// </summary>
        /// <param name="recordid"></param>
        /// <param name="conferid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ConferRecordContent(string recordid, string conferid, int pageIndex = 0, int pageSize = 20)
        {
            Guid.TryParse(recordid, out var recordGuid);
            Guid.TryParse(conferid, out var conferGuid);
            var list = context.PartyUserRecordContentsDb;
            if (!recordGuid.Equals(Guid.Empty))
            {
                var record = context.PartyUserRecordsDb.FirstOrDefault(i => i.Id == recordGuid);
                if (record == null) throw new ActionStatusMessageException(record.NotExist());
                var companyCode = record.Conference.CreateByCode;
                var p = userActionServices.Permission(currentUserService.CurrentUser, ApplicationPermissions.Party.Confer.ConferRecord.Item, PermissionType.Read, companyCode, "记录内容");
                if (!p) throw new ActionStatusMessageException(new ApiResult(new GoogleAuthDataModel().PermitDenied(), $"授权到{companyCode}", true));
                list = list.Where(i => i.RecordId == recordGuid);
            }
            else if (!conferGuid.Equals(Guid.Empty))
            {
                var confer = context.PartyConferences.FirstOrDefault(i => i.Id == conferGuid);
                if (confer == null) throw new ActionStatusMessageException(confer.NotExist());
                var companyCode = confer.CreateByCode;
                var p = userActionServices.Permission(currentUserService.CurrentUser, ApplicationPermissions.Party.Confer.ConferRecord.Item, PermissionType.Read, companyCode, "记录内容");
                if (!p) throw new ActionStatusMessageException(new ApiResult(new GoogleAuthDataModel().PermitDenied(), $"授权到{companyCode}", true));
                list = list.Where(i => i.Record.ConferenceId == conferGuid);
            }
            else
                throw new ActionStatusMessageException(ActionStatusMessage.StaticMessage.IdIsNull);
            var result = list.SplitPage(pageIndex, pageSize);
            return new JsonResult(new EntitiesListViewModel<PartyConferRecordContentDto>(result.Item1.Select(i => i.ToDto()), result.Item2));
        }
    }

    [Authorize]
    [Route("[controller]/[action]")]

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
