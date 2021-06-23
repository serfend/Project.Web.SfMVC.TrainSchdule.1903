using Abp.Extensions;
using BLL.Extensions.Common;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.Common;
using DAL.Data;
using DAL.DTO.ClientDevice;
using DAL.Entities;
using DAL.Entities.ClientDevice;
using DAL.Entities.Permisstions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.Extensions.Common;
using TrainSchdule.ViewModels.ClientDevice;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.Controllers.ClientDevices
{
    /// <summary>
    /// 终端标签管理
    /// </summary>
    public class ClientTagController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IUserActionServices userActionServices;
        private readonly ICurrentUserService currentUserService;
        private readonly IGoogleAuthService googleAuthService;
        private readonly IUsersService usersService;
        private readonly IDataUpdateServices dataUpdateServices;

        /// <summary>
        /// 
        /// </summary>
        public ClientTagController(ApplicationDbContext context, IUserActionServices userActionServices, ICurrentUserService currentUserService, IGoogleAuthService googleAuthService, IUsersService usersService, IDataUpdateServices dataUpdateServices)
        {
            this.context = context;
            this.userActionServices = userActionServices;
            this.currentUserService = currentUserService;
            this.googleAuthService = googleAuthService;
            this.usersService = usersService;
            this.dataUpdateServices = dataUpdateServices;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="appName"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Children(string id, string appName = "default")
        {
            Guid? target;
            var list = context.ClientTagsDb;
            // 查询根节点时判断应用名称
            if (id.Equals("root"))
            {
                list = list.Where(t => t.AppName == appName);
                target = null;
            }
            else
            {
                _ = Guid.TryParse(id, out var tmp);
                if (tmp == Guid.Empty) target = null;
                else target = tmp;
            }
            var tags = list.Where(c => c.ParentId == target).Select(i => i.ToDto());
            return new JsonResult(new EntitiesListViewModel<ClientTagDto>(tags));
        }

        /// <summary>
        /// 获取终端的tags
        /// </summary>
        /// <param name="machineId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult List(string machineId)
        {
            var client = context.ClientsDb.FirstOrDefault(c => c.MachineId == machineId);
            if (client == null) throw new ActionStatusMessageException(client.NotExist());
            var list = context.ClientWithTags.Where(c => c.ClientId == client.Id).Select(c => c.ClientTags);
            return new JsonResult(new EntitiesListViewModel<ClientTags>(list.ToList()));
        }
        /// <summary>
        /// 修改终端的tags
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List([FromBody] ClientWithTagViewModel model)
        {
            var client = context.ClientsDb.FirstOrDefault(c => c.MachineId == model.Data.MachineId);
            if (client == null) throw new ActionStatusMessageException(client.NotExist());
            var p = userActionServices.Permission(currentUserService.CurrentUser, ApplicationPermissions.Client.Manage.Info.Item, PermissionType.Write, new List<string>() { client.CompanyCode }, "标签列表", out var failCompany);
            if (!p) throw new ActionStatusMessageException(new ApiResult(model.Auth.PermitDenied(), $"授权到{client.CompanyCode}", true));
            var list = context.ClientWithTags.Where(c => c.ClientId == client.Id);
            context.ClientWithTags.RemoveRange(list);
            var toAdd = model.Data.Tags.Select(t =>
            {
                _ = Guid.TryParse(t, out var guid);
                return new ClientWithTags()
                {
                    ClientId = client.Id,
                    ClientTagsId = guid
                };
            });
            context.ClientWithTags.AddRange(toAdd);
            context.SaveChanges();
            return new JsonResult(ActionStatusMessage.Success);
        }
        /// <summary>
        /// 更新标签内容
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Info([FromBody] ClientTagDataModel model)
        {
            var item = model.Data.ToModel();
            if (item.CreateCompany == null)
                item.CreateCompany = currentUserService.CurrentUser.CompanyInfo?.CompanyCode;
            var currentUser = model.Auth.AuthUser(googleAuthService, usersService, currentUserService.CurrentUser);
            var action = dataUpdateServices.Update(new EntityModifyExtensions.DataUpdateModel<ClientTags>()
            {
                Item = item,
                AuthUser = currentUser,
                BeforeAdd = v =>
                {
                    v.Create = DateTime.Now;
                },
                BeforeModify = (cur, prev) =>
                {
                    prev.AppName = cur.AppName;
                    prev.Color = cur.Color;
                    prev.CreateCompany = cur.CreateCompany;
                    prev.Description = cur.Description;
                    prev.Name = cur.Name;
                    prev.ParentId = cur.ParentId;
                },
                Db = context.ClientTags,
                UpdateJudge =  new EntityModifyExtensions.PermissionJudgeItem<ClientTags>()
                {
                    CompanyGetter = c => c.CreateCompany,
                    Description = "标签内容",
                    Permission = ApplicationPermissions.Client.Manage.Info.Item,
                },
                QueryItemGetter = c => c.Id == model.Data.Id
            });
            if (action.Item1 == EntityModifyExtensions.ActionType.Update && !model.AllowOverwrite) return new JsonResult(ActionStatusMessage.CheckOverwrite);
            context.SaveChanges();
            return new JsonResult(ActionStatusMessage.Success);
        }
        /// <summary>
        /// 获取标签内容
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Info(string name)
        {
            var tag = context.ClientTagsDb.FirstOrDefault(c => c.Name == name);
            return new JsonResult(new EntityViewModel<ClientTagDto>(tag.ToDto()));
        }
    }
}
