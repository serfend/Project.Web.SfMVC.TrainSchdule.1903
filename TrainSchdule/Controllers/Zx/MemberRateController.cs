using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.File;
using DAL.Data;
using DAL.DTO.ZX.MemberRate;
using DAL.Entities.ZX.MemberRate;
using Magicodes.ExporterAndImporter.Core.Models;
using Magicodes.ExporterAndImporter.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.Extensions;
using TrainSchdule.ViewModels;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.ZX;

namespace TrainSchdule.Controllers.Zx
{
    /// <summary>
    /// 成员评分
    /// </summary>
    [Route("[controller]/[action]")]
    [Authorize]
    public partial class MemberRateController:Controller
    {
        private readonly ApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IFileServices fileServices;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="currentUserService"></param>
        /// <param name="fileServices"></param>
        public MemberRateController(ApplicationDbContext context,ICurrentUserService currentUserService,IFileServices fileServices)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.fileServices = fileServices;
        }
    }
    public partial class MemberRateController
    {
        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> XlsUpload(MemberRateXlsDto model)
        {
            if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState)) ;
            var currentUser = currentUserService.CurrentUser;
            var currentCompany = currentUser.CompanyInfo.Company;
            var importer = new ExcelImporter();
            ImportResult<MemberRateImportDto> data;
            using (var inputStream = model.File.OpenReadStream())
            {
                data = await importer.ImportWithErrorCheck<MemberRateImportDto>(inputStream);
            }
            var currentList = context.NormalRates
                    .Where(i => i.RatingType == model.RatingType)
                    .Where(i => i.RatingCycleCount == model.RatingCycleCount)
                    .Select(i=>i.ToDataModel())
                    .ToList();
            if (currentList.Any() && !model.Confirm) return new JsonResult(new EntitiesListViewModel<MemberRateDataModel>(currentList));
            var list = data.Data.Select(i => i.ToModel(context.CompaniesDb,context.AppUsersDb));
            await context.NormalRates.AddRangeAsync(list);
            await context.SaveChangesAsync();
            return new JsonResult(ActionStatusMessage.Success);
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
}
