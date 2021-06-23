using BLL.Extensions.Common;
using BLL.Extensions.Party;
using BLL.Helpers;
using DAL.DTO.ZZXT;
using DAL.Entities.Permisstions;
using DAL.Entities.ZZXT;
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
            var p = userActionServices.Permission(currentUserService.CurrentUser, ApplicationPermissions.Party.Confer.NormalConfer.Item, PermissionType.Read, new List<string> { companyCode }, "用户参会记录列表", out var failCompany);
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
            var action = dataUpdateServices.Update(new EntityModifyExtensions.DataUpdateModel<PartyUserRecord>()
            {
                AuthUser = authUser,
                BeforeAdd = v =>
                 {
                     v.Create = DateTime.Now;
                 },
                BeforeModify = (cur, prev) =>
                {
                    prev.ConferenceId = cur.ConferenceId;
                    prev.Type = cur.Type;
                    prev.UserId = cur.UserId;
                },
                Db = context.PartyUserRecords,
                Item = model.Data.ToModel(context),
                UpdateJudge = new EntityModifyExtensions.PermissionJudgeItem<PartyUserRecord>()
                {
                    CompanyGetter = c => c.User.CompanyInfo.CompanyCode,
                    Description = "用户操作记录",
                    Permission = ApplicationPermissions.Party.Confer.ConferRecord.Item,
                },
                QueryItemGetter = c => c.Id == model.Data.Id
            });
            if (action.Item1 == EntityModifyExtensions.ActionType.Update && !model.AllowOverwrite) return new JsonResult(ActionStatusMessage.CheckOverwrite);
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
            var action = dataUpdateServices.Update(new EntityModifyExtensions.DataUpdateModel<PartyUserRecordContent>()
            {
                AuthUser = authUser,
                BeforeAdd = v =>
                {
                    v.Create = DateTime.Now;
                },
                BeforeModify = (cur, prev) =>
                {
                    prev.Content = cur.Content;
                    prev.ContentType = cur.ContentType;
                    prev.RecordId = cur.RecordId;
                },
                Db = context.PartyUserRecordContents,
                Item = model.Data.ToModel(context),
                UpdateJudge = new EntityModifyExtensions.PermissionJudgeItem<PartyUserRecordContent>()
                {
                    CompanyGetter = c => c.Record.User.CompanyInfo.CompanyCode,
                    Description = "操作记录内容",
                    Permission = ApplicationPermissions.Party.Confer.ConferRecord.Item
                },
                QueryItemGetter = c => c.Id == model.Data.Id
            });
            if (action.Item1 == EntityModifyExtensions.ActionType.Update && !model.AllowOverwrite) return new JsonResult(ActionStatusMessage.CheckOverwrite);
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
                var p = userActionServices.Permission(currentUserService.CurrentUser, ApplicationPermissions.Party.Confer.ConferRecord.Item, PermissionType.Read, new List<string> { companyCode }, "记录内容", out var failCompany);
                if (!p) throw new ActionStatusMessageException(new ApiResult(new GoogleAuthDataModel().PermitDenied(), $"授权到{companyCode}", true));
                list = list.Where(i => i.RecordId == recordGuid);
            }
            else if (!conferGuid.Equals(Guid.Empty))
            {
                var confer = context.PartyConferences.FirstOrDefault(i => i.Id == conferGuid);
                if (confer == null) throw new ActionStatusMessageException(confer.NotExist());
                var companyCode = confer.CreateByCode;
                var p = userActionServices.Permission(currentUserService.CurrentUser, ApplicationPermissions.Party.Confer.ConferRecord.Item, PermissionType.Read, new List<string> { companyCode }, "记录内容", out var failCompany);
                if (!p) throw new ActionStatusMessageException(new ApiResult(new GoogleAuthDataModel().PermitDenied(), $"授权到{companyCode}", true));
                list = list.Where(i => i.Record.ConferenceId == conferGuid);
            }
            else
                throw new ActionStatusMessageException(ActionStatusMessage.StaticMessage.IdIsNull);
            var result = list.SplitPage(pageIndex, pageSize);
            return new JsonResult(new EntitiesListViewModel<PartyConferRecordContentDto>(result.Item1.Select(i => i.ToDto()), result.Item2));
        }
    }
}
