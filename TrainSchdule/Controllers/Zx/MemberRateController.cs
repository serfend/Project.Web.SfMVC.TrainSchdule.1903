﻿using Abp.Extensions;
using BLL.Extensions.Common;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.Common;
using BLL.Interfaces.File;
using DAL.Data;
using DAL.DTO.ZX.MemberRate;
using DAL.Entities;
using DAL.Entities.Common.DataDictionary;
using DAL.Entities.Permisstions;
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
        public MemberRateController(ApplicationDbContext context, ICurrentUserService currentUserService, IUserActionServices userActionServices, IUsersService usersService, IDataDictionariesServices dataDictionariesServices)
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
            if (c == null) throw new ActionStatusMessageException(ActionStatusMessage.CompanyMessage.NotExist);
            if (!userActionServices.Permission(currentUser, ApplicationPermissions.Grade.MemberRate.Detail.Item, PermissionType.Write, new List<string> { model.Company }, "批量授权录入", out var failCompany)) throw new ActionStatusMessageException(new GoogleAuthDataModel().PermitDenied($"无权录入{c.Name}的数据"));
            // convert to data
            var notExistUser = new List<string>();
            var authList = new HashSet<string>();
            var list = data.Select(i =>
            {
                var f = i.ToModel(context.CompaniesDb, context.AppUsersDb);
                if (f.User == null) notExistUser.Add(i.UserCid);
                return f;
            }).ToList();
            var umax = model.Company; // 全局指定的单位
            var authCompany = new List<string>()
            {
                umax
            };
            foreach (var i in list)
            {
                i.RatingCycleCount = model.RatingCycleCount;
                i.RatingType = model.RatingType;
                if (i.CompanyCode == null) i.CompanyCode = umax; // 默认选中
                else authCompany.Add(i.CompanyCode);
                var userCompany = i.User.CompanyInfo.CompanyCode;
                if (userCompany != null) authCompany.Add(userCompany);
            }
            authCompany = authCompany.Distinct().ToList();
            if (!userActionServices.Permission(currentUser, ApplicationPermissions.Grade.MemberRate.Detail.Item, PermissionType.Write, authCompany, "单点授权录入",out var authFailCompany)) throw new ActionStatusMessageException(new GoogleAuthDataModel().PermitDenied($"授权到[{list.FirstOrDefault(c => c.CompanyCode == failCompany)?.User.BaseInfo.RealName}]失败"));
            if (notExistUser.Any())
                return new JsonResult(new EntitiesListViewModel<string>(notExistUser));

            // check if exist
            var currentListRaw = context.NormalRates.ToExistQueryable()
                  .Where(i => i.RatingType == model.RatingType)
                  .Where(i => i.RatingCycleCount == model.RatingCycleCount)
                  .ToList();
            var currentList = currentListRaw
                  .Select(i => i.ToDataModel())
                  .ToList();
            if (currentList.Any())
            {
                if (model.Confirm)
                {
                    var companies = currentListRaw.Select(r => r.CompanyCode).Distinct();
                    var permit = userActionServices.Permission(currentUser, ApplicationPermissions.Grade.MemberRate.Detail.Item, PermissionType.Write, companies, "单点授权修改", out var currentFailCompany);
                    if (!permit) throw new ActionStatusMessageException(new GoogleAuthViewModel().Auth.PermitDenied());
                    currentListRaw.ForEach(v =>
                    {
                        v.Remove();
                    });
                    context.UpdateRange(currentListRaw);
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
            var list = context.NormalRates.ToExistQueryable();
            var currentUser = currentUserService.CurrentUser;

            var ratingCycleCount = model.RatingCycleCount?.Start;
            var ratingType = model.RatingType?.Start;
            if (ratingType != null && ratingCycleCount != null)
                list = list.Where(i => (int)i.RatingType == ratingType).Where(i => i.RatingCycleCount == ratingCycleCount);
            var user = model.User?.Value;
            if (user != null)
            {
                var userCompany = usersService.GetById(user)?.CompanyInfo?.CompanyCode;
                if (user != currentUser.Id)
                {
                    if (!userActionServices.Permission(currentUser, ApplicationPermissions.Grade.MemberRate.Detail.Item, PermissionType.Read, new List<string>() { userCompany }, $"查询{user}", out var failCompany)) throw new ActionStatusMessageException(new GoogleAuthDataModel().PermitDenied());
                }
                list = list.Where(i => i.UserId == user);
            }
            var company = model.Company?.Value;
            if (user == null && company == null)
                company = currentUser.CompanyInfo.CompanyCode;
            if (company != null)
            {
                if (!userActionServices.Permission(currentUser, ApplicationPermissions.Grade.MemberRate.Detail.Item, PermissionType.Read, new List<string>() { company }, "查询", out var companyFailCompany)) throw new ActionStatusMessageException(new GoogleAuthDataModel().PermitDenied());
                list = list.Where(i => i.CompanyCode.StartsWith(company));
            }
            list = list
                .OrderBy(i => i.RatingType)
                .ThenByDescending(i => i.RatingCycleCount)
                .ThenBy(i => i.Rank)
                .ThenByDescending(i => i.Level)
                .ThenByDescending(i => i.Create);
            var r = list.SplitPage(model.Page);
            return new JsonResult(new EntitiesListViewModel<MemberRateDataModel>(r.Item1.Select(i => i.ToDataModel()), r.Item2));
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
