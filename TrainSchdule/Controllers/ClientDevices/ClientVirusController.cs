
using Abp.Extensions;
using Abp.Linq.Expressions;
using BLL.Extensions.Common;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.ClientDevice;
using BLL.Interfaces.Common;
using BLL.Interfaces.Permission;
using DAL.Data;
using DAL.DTO.ClientDevice;
using DAL.Entities.ClientDevice;
using DAL.Entities.Common.DataDictionary;
using DAL.Entities.Permisstions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TrainSchdule.Extensions.Common;
using TrainSchdule.System;
using TrainSchdule.ViewModels.ClientDevice;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.Controllers.ClientDevices
{
    /// <summary>
    /// 病毒报告
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public partial class ClientVirusController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IDataDictionariesServices dataDictionariesServices;
        private readonly IUsersService usersService;
        private readonly IGoogleAuthService googleAuthService;
        private readonly IClientVirusServices clientVirusServices;
        private readonly IUserActionServices userActionServices;
        private readonly ICurrentUserService currentUserService;

        /// <summary>
        /// 
        /// </summary>
        public ClientVirusController(ApplicationDbContext context, IDataDictionariesServices dataDictionariesServices,IUsersService usersService,IGoogleAuthService googleAuthService, IClientVirusServices clientVirusServices, IUserActionServices userActionServices, ICurrentUserService currentUserService)
        {
            this.context = context;
            this.dataDictionariesServices = dataDictionariesServices;
            this.usersService = usersService;
            this.googleAuthService = googleAuthService;
            this.clientVirusServices = clientVirusServices;
            this.userActionServices = userActionServices;
            this.currentUserService = currentUserService;
        }
    }
    public partial class ClientVirusController
    {

        /// <summary>
        /// 病毒编辑
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Info([FromBody] VirusDto model)
        {
            var r = model.ToModel(context.ClientsDb).UpdateGuidEntity(context.Viruses, v => v.Key == model.Key, v => v.Company, null, ApplicationPermissions.Client.Virus.Info.Item, PermissionType.Write, "病毒",(cur,prev)=> {
                prev.ClientMachineId = cur.ClientMachineId;
                prev.Key = cur.Key;
                prev.Sha1 = cur.Sha1;
                prev.ClientIp = cur.ClientIp;
                prev.Create = cur.Create;
                prev.FileName = cur.FileName;
                prev.Type = cur.Type;
            },newItem=> {
                newItem.Create = DateTime.Now;
            }, googleAuthService, usersService, currentUserService, userActionServices);
            if(r.Item1 == EntityModifyExtensions.ActionType.Add)
            {
                r.Item2.Status = VirusStatus.Unhandle;
            }
            context.SaveChanges();
            return new JsonResult(ActionStatusMessage.Success);
        }
        /// <summary>
        /// 查询病毒 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Info([FromBody] VirusQueryDataModel model)
        {
            var list = context.VirusesDb;
            var id = model.Id?.Value;
            if (id != null) return new JsonResult(new EntityViewModel<VirusDto>(list.FirstOrDefault(i => i.Id == id)?.ToModel()));
            var createStart = model.Create?.Start;
            var createEnd = model.Create?.End;
            var companies = model.Companies?.Arrays;
            if (companies != null && companies.Any())
            {
                list = list.Where(i => i.Company != null);
                var exp = PredicateBuilder.New<Virus>(false);
                foreach (var c in companies)
                    exp = exp.Or(i => i.Company.StartsWith(c));
                list = list.Where(exp);
            }
            if (createStart != null && createEnd != null && createStart > DateTime.MinValue && createEnd > DateTime.MinValue)
                list = list.Where(i => i.Create >= createStart && i.Create <= createEnd);
            var client = model.Client?.Value;
            if (client != null) list = list.Where(i => i.ClientMachineId == client);
            var ip = model.Ip?.Value;
            if (ip != null) list = list.Where(i => i.ClientIp.Contains(ip));
            var createBy = model.CreateBy?.Value;
            if (createBy != null) list = list.Where(i => i.Owner == createBy);
            var fileName = model.FileName?.Value;
            if (fileName != null) list = list.Where(i => i.FileName.Contains(fileName));
            var type = model.Type?.Value;
            if (type != null)
            {
                var exp = PredicateBuilder.New<Virus>(false);
                exp.Or(i => i.Type.Contains(type));
                exp.Or(i => i.TraceAlias.Contains(type));
                list = list.Where(exp);
            }
            var status = model.Status?.Arrays;
            if (status != null)
            {
                int status_int = status.Sum(i => i);
                list = list.Where(i => ((int)i.Status & status_int) > 0);
            }

            var result = list.OrderByDescending(i => (i.Status & VirusStatus.Unhandle)).ThenByDescending(i => i.Create).SplitPage(model.Pages);
            return new JsonResult(new EntitiesListViewModel<VirusDto>(result.Item1.Select(i => i.ToModel()), result.Item2));
        }
    }

    public partial class ClientVirusController
    {

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
