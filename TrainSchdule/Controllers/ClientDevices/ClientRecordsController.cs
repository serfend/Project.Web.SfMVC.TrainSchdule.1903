using Abp.Extensions;
using BLL.Extensions.Common;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.Common;
using DAL.Data;
using DAL.Entities.ClientDevice;
using DAL.Entities.Common.DataDictionary;
using DAL.Entities.Permisstions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.ClientDevice;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.Controllers.ClientDevices
{

    /// <summary>
    /// 终端日志记录
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public partial class ClientRecordsController:Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IDataDictionariesServices dataDictionariesServices;
        private readonly IUserActionServices userActionServices;
        private readonly ICurrentUserService currentUserService;

        /// <summary>
        /// 
        /// </summary>
        public ClientRecordsController(ApplicationDbContext context, IDataDictionariesServices dataDictionariesServices,IUserActionServices userActionServices,ICurrentUserService currentUserService)
        {
            this.context = context;
            this.dataDictionariesServices = dataDictionariesServices;
            this.userActionServices = userActionServices;
            this.currentUserService = currentUserService;
        }
    }

    public partial class ClientRecordsController 
    {
        /// <summary>
        /// 处置记录编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Info([FromBody] VirusHandleRecordDataModel model)
        {
            var r = context.VirusHandleRecords.FirstOrDefault(i => i.Id == model.Id);
            var client = r ?? new VirusHandleRecord();
            var prev_status = client.HandleStatus;
            model.ToModel(context.Viruses, client);
            if (client.Virus == null) return new JsonResult(client.Virus.NotExist());
            if (!client.Virus.Company.IsNullOrEmpty())
                if (!userActionServices.Permission(currentUserService.CurrentUser, ApplicationPermissions.Client.Virus.Info.Item, PermissionType.Write, client.Virus.Company, "处置记录"))
                    throw new ActionStatusMessageException(new ApiResult(new GoogleAuthDataModel().PermitDenied(), $"授权到{client.Virus.Company}", true));
            if (client.IsRemoved && r != null)
            {
                r.Remove();
                context.VirusHandleRecords.Update(r);
            }
            else
            {
                if(prev_status==client.HandleStatus) return new JsonResult(ActionStatusMessage.Success);
                var lastest = context.VirusHandleRecordsDb.Where(i => i.VirusKey == client.VirusKey).OrderByDescending(i => i.Create).FirstOrDefault();
                if (lastest==null||lastest.Create <= client.Create)
                {
                    if (client.HandleStatus.IsSuccess())
                    {
                        client.Virus.HandleDate = DateTime.Now;
                        if (client.Virus.Status.HasFlag(VirusStatus.Unhandle)) client.Virus.Status -= VirusStatus.Unhandle;
                        client.Virus.Status |= VirusStatus.Success;
                    }
                    else
                    {
                        switch (client.HandleStatus)
                        {
                            case VirusHandleStatus.ClientDeviceVirusMessage:
                                {
                                    client.Virus.Status |= VirusStatus.MessageSend;
                                    break;
                                }
                            case VirusHandleStatus.ClientDeviceVirusNotify:
                                {
                                    client.Virus.Status |= VirusStatus.ClientNotify;
                                    break;
                                }
                            case VirusHandleStatus.ClientDeviceVirusNewFail:
                            case VirusHandleStatus.ClientDeviceVirusNewUnhandle:
                                {
                                    if (client.Virus.Status.HasFlag(VirusStatus.Success)) client.Virus.Status -= VirusStatus.Success;
                                    client.Virus.Status |= VirusStatus.Unhandle;
                                    break;
                                }
                        }
                    }
                }
                if (r == null) context.VirusHandleRecords.Add(client);
                else context.VirusHandleRecords.Update(client);
                context.Viruses.Update(client.Virus);
            }
            context.SaveChanges();
            return new JsonResult(ActionStatusMessage.Success);
        }


        /// <summary>
        /// 查询处置记录 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Info([FromBody] VirusHandleRecordQueryDataModel model)
        {
            var list = context.VirusHandleRecordsDb.AsQueryable();
            var createStart = model.Create?.Start;
            var createEnd = model.Create?.End;
            if (createStart != null && createEnd != null && createStart > DateTime.MinValue && createEnd > DateTime.MinValue)
                list = list.Where(i => i.Create >= createStart && i.Create <= createEnd);
            var virus = model.Virus?.Value;
            if (virus != null) list = list.Where(i => i.VirusKey == virus);
            var status = model.HandleStatus?.Arrays;
            if (status != null) list = list.Where(i => status.Contains((int)i.HandleStatus));
            var remark = model.Remark?.Value;
            if (remark != null) list = list.Where(i => i.Remark.Contains(remark));
            var result = list.OrderByDescending(i => i.Create).SplitPage(model.Pages);
            return new JsonResult(new EntitiesListViewModel<VirusHandleRecordDataModel>(result.Item1.Select(i => i.ToModel()), result.Item2));
        }
        /// <summary>
        /// 编辑备注
        /// </summary>
        /// <param name="model">id,remark</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Remark([FromBody] VirusHandleRecordDataModel model) {
            var record = context.VirusHandleRecordsDb.FirstOrDefault(i => i.Id == model.Id);
            if (record == null) return new JsonResult(record.NotExist());
            if (!record.Virus.Company.IsNullOrEmpty())
                if (!userActionServices.Permission(currentUserService.CurrentUser, ApplicationPermissions.Client.Virus.Info.Item, PermissionType.Write, record.Virus.Company, "病毒处置备注"))
                    throw new ActionStatusMessageException(new ApiResult(new GoogleAuthDataModel().PermitDenied(), $"授权到{record.Virus.Company}", true));
            record.Remark = model.Remark;
            context.VirusHandleRecords.Update(record);
            context.SaveChanges();
            return new JsonResult(ActionStatusMessage.Success);
        }
    }

   public partial class ClientRecordsController
    {
        /// <summary>
        /// 获取状态列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RecordStatusDict() {
            var dict = dataDictionariesServices.GetByGroupName(ApplicationDbContext.clientVirusHandleStatus);
            return new JsonResult(new EntityViewModel<Dictionary<string, CommonDataDictionary>>(new Dictionary<string, CommonDataDictionary>(dict.Select(s => new KeyValuePair<string, CommonDataDictionary>(s.Value.ToString(), s)))));
        }
    }
}
