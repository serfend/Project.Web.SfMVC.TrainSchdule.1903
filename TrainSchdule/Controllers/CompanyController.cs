using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.BLL.Helpers;
using TrainSchdule.BLL.Interfaces;
using TrainSchdule.BLL.Services;
using TrainSchdule.ViewModels.Company;
using TrainSchdule.Web.ViewModels.Company;

namespace TrainSchdule.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class CompanyController : ControllerBase
    {
	    private readonly ICompaniesService _companieService;
	    private readonly ICurrentUserService _currentUserService;

	    public CompanyController(ICompaniesService companieService, ICurrentUserService currentUserService)
	    {
		    _companieService = companieService;
		    _currentUserService = currentUserService;
	    }

	    [HttpGet]
	    [AllowAnonymous]
	    public IActionResult Child(string path)
	    {
		    if (path == null)path = "Root";

		    var nowCompany = _companieService.Get(path);
		    if (nowCompany != null)
		    {
			    var list = _companieService.FindAllChild(nowCompany.id);
				return new JsonResult(new GetDicViewModel()
				{
					Child=list,
					Name = nowCompany.Name,
					Path = path
				});
		    }
		    else
		    {
			    return new JsonResult(ActionStatusMessage.Company_NotExist);
		    }

	    }

	    [HttpGet]
	    public IActionResult Detail(string path = null)
	    {
		    if (path == null) path = _currentUserService.CurrentUser.Company.Path;
		    var cmp = _companieService.Get(path);
			if(cmp==null)return new JsonResult(ActionStatusMessage.Company_NotExist);
			return new JsonResult(new CompanyDetailViewModel()
			{
				data=cmp
			});
	    }
		[HttpPost]
	    public async Task<IActionResult> Create(CompanyViewModel company)
	    {
		    if (!ModelState.IsValid)
		    {
			    var rst = new StringBuilder();
			    foreach (var item in ModelState.Root.Children)
			    {
				    foreach (var err in item.Errors)
				    {
					    rst.AppendLine(err.ErrorMessage);
				    }
			    }
				return new JsonResult(new Status(ActionStatusMessage.AccountLogin_InvalidAuthFormat.Code,rst.ToString()));
		    }
			if(!CheckPermissionCompany(company.ParentPath))
				return new JsonResult(ActionStatusMessage.AccountAuth_Forbidden);
		    var anyExist = _companieService.Get($"{company.ParentPath}/{company.Name}");
		    if (anyExist == null)
		    {
			    var newCompanyDTO= await _companieService.CreateAsync(company.Name);
			    try
			    {
				    await _companieService.SetParentAsync(newCompanyDTO.Id,company.ParentPath);
			    }
			    catch (Exception ex)
			    {
					return new JsonResult(new Status(ActionStatusMessage.Company_NotExist.Code,ex.Message));
			    }
			    return new JsonResult(ActionStatusMessage.Success);
		    }

		    return new JsonResult(ActionStatusMessage.Company_CreateExisted);
		    
	    }
		/// <summary>
		/// 检查当前用户是否具有操作对应路径单位的权限
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
	    public bool CheckPermissionCompany(string target)
		     => _currentUserService.CurrentUser.PermissionCompanies.Any((company)=>target.StartsWith(company.Path));
		 
    }
}