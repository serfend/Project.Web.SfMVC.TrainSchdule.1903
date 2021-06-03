﻿using Abp.Extensions;
using BLL.Extensions.Common;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.ClientDevice;
using DAL.Data;
using DAL.Entities.ClientDevice;
using DAL.Entities.Permisstions;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.BBS;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.Controllers.ClientDevices
{
    public partial class ClientController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IUsersService usersService;
        private readonly IUserActionServices userActionServices;
        private readonly ICurrentUserService currentUserService;

        /// <summary>
        /// 
        /// </summary>
        public ClientController(ApplicationDbContext context, IUsersService usersService, IUserActionServices userActionServices, ICurrentUserService currentUserService)
        {
            this.context = context;
            this.usersService = usersService;
            this.userActionServices = userActionServices;
            this.currentUserService = currentUserService;
        }
    }
    /// <summary>
    /// 终端信息
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public partial class ClientController : Controller
    {
        
        /// <summary>
        /// 终端信息
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Info([FromBody] ClientDeviceDataModel model)
        {
            var r = context.ClientsDb.FirstOrDefault(i => i.MachineId == model.MachineId);
            var client = r ?? new Client();
            model.ToModel(usersService, context.CompaniesDb,client);
            if (!client.Company?.Code.IsNullOrEmpty()??false)
                if (!userActionServices.Permission(currentUserService.CurrentUser, ApplicationPermissions.Client.Manage.Info.Item, PermissionType.Write, client.Company.Code, "终端新信息"))
                    throw new ActionStatusMessageException(new ApiResult(new GoogleAuthDataModel().PermitDenied(), $"授权到{client.Company.Code}", true));
            if (!r.Company?.Code.IsNullOrEmpty()?? false)
                if (!userActionServices.Permission(currentUserService.CurrentUser, ApplicationPermissions.Client.Manage.Info.Item, PermissionType.Write, r.Company.Code, "终端原信息"))
                    throw new ActionStatusMessageException(new ApiResult(new GoogleAuthDataModel().PermitDenied(), $"授权到{r.Company.Code}", true));
            if (client.IsRemoved && r != null)
            {
                r.Remove();
                context.Clients.Update(r);
            }
            else if (r == null) context.Clients.Add(client);
            else context.Clients.Update(client);
            BackgroundJob.Schedule<IClientDeviceService>(s => s.UpdateClientTags(r.Id,client.Id), TimeSpan.FromSeconds(3));
            if (r.OwnerId != client.OwnerId || r.CompanyCode!=client.CompanyCode)
            {
                BackgroundJob.Schedule<IClientDeviceService>(s=>s.UpdateClientRelate(client.Id),TimeSpan.FromSeconds(3));
            }
            context.SaveChanges();
            return new JsonResult(ActionStatusMessage.Success);
        }
        /// <summary>
        /// 查询终端 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Info([FromBody] ClientDeviceQueryDataModel model)
        {
            var list = context.ClientsDb;
            var ip = model.Ip?.Value;
            if (ip != null) list = list.Where(i=>i.Ip.Contains(ip));
            var company = model.Company?.Value;
            if (company != null) list = list.Where(i=>i.Company!=null).Where(i => i.CompanyCode.StartsWith(company));
            var MachineId = model.MachineId?.Value;
            if (MachineId != null) list = list.Where(i => i.MachineId == MachineId);
            var deviceType = model.DeviceType?.Value;
            if (deviceType != null) list = list.Where(i => i.DeviceType.Contains(deviceType));
            var futherInfo = model.FutherInfo?.Value;
            if (futherInfo != null) list = list.Where(i => i.FutherInfo.Contains(futherInfo));
            var owner = model.Owner?.Value;
            if (owner != null) list = list.Where(i => i.OwnerId==owner);
            var result = list.OrderBy(i => i.IpInt).SplitPage(model.Pages);
            return new JsonResult(
                new EntitiesListViewModel<ClientDeviceDataModel>(
                    result.Item1.Select(i=>i.ToModel()), result.Item2));
        }
    }
    public partial class ClientController
    {
        /// <summary>
        /// 修改附加信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult FutherInfo([FromBody] ClientDeviceDataModel model) {
            var client = context.ClientsDb.FirstOrDefault(i => i.Id == model.Id);
            if (client == null) return new JsonResult(client.NotExist());
            client.FutherInfo = model.FutherInfo;
            context.Clients.Update(client);
            context.SaveChanges();
            return new JsonResult(ActionStatusMessage.Success);
        }
    }
}
