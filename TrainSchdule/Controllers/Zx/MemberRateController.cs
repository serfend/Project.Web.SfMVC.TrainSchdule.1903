using Abp.Extensions;
using BLL.Extensions.Common;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.Common;
using BLL.Interfaces.File;
using DAL.Data;
using DAL.DTO.ZX.MemberRate;
using DAL.Entities;
using DAL.Entities.Common.DataDictionary;
using DAL.Entities.ZX.MemberRate;
using Magicodes.ExporterAndImporter.Core.Models;
using Magicodes.ExporterAndImporter.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TrainSchdule.Extensions;
using TrainSchdule.ViewModels;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;
using TrainSchdule.ViewModels.ZX;

namespace TrainSchdule.Controllers.Zx
{
    /// <summary>
    /// 成员评分
    /// </summary>
    [Route("[controller]/[action]")]
    [Authorize]
    public partial class MemberRateController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IUserActionServices userActionServices;
        private readonly IUsersService usersService;
        private readonly IDataDictionariesServices dataDictionariesServices;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="currentUserService"></param>
        /// <param name="userActionServices"></param>
        /// <param name="usersService"></param>
        /// <param name="dataDictionariesServices"></param>
        public MemberRateController(ApplicationDbContext context, ICurrentUserService currentUserService, IUserActionServices userActionServices,IUsersService usersService, IDataDictionariesServices dataDictionariesServices)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.userActionServices = userActionServices;
            this.usersService = usersService;
            this.dataDictionariesServices = dataDictionariesServices;
        }
    }
    public partial class MemberRateController
    {
        private const string cacheMemberRate = "cache.memberRate.upload";
        private const string cacheMemberRateXlsModel = "cache.memberRate.upload.model";
        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> XlsUpload(MemberRateXlsDto model)
        {
            if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
            var importer = new ExcelImporter();
            ImportResult<MemberRateImportDto> data;
            HttpContext.Session.Remove(cacheMemberRate);
            HttpContext.Session.Remove(cacheMemberRateXlsModel);
            using (var inputStream = model.File.OpenReadStream())
            {
                data = await importer.ImportWithErrorCheck<MemberRateImportDto>(inputStream);
            }
            return await CheckData(data.Data.ToList(), model) ?? new JsonResult(ActionStatusMessage.Success);
        }
        /// <summary>
        /// 检查数据并保存
        /// </summary>
        /// <param name="data"></param>
        /// <param name="model"></param>
        private async Task<IActionResult> CheckData(List<MemberRateImportDto> data, MemberRateXlsDto model)
        {
            var currentUser = currentUserService.CurrentUser;
            var c = context.CompaniesDb.FirstOrDefault(i => i.Code == model.Company);
            if (c==null) throw new ActionStatusMessageException(ActionStatusMessage.CompanyMessage.NotExist);
            if(!userActionServices.Permission(currentUser.Application.Permission, DictionaryAllPermission.Grade.MemberRate, Operation.Update, currentUser.Id, model.Company, "批量授权录入")) throw new ActionStatusMessageException(new GoogleAuthDataModel().PermitDenied());
            // convert to data
            var notExistUser = new List<string>();
            var authList = new HashSet<string>();
            var list = data.Select(i =>
            {
                var f = i.ToModel(context.CompaniesDb, context.AppUsersDb);
                if (f.User == null) notExistUser.Add(i.UserCid);
                return f;
            }).Select(i =>
            {
                i.RatingCycleCount = model.RatingCycleCount;
                i.RatingType = model.RatingType;
                var umax = model.Company; // 全局指定的单位
                if(i.CompanyCode==null) i.CompanyCode = umax; // 默认选中
                var ccode = i.CompanyCode; // 表格中指定的单位
                umax = ccode == null ? umax : (ccode.StartsWith(umax) ? umax : ccode); // 取高权限
                var ucode = i.User.CompanyInfo.CompanyCode; // 用户的单位
                umax = ucode==null ? umax : (ucode.StartsWith(umax) ? umax : ucode); // 取高权限
                if (authList.Contains(umax)) { 
                    authList.Add(umax);
                    if(!userActionServices.Permission(currentUser.Application.Permission, DictionaryAllPermission.Grade.MemberRate, Operation.Update, currentUser.Id, umax, "单点授权录入")) throw new ActionStatusMessageException(new GoogleAuthDataModel().PermitDenied());
                }
                return i;
            }).ToList();
            if (notExistUser.Any())
                return new JsonResult(new EntitiesListViewModel<string>(notExistUser));

            // check if exist
            var currentListRaw = context.NormalRates.ToExistDbSet()
                  .Where(i => i.RatingType == model.RatingType)
                  .Where(i => i.RatingCycleCount == model.RatingCycleCount);
            var currentList = currentListRaw
                  .Select(i => i.ToDataModel())
                  .ToList();
            if (currentList.Any())
            {
                if (model.Confirm)
                {
                    foreach (var r in currentListRaw) r.Remove();
                }
                else
                {
                    HttpContext.Session.Set(cacheMemberRate, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data)));
                    model.File = null;
                    HttpContext.Session.Set(cacheMemberRateXlsModel, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(model)));
                    return new JsonResult(new EntitiesListViewModel<MemberRateDataModel>(currentList));
                }

            }
            // save
            await context.NormalRates.AddRangeAsync(list);
            await context.SaveChangesAsync();
            return null;
        }
        /// <summary>
        /// 确认上次的上传
        /// </summary>
        /// <returns></returns>
        [HttpPatch]
        public async Task<IActionResult> ConfirmLastXlsUpload()
        {
            if (!HttpContext.Session.TryGetValue(cacheMemberRate, out var data)) return new JsonResult(ActionStatusMessage.StaticMessage.ResourceNotExist);
            var _ = HttpContext.Session.TryGetValue(cacheMemberRateXlsModel, out var model);
            var datas = JsonSerializer.Deserialize<List<MemberRateImportDto>>(Encoding.UTF8.GetString(data));
            var models = JsonSerializer.Deserialize<MemberRateXlsDto>(Encoding.UTF8.GetString(model));
            models.Confirm = true;
            var result = await CheckData(datas, models) ?? new JsonResult(ActionStatusMessage.Success);
            return result;
        }
        private const string TemplateName = "评分导入模板.xlsx";
        /// <summary>
        /// 获取模板
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> XlsTemplate()
        {
            var importer = new ExcelImporter();
            var content = await importer.GenerateTemplateBytes<MemberRateImportDto>();
            new FileExtensionContentTypeProvider().TryGetContentType(TemplateName, out var contentType);
            return File(content, contentType ?? "text/plain", TemplateName);
        }
    }


    public partial class MemberRateController
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Info([FromBody] MemberRateQueryModel model)
        {
            if (!ModelState.IsValid) return new JsonResult(ModelState.ToModel());
            var list = context.NormalRates.ToExistDbSet();
            var currentUser = currentUserService.CurrentUser;
            
            var ratingCycleCount = model.RatingCycleCount?.Start;
            var ratingType = model.RatingType?.Start;
            if(ratingType != null && ratingCycleCount!=null)
                list = list.Where(i => (int)i.RatingType == ratingType).Where(i=>i.RatingCycleCount==ratingCycleCount);
            var user = model.User?.Value;
            if (user != null)
            {
                var userCompany = usersService.GetById(user)?.CompanyInfo?.CompanyCode;
                if (user != currentUser.Id) {
                    if(!userActionServices.Permission(currentUser.Application.Permission, DictionaryAllPermission.Grade.MemberRate, Operation.Query, currentUser.Id, userCompany, $"查询{user}")) throw new ActionStatusMessageException(new GoogleAuthDataModel().PermitDenied());
                }
                list = list.Where(i => i.UserId == user);
            }
            var company = model.Company?.Value;
            if (user == null && company == null)
                company = currentUser.CompanyInfo.CompanyCode;
            if (company != null)  {
                userActionServices.Permission(currentUser.Application.Permission, DictionaryAllPermission.Grade.MemberRate, Operation.Query, currentUser.Id, company);
                list = list.Where(i => i.CompanyCode.StartsWith(company));
            }
            list = list
                .OrderBy(i=>i.RatingType)
                .ThenByDescending(i=>i.RatingCycleCount)
                .ThenBy(i=>i.Rank)
                .ThenByDescending(i => i.Level)
                .ThenByDescending(i => i.Create);
            var r = list.SplitPage(model.Page);
            return new JsonResult(new EntitiesListViewModel<MemberRateDataModel>(r.Item1.Select(i=>i.ToDataModel()),r.Item2));
        }
    }

    public partial class MemberRateController
    {
        /// <summary>
        /// 获取状态列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult StatusDict()
        {
            var dict = dataDictionariesServices.GetByGroupName(ApplicationDbContext.normarRateLevel);
            return new JsonResult(new EntityViewModel<Dictionary<string, CommonDataDictionary>>(new Dictionary<string, CommonDataDictionary>(dict.Select(s => new KeyValuePair<string, CommonDataDictionary>(s.Value.ToString(), s)))));
        }
    }
}
