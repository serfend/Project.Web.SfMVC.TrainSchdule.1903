using Abp.Extensions;
using BLL.Extensions.Common;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.ClientDevice;
using BLL.Interfaces.Common;
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
using TrainSchdule.Extensions.Common;
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
        private readonly IGoogleAuthService googleAuthService;
        private readonly IDataUpdateServices dataUpdateServices;

        /// <summary>
        /// 
        /// </summary>
        public ClientController(ApplicationDbContext context, IUsersService usersService, IUserActionServices userActionServices, ICurrentUserService currentUserService, IGoogleAuthService googleAuthService, IDataUpdateServices dataUpdateServices)
        {
            this.context = context;
            this.usersService = usersService;
            this.userActionServices = userActionServices;
            this.currentUserService = currentUserService;
            this.googleAuthService = googleAuthService;
            this.dataUpdateServices = dataUpdateServices;
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
            var currentUser = currentUserService.CurrentUser;
            model.ToModel(usersService, context.CompaniesDb, client);
            var update = dataUpdateServices.Update(new EntityModifyExtensions.DataUpdateModel<Client>()
            {
                Item = client,
                AuthUser = currentUser,
                BeforeModify = (cur, prev) =>
                {
                    prev.CompanyCode = cur.CompanyCode;
                    prev.DeviceType = cur.DeviceType;
                    prev.FutherInfo = cur.FutherInfo;
                    prev.Ip = cur.Ip;
                    prev.OwnerId = cur.OwnerId;
                    prev.Mac = cur.Mac;
                },
                Db = context.Clients,
                UpdateJudge =  new EntityModifyExtensions.PermissionJudgeItem<Client>()
                {
                    CompanyGetter = c => c.CompanyCode,
                    Description = "终端",
                    Permission = ApplicationPermissions.Client.Manage.Info.Item
                },
                QueryItemGetter = c => c.MachineId == model.MachineId
            });
            var prevClientId = r?.Id;
            var clientId = client.Id;
            if (update.Item1 != EntityModifyExtensions.ActionType.Remove && (prevClientId == null || r.OwnerId != client.OwnerId || r.CompanyCode != client.CompanyCode))
            {
                BackgroundJob.Schedule<IClientDeviceService>(s => s.UpdateClientRelate(clientId), TimeSpan.FromSeconds(3));
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
            if (ip != null) list = list.Where(i => i.Ip.Contains(ip));
            var company = model.Company?.Value;
            if (company != null) list = list.Where(i => i.Company != null).Where(i => i.CompanyCode.StartsWith(company));
            var MachineId = model.MachineId?.Value;
            if (MachineId != null) list = list.Where(i => i.MachineId == MachineId);
            var deviceType = model.DeviceType?.Value;
            if (deviceType != null) list = list.Where(i => i.DeviceType.Contains(deviceType));
            var futherInfo = model.FutherInfo?.Value;
            if (futherInfo != null) list = list.Where(i => i.FutherInfo.Contains(futherInfo));
            var owner = model.Owner?.Value;
            if (owner != null) list = list.Where(i => i.OwnerId == owner);
            var result = list.OrderBy(i => i.IpInt).SplitPage(model.Pages);
            return new JsonResult(
                new EntitiesListViewModel<ClientDeviceDataModel>(
                    result.Item1.Select(i => i.ToModel()), result.Item2));
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
        public IActionResult FutherInfo([FromBody] ClientDeviceDataModel model)
        {
            var client = context.ClientsDb.FirstOrDefault(i => i.Id == model.Id);
            if (client == null) return new JsonResult(client.NotExist());
            client.FutherInfo = model.FutherInfo;
            context.Clients.Update(client);
            context.SaveChanges();
            return new JsonResult(ActionStatusMessage.Success);
        }
    }
}
