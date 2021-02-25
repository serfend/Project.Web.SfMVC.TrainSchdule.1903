using Abp.Extensions;
using BLL.Extensions.Common;
using BLL.Helpers;
using BLL.Interfaces.Common;
using DAL.Data;
using DAL.Entities.ClientDevice;
using DAL.Entities.Common.DataDictionary;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.ClientDevice;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.Controllers.ClientDevices
{
    /// <summary>
    /// 病毒报告
    /// </summary>
   [ApiController]
    [Route("[controller]/[action]")]
    public partial class ClientVirusController:Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IDataDictionariesServices dataDictionariesServices;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dataDictionariesServices"></param>
        public ClientVirusController(ApplicationDbContext context, IDataDictionariesServices dataDictionariesServices)
        {
            this.context = context;
            this.dataDictionariesServices = dataDictionariesServices;
        }
    }
    public partial class ClientVirusController{

        /// <summary>
        /// 病毒编辑
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Info([FromBody] VirusDataModel model)
        {
            if (model.Key.IsNullOrEmpty()) return new JsonResult(ActionStatusMessage.StaticMessage.IdIsNull);
            var r = context.VirusesDb.FirstOrDefault(i => i.Key == model.Key);
            var client = r ?? new Virus();
            model.ToModel(context.Clients, client);
            if (client.IsRemoved && r != null) client.Remove();
            else if (r == null) context.Viruses.Add(client);
            else context.Viruses.Update(client);
            context.SaveChanges();
            return new JsonResult(ActionStatusMessage.Success);
        }
        /// <summary>
        /// 查询病毒 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Info([FromBody]VirusQueryDataModel model)
        {
            var list = context.VirusesDb;
            var createStart = model.Create?.Start;
            var createEnd = model.Create?.End;
            if (createStart != null && createEnd != null && createStart > DateTime.MinValue && createEnd > DateTime.MinValue)
                list = list.Where(i => i.Create >= createStart && i.Create <= createEnd);
            var client = model.Client?.Value;
            if (client != null) list = list.Where(i => i.ClientMachineId == client);
            var ip = model.Ip?.Value;
            if (ip != null) list = list.Where(i => i.ClientIp.Contains(ip));
            var fileName = model.FileName?.Value;
            if (fileName != null) list = list.Where(i => i.FileName.Contains(fileName));
            var type = model.Type?.Value;
            if (type != null) list = list.Where(i => i.Type.Contains(type));
            var status = model.Status?.Arrays;
            if (status != null)
            {
                int status_int = status.Sum(i => i);
                list = list.Where(i => ((int)i.Status & status_int) > 0);
            }
                
            var result = list.OrderByDescending(i=> i.Status).ThenByDescending(i => i.Create).SplitPage(model.Pages);
            return new JsonResult(new EntitiesListViewModel<VirusDataModel>(result.Item1.Select(i => i.ToModel()), result.Item2));
        }
    }

    public partial class ClientVirusController
    {

        /// <summary>
        /// 查询病毒类型 TODO
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="description">描述包含</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult VirusType(string type,string description)
        {
            return new JsonResult(ActionStatusMessage.Success);
        }

        /// <summary>
        /// 获取状态列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult StatusDict()
        {
            var dict = dataDictionariesServices.GetByGroupName(ApplicationDbContext.clientVirusStatus);
            return new JsonResult(new EntityViewModel<Dictionary<string, CommonDataDictionary>>(new Dictionary<string, CommonDataDictionary>(dict.Select(s => new KeyValuePair<string, CommonDataDictionary>(s.Value.ToString(), s)))));
        }
    }
}
