using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TrainSchdule.BLL.Helpers;
using TrainSchdule.BLL.Interfaces;
using TrainSchdule.BLL.Services;
using TrainSchdule.Extensions;
using TrainSchdule.ViewModels.Company;
using TrainSchdule.Web.ViewModels.Company;

namespace TrainSchdule.Web.Controllers
{
	[Authorize]
    [Route("[controller]/[action]")]
    public class CompanyController : ControllerBase
    {
	    private readonly ICompaniesService _companiesService;
	    private readonly ICurrentUserService _currentUserService;

	    public CompanyController(ICompaniesService companiesService, ICurrentUserService currentUserService)
	    {
		    _companiesService = companiesService;
		    _currentUserService = currentUserService;
	    }

	    [HttpGet]
	    [AllowAnonymous]
	    public IActionResult Child(string path,string showPrivate=null)
	    {
		    if (path == null)path = "Root";
		    bool canShowPrivate = showPrivate == "201700816";
		    var nowCompany = _companiesService.Get(path);
		    if (nowCompany != null)
		    {
			    var list = _companiesService.FindAllChild(nowCompany.id).Where(item=> canShowPrivate || !item.IsPrivate);
				return new JsonResult(new GetDicViewModel()
				{
					Child=list,
					Name = nowCompany.Name,
					Path = path
				});
		    }
		    else
		    {
			    return new JsonResult(ActionStatusMessage.Company.NotExist);
		    }

	    }

	    [HttpGet]
	    [AllowAnonymous]
		public IActionResult Detail(string path = null)
	    {
		    if (path == null) path = _currentUserService.CurrentUser.Company.Path;
		    var cmp = _companiesService.Get(path);
			if(cmp==null)return new JsonResult(ActionStatusMessage.Company.NotExist);
			return new JsonResult(new CompanyDetailViewModel()
			{
				data=cmp
			});
	    }

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Create(CompanyViewModel company)
	    {
			if (!ModelState.IsValid) return new JsonResult(new Status(ActionStatusMessage.Fail.status, JsonConvert.SerializeObject(ModelState.AllModelStateErrors())));
			if (!CheckPermissionCompany(company.ParentPath))
				return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
		    var anyExist = _companiesService.Get($"{company.ParentPath}/{company.Name}");
		    if (anyExist == null)
		    {
			    var newCompanyDTO= await _companiesService.CreateAsync(company.Name);
			    try
			    {
				    await _companiesService.SetParentAsync(newCompanyDTO.Id,company.ParentPath);
				    await _companiesService.EditAsync(newCompanyDTO.Path, item =>
					    {
						    item.IsPrivate = company.IsPrivate;
					    });
			    }
			    catch (Exception ex)
			    {
					return new JsonResult(new Status(ActionStatusMessage.Company.NotExist.status,ex.Message));
			    }
			    return new JsonResult(ActionStatusMessage.Success);
		    }

		    return new JsonResult(ActionStatusMessage.Company.CreateExisted);
		    
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