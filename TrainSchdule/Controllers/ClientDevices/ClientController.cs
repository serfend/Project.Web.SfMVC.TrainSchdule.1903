using BLL.Extensions.Common;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Data;
using DAL.Entities.ClientDevice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.BBS;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.Controllers.ClientDevices
{
    public partial class ClientController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IUsersService usersService;

        /// <summary>
        /// 
        /// </summary>
        public ClientController(ApplicationDbContext context,IUsersService usersService)
        {
            this.context = context;
            this.usersService = usersService;
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
            var client =r ??new Client();
            model.ToModel(usersService, context.CompaniesDb,client);
            if (client.IsRemoved && r != null) client.Remove();
            else if (r == null) context.Clients.Add(client);
            else context.Clients.Update(client);
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
            if (company != null) list = list.Where(i=>i.Company!=null).Where(i => i.Company.Code.StartsWith(company));
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
