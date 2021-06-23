using BLL.Extensions;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.DTO.Company;
using DAL.Entities;
using DAL.Entities.Permisstions;
using DAL.Entities.UserInfo.Settle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TrainSchdule.Extensions.Common;
using TrainSchdule.ViewModels;
using TrainSchdule.ViewModels.User;
using TrainSchdule.ViewModels.User.Social;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.Controllers
{
    public partial class UsersController
    {
        private readonly ICompanyManagerServices companyManagerServices;

        /// <summary>
        /// 此用户所管辖的单位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserManageRangeDataModel), 0)]
        [Route("[action]")]
        public IActionResult OnMyManage(string id)
        {
            id = id ?? currentUserService.CurrentUser?.Id;
            var targetUser = usersService.GetById(id);
            if (targetUser == null) return new JsonResult(ActionStatusMessage.UserMessage.NotExist);
            var result = usersService.InMyManage(targetUser).Result;
            var list = result.Item1.Select(c => c.ToDto(companiesService));
            return new JsonResult(new UserManageRangeViewModel()
            {
                Data = new UserManageRangeDataModel()
                {
                    List = list,
                    TotalCount = result.Item2
                }
            });
        }

        /// <summary>
        /// 移除管辖单位
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpDelete]
        [AllowAnonymous]
        [Route("[action]")]
        [ProducesResponseType(typeof(ApiResult), 0)]
        public IActionResult OnMyManage([FromBody] UserManageRangeModifyViewModel model)
        {
            if (model.Auth == null || !model.Auth.Verify(authService, currentUserService.CurrentUser?.Id)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
            var id = model.Id ?? currentUserService.CurrentUser?.Id;
            var authUser = usersService.GetById(model.Auth.AuthByUserID);
            if (authUser == null) return new JsonResult(ActionStatusMessage.UserMessage.NotExist);
            var targetUser = usersService.GetById(id);
            var permit = userActionServices.Permission(authUser, ApplicationPermissions.User.Application.Item, PermissionType.Write, new List<string> { model.Code }, "移除管辖单位", out var failCompany);
            if (targetUser == null) return new JsonResult(ActionStatusMessage.UserMessage.NotExist);
            var manages = companyManagerServices.GetManagers(model.Code);
            var manage = manages.Where(u => u.CompanyCode == targetUser.Id).FirstOrDefault();
            ; if (manage == null) return new JsonResult(ActionStatusMessage.CompanyMessage.ManagerMessage.NotExist);
            var unused = companyManagerServices.Delete(manage);
            return new JsonResult(ActionStatusMessage.Success);
        }

        /// <summary>
        /// 新增管辖单位
        /// </summary>
        /// <param name="model"></param>
        /// <param name="mdzz">填充参数，无需填写</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("[action]")]
        [ProducesResponseType(typeof(ApiResult), 0)]
        public IActionResult OnMyManage([FromBody] UserManageRangeModifyViewModel model, string mdzz)
        {
            if (!model.Auth.Verify(authService, currentUserService.CurrentUser?.Id)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
            var authByUser = usersService.GetById(model.Auth.AuthByUserID);
            var id = model.Id ?? currentUserService.CurrentUser?.Id;
            var targetUser = usersService.GetById(id);
            if (targetUser == null) return new JsonResult(ActionStatusMessage.UserMessage.NotExist);
            var permit = userActionServices.Permission(authByUser, ApplicationPermissions.User.Application.Item, PermissionType.Write, new List<string> { model.Code }, "新增管辖单位", out var failCompany);
            if (!permit) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
            var manages = companyManagerServices.GetManagers(model.Code);
            var manage = manages.Where(u => u.UserId == targetUser.Id).FirstOrDefault();
            if (manage != null) return new JsonResult(ActionStatusMessage.CompanyMessage.ManagerMessage.Existed);
            var dto = new CompanyManagerVdto()
            {
                AuditById = model.Auth.AuthByUserID,
                CompanyCode = model.Code,
                UserId = model.Id
            };
            manage = companyManagerServices.CreateManagers(dto);
            if (manage == null) return new JsonResult(ActionStatusMessage.CompanyMessage.ManagerMessage.Default);
            return new JsonResult(ActionStatusMessage.Success);
        }
    }
}