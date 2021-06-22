using BLL.Extensions.Common;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.Common;
using DAL.Entities;
using DAL.Entities.Permisstions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BLL.Extensions.Common.EntityModifyExtensions;

namespace BLL.Services.Common
{
    public class DataUpdateServices : IDataUpdateServices
    {
        private readonly IUserActionServices userActionServices;
        private readonly ICurrentUserService currentUserService;
        private readonly IUsersService usersService;

        public DataUpdateServices(IUserActionServices userActionServices, ICurrentUserService currentUserService, IUsersService usersService)
        {
            this.userActionServices = userActionServices;
            this.currentUserService = currentUserService;
            this.usersService = usersService;
        }


        public (ActionType, T) Update<T>(DataUpdateModel<T> model) where T : BaseEntityGuid
        {
            var directAllow = model.PermissionJudgeItem.Flag.HasFlag(PermissionFlag.WriteDirectAllow);
            string permit_fail_company = null;
            var prevItem = model.Db.Where(c => !c.IsRemoved).FirstOrDefault(model.QueryItemGetter);
            var prevCompany = prevItem == null ? null : model.PermissionJudgeItem.CompanyGetter(prevItem);
            if (prevItem != null
                && model.PermissionJudgeItem.Flag.HasFlag(PermissionFlag.WriteSelfDirectAllow)
                && ((model.PermissionJudgeItem.UserGetter?.Invoke(prevItem) ?? string.Empty) == model.AuthUser.Id)
                )
                directAllow = true;
            var company = model.PermissionJudgeItem.CompanyGetter(model.Item);
            if (model.Item.IsRemoved)
            {
                if (prevItem == null) throw new ActionStatusMessageException(model.Item.NotExist());
                prevCompany = model.PermissionJudgeItem.CompanyGetter(prevItem);
                var checkIsBan = model.PermissionJudgeItem.Flag.HasFlag(PermissionFlag.GlobalReverse) || model.PermissionJudgeItem.Flag.HasFlag(PermissionFlag.WriteReverse);
                var permit = directAllow ? true : userActionServices.Permission(model.AuthUser, model.PermissionJudgeItem.Permission, checkIsBan ? PermissionType.BanWrite : PermissionType.Write, prevCompany, $"通用移除:{model.PermissionJudgeItem.Description}");
                if (directAllow) userActionServices.Log(DAL.Entities.UserInfo.UserOperation.UpdateInfo, model.AuthUser.Id, $"通用直接操作移除{model.PermissionJudgeItem.Description}", true, DAL.Entities.UserInfo.ActionRank.Infomation);
                if (permit)
                {
                    prevItem.Remove();
                    model.Db.Update(prevItem);
                    return (ActionType.Remove, prevItem);
                }
                else
                    permit_fail_company = prevCompany;
            }
            else
            {
                if (model.Item == null) throw new ActionStatusMessageException(ActionStatusMessage.StaticMessage.IdIsNull);
                if (prevItem != null)
                {
                    var checkIsBan = model.PermissionJudgeItem.Flag.HasFlag(PermissionFlag.GlobalReverse) || model.PermissionJudgeItem.Flag.HasFlag(PermissionFlag.WriteReverse);
                    var permit = directAllow ? true : userActionServices.Permission(currentUserService.CurrentUser, model.PermissionJudgeItem.Permission, checkIsBan ? PermissionType.BanWrite : PermissionType.Write, new List<string>() { prevCompany, company }, $"通用更新:{model.PermissionJudgeItem.Description}");
                    if (directAllow) userActionServices.Log(DAL.Entities.UserInfo.UserOperation.UpdateInfo, model.AuthUser.Id, $"通用直接操作更新{model.PermissionJudgeItem.Description}", true, DAL.Entities.UserInfo.ActionRank.Infomation);

                    if (permit)
                    {
                        model.BeforeModify?.Invoke(model.Item, prevItem);
                        model.Db.Update(prevItem);
                        return (ActionType.Update, prevItem);
                    }
                    else
                        permit_fail_company = company;
                }
                else
                {
                    var checkIsBan = model.PermissionJudgeItem.Flag.HasFlag(PermissionFlag.GlobalReverse) || model.PermissionJudgeItem.Flag.HasFlag(PermissionFlag.WriteReverse);
                    var permit = directAllow ? true : userActionServices.Permission(currentUserService.CurrentUser, model.PermissionJudgeItem.Permission, checkIsBan ? PermissionType.BanWrite : PermissionType.Write, company, $"通用新增:{model.PermissionJudgeItem.Description}");
                    if (directAllow) userActionServices.Log(DAL.Entities.UserInfo.UserOperation.UpdateInfo, model.AuthUser.Id, $"通用直接操作新增:{model.PermissionJudgeItem.Description}", true, DAL.Entities.UserInfo.ActionRank.Infomation);

                    if (permit)
                    {
                        model.BeforeAdd.Invoke(model.Item);
                        model.Db.Add(model.Item);
                        return (ActionType.Add, model.Item);
                    }
                    else
                        permit_fail_company = company;
                }
            }
            if (permit_fail_company != null) throw new ActionStatusMessageException(new ApiResult(ActionStatusMessage.Account.Auth.Invalid.Default, $"需要授权[{permit_fail_company}]时被拒绝", true));

            return (ActionType.Update, prevItem);
        }
    }
}
