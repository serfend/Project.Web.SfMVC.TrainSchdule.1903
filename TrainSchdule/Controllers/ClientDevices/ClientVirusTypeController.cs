using Abp.Extensions;
using Abp.Linq.Expressions;
using BLL.Extensions.Common;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.ClientDevice;
using BLL.Interfaces.Common;
using DAL.Data;
using DAL.DTO.ClientDevice;
using DAL.Entities.ClientDevice;
using DAL.Entities.Permisstions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TrainSchdule.Extensions.Common;
using TrainSchdule.ViewModels.ClientDevice;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.Controllers.ClientDevices
{
    /// <summary>
    /// 病毒报告
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public partial class ClientVirusTypeController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IClientVirusServices clientVirusServices;
        private readonly IUsersService usersService;
        private readonly ICurrentUserService currentUserService;
        private readonly IGoogleAuthService googleAuthService;
        private readonly IUserActionServices userActionServices;
        private readonly IDataUpdateServices dataUpdateServices;

        /// <summary>
        /// 
        /// </summary>
        public ClientVirusTypeController(ApplicationDbContext context, IClientVirusServices clientVirusServices,IUsersService usersService,ICurrentUserService currentUserService,IGoogleAuthService googleAuthService,IUserActionServices userActionServices,IDataUpdateServices dataUpdateServices)
        {
            this.context = context;
            this.clientVirusServices = clientVirusServices;
            this.usersService = usersService;
            this.currentUserService = currentUserService;
            this.googleAuthService = googleAuthService;
            this.userActionServices = userActionServices;
            this.dataUpdateServices = dataUpdateServices;
        }
    }

    public partial class ClientVirusTypeController
    {
        /// <summary>
        /// 病毒编辑类型
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Info([FromBody] VirusTypeDataModel model)
        {
            var item = model.ToModel();
            var currentUser = currentUserService.CurrentUser;
            dataUpdateServices.Update(new EntityModifyExtensions.DataUpdateModel<VirusTrace>() {
                Item = item,
                AuthUser = currentUser,
                BeforeAdd = v =>
                {
                    v.Create = DateTime.Now;
                },
                BeforeModify = (cur, prev) =>
                {
                    prev.Alias = cur.Alias;
                    prev.Description = cur.Description;
                    prev.Sha1 = cur.Sha1;
                    prev.WarningLevel = cur.WarningLevel;
                    prev.Type = cur.Type;
                },
                Db = context.VirusTraces,
                UpdateJudge =  new EntityModifyExtensions.PermissionJudgeItem<VirusTrace>()
                {
                    CompanyGetter = c=>null,
                    Description = "病毒类型",
                    Permission = ApplicationPermissions.Client.Virus.Info.Item
                },
                QueryItemGetter = c=>c.Id == model.Id
            });
            context.SaveChanges();
            return new JsonResult(ActionStatusMessage.Success);
        }
        /// <summary>
        /// 查询病毒类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Info([FromBody] VirusTypeQueryDataModel model)
        {
            var list = context.VirusTracesDb;
            var id = model.Id?.Value;
            if (id != null) return new JsonResult(new EntityViewModel<VirusTypeDataModel>(list.FirstOrDefault(i => i.Id == id)?.ToModel(false)));
            var createStart = model.Create?.Start;
            var createEnd = model.Create?.End;
            if (createStart != null && createEnd != null && createStart > DateTime.MinValue && createEnd > DateTime.MinValue)
                list = list.Where(i => i.Create >= createStart && i.Create <= createEnd);
            var type = model.Type?.Value;
            if (type != null) list = list.Where(i => i.Type.Contains(type));
            var description = model.Description?.Value;
            if (description != null) list = list.Where(i => i.Description.Contains(description));
            var result = list.OrderBy(i =>i.Create).SplitPage(model.Pages);
            return new JsonResult(new EntitiesListViewModel<VirusTypeDataModel>(result.Item1.Select(i => i.ToModel(false)), result.Item2));
        }

    }
    public partial class ClientVirusTypeController
    {
        /// <summary>
        /// 指定关联类型
        /// </summary>
        /// <param name="virusId"></param>
        /// <param name="virusTypeId"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Relate(Guid virusId,Guid virusTypeId) {
            if (virusId == Guid.Empty || virusTypeId == Guid.Empty) return new JsonResult(ActionStatusMessage.StaticMessage.IdIsNull);
            var virus = context.VirusesDb.FirstOrDefault(i => i.Id == virusId);
            if (virus == null) return new JsonResult(virus.NotExist());
            var virusType = context.VirusTracesDb.FirstOrDefault(i => i.Id == virusTypeId);
            if (virusType == null) return new JsonResult(virusType.NotExist());
            clientVirusServices.RelateVirusTrace(virus, virusType);
            context.SaveChanges();
            return new JsonResult(ActionStatusMessage.Success);
        }
    }
    public partial class ClientVirusTypeController
    {
        /// <summary>
        /// 获取指定目标的类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RelateByName(string type,int pageIndex=0,int pageSize=20) {
            if (type.IsNullOrEmpty()) type = "";
            type = type.ToLower();
            var exp = PredicateBuilder.New<VirusTrace>(false);
            exp = exp.Or(i => i.Type.Contains(type));
            exp = exp.Or(i => i.Alias.Contains(type));
            var list = context.VirusTracesDb.Where(exp);
            var r = list.OrderByDescending(i=>i.WarningLevel).ThenByDescending(i => i.Create).SplitPage(pageIndex, pageSize);
            return new JsonResult(new EntitiesListViewModel<VirusTypeDataModel>(r.Item1.Select(i => i.ToModel(false)), r.Item2));
        }
        /// <summary>
        /// 获取指定类型的详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RelateById(Guid id) {
            var item = context.VirusTracesDb.FirstOrDefault(i => i.Id == id);
            if (item == null) return new JsonResult(item.NotExist());
            return new JsonResult(new EntityViewModel<VirusTypeDataModel>(item.ToModel(false)));
        }
        /// <summary>
        /// 病毒的相关项
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RelateByVirusId(Guid id, int pageIndex = 0, int pageSize = 20)
        {
            //if (Guid.Empty == id) return new JsonResult(ActionStatusMessage.StaticMessage.IdIsNull);
            var virus = context.VirusesDb.FirstOrDefault(i => i.Id == id);
            if (virus == null) return new JsonResult(new Virus().NotExist());
            var trace = context.VirusTypeDispatchesDb.FirstOrDefault(i => i.Virus.Id == id);
            if (trace == null)
            {
                clientVirusServices.RelateVirusTrace(virus);
                context.SaveChanges();
                trace = context.VirusTypeDispatchesDb.FirstOrDefault(i => i.Virus.Id == id);
            }
            var virusTrace = trace?.VirusTrace;
            var result = virusTrace?.ToModel(trace?.IsAutoDispatch ?? false);
            return new JsonResult(new EntityViewModel<VirusTypeDataModel>(result));
        }
    }
}
