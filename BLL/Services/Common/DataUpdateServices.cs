using BLL.Extensions.Common;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.Common;
using DAL.Data;
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
        private readonly ApplicationDbContext context;

        public DataUpdateServices(IUserActionServices userActionServices, ICurrentUserService currentUserService, IUsersService usersService,ApplicationDbContext context)
        {
            this.userActionServices = userActionServices;
            this.currentUserService = currentUserService;
            this.usersService = usersService;
            this.context = context;
        }
        private PermissionJudgeItem<T> DefaultJudgeItem<T>(PermissionJudgeItem<T> target, PermissionJudgeItem<T> defaultItem) where T : BaseEntityGuid
        {
            if (target == null) return defaultItem;
            var result = new PermissionJudgeItem<T>()
            {
                CompanyGetter = target.CompanyGetter ?? defaultItem.CompanyGetter,
                Description = target.Description ?? defaultItem.Description,
                Flag = target.Flag == PermissionFlag.None ? defaultItem.Flag : target.Flag,
                Permission = target.Permission ?? defaultItem.Permission,
                UserGetter = target.UserGetter ?? defaultItem.UserGetter
            };
            return result;
        }
        private (ActionType, T) HandleRemove<T>(DataUpdateModel<T> model, T prevItem, ref string permit_fail_company) where T : BaseEntityGuid
        {
            if (prevItem == null) throw new ActionStatusMessageException(prevItem.NotExist());
            var judgeItem = DefaultJudgeItem(model.RemoveJudge, model.UpdateJudge);
            var prevCompany = prevItem == null ? null : judgeItem.CompanyGetter(prevItem);
            if (PermissionCheck(model, prevItem, judgeItem, new List<string>() { prevCompany }, "移除"))
            {
                prevItem.Remove();
                model.Db.Update(prevItem);
                return (ActionType.Remove, prevItem);
            }
            else
                permit_fail_company = prevCompany ?? "[空单位]";
            return (ActionType.Remove, null);
        }
        public (ActionType, T) HandleUpdate<T>(DataUpdateModel<T> model, T prevItem, ref string permit_fail_company) where T : BaseEntityGuid
        {
            var judgeItem = model.UpdateJudge;
            var prevCompany = prevItem == null ? null : judgeItem.CompanyGetter(prevItem);
            var company = judgeItem.CompanyGetter(model.Item);
            if (PermissionCheck(model, prevItem, judgeItem, new List<string>() { prevCompany, company }, "更新"))
            {
                model.BeforeModify?.Invoke(model.Item, prevItem);
                model.Db.Update(prevItem);
                return (ActionType.Update, prevItem);
            }
            else
                permit_fail_company = company ?? "[空单位]";
            return (ActionType.Update, null);
        }
        public (ActionType, T) HandleAdd<T>(DataUpdateModel<T> model, ref string permit_fail_company) where T : BaseEntityGuid
        {
            var judgeItem = DefaultJudgeItem(model.AddJudge, model.UpdateJudge);
            var company = judgeItem.CompanyGetter(model.Item);
            if (PermissionCheck(model, model.Item, judgeItem, new List<string>() { company }, "新增"))
            {
                model.BeforeAdd?.Invoke(model.Item);
                model.Db.Add(model.Item);
                return (ActionType.Add, model.Item);
            }
            else
                permit_fail_company = company ?? "[空单位]";
            return (ActionType.Add, null);
        }
        private bool PermissionCheck<T>(DataUpdateModel<T> model, T prevItem, PermissionJudgeItem<T> judgeItem, List<string> companies, string description) where T : BaseEntityGuid
        {
            var directAllow = judgeItem.Flag.HasFlag(PermissionFlag.WriteDirectAllow);
            var directSelfAllow = (prevItem != null
                && judgeItem.Flag.HasFlag(PermissionFlag.WriteSelfDirectAllow)
                && ((judgeItem.UserGetter?.Invoke(prevItem) ?? string.Empty) == model.AuthUser.Id)
                );
            var checkIsBan = judgeItem.Flag.HasFlag(PermissionFlag.GlobalReverse) || judgeItem.Flag.HasFlag(PermissionFlag.WriteReverse);
            var direct = directAllow
                || (!checkIsBan && directSelfAllow);
            var permit = direct
                || userActionServices.Permission(currentUserService.CurrentUser, judgeItem.Permission, checkIsBan ? PermissionType.BanWrite : PermissionType.Write, companies, $"通用{description}:{judgeItem.Description}", out var failCompany);
            var desc = directAllow ? "通用放行所有" : "通用放行个人";
            if (direct) userActionServices.Log(DAL.Entities.UserInfo.UserOperation.UpdateInfo, model.AuthUser.Id, $"{desc}{description}:{judgeItem.Description}", true, DAL.Entities.UserInfo.ActionRank.Infomation);
            return permit;
        }
        /// <summary>
        /// directAllow > ban > directSelfAllow > normalPermission
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public (ActionType, T) Update<T>(DataUpdateModel<T> model) where T : BaseEntityGuid
        {
            string permit_fail_company = null;
            var prevItem = model.Db.Where(c => !c.IsRemoved).FirstOrDefault(model.QueryItemGetter);
            (ActionType, T) result;
            if (model.Item.IsRemoved)
                result = HandleRemove(model, prevItem, ref permit_fail_company);
            else
            {
                if (model.Item == null) throw new ActionStatusMessageException(ActionStatusMessage.StaticMessage.IdIsNull);
                if (prevItem != null)
                    result = HandleUpdate(model, prevItem, ref permit_fail_company);
                else
                    result = HandleAdd(model, ref permit_fail_company);
            }
            if (permit_fail_company != null) throw new ActionStatusMessageException(new ApiResult(ActionStatusMessage.Account.Auth.Invalid.Default, $"需要授权[{permit_fail_company}]时被拒绝", true));

            return result;
        }
    }
}
