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
using TrainSchdule.Web.ViewModels.Company;

namespace TrainSchdule.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class CompanyController : ControllerBase
    {
	    private readonly ICompanyService _companyService;
	    private readonly ICurrentUserService _currentUserService;

	    public CompanyController(ICompanyService companyService, ICurrentUserService currentUserService)
	    {
		    _companyService = companyService;
		    _currentUserService = currentUserService;
	    }

	    [HttpGet]
	    [AllowAnonymous]
	    public IActionResult GetDic(string path)
	    {
		    if (path == null)
		    {
				return new JsonResult(new {code=404,message="无效的单位"});
		    }
		    var nowCompany = _companyService.Get(path);
		    if (nowCompany != null)
		    {
			    var list = _companyService.FindAllChild(nowCompany.id);
				return new JsonResult(new GetDicViewModel()
				{
					Child=list,
					Name = nowCompany.Name,
					Path = path
				});
		    }
		    else
		    {
			    return new JsonResult(new GetDicViewModel()
			    {
					Code = -1,
					Message = "无效的单位路径"
			    });
		    }

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
		    var anyExist = _companyService.Get($"{company.ParentPath}/{company.Name}");
		    if (anyExist == null)
		    {
			    var newCompanyDTO= await _companyService.CreateAsync(company.Name);
			    await _companyService.SetParentAsync(newCompanyDTO.Id,company.ParentPath);
			    return new JsonResult(ActionStatusMessage.Success);
		    }

		    return new JsonResult(ActionStatusMessage.Company_CreateExisted);
		    
	    }

	    private bool CheckPermissionCompany(string target)
	    {
		    var currentCompany = _currentUserService.CurrentUser.PermissionCompanies;
		    if (currentCompany!= null)
		    {
			    foreach (var permissionCompany in currentCompany)
			    {
				    if (target.Substring(0, permissionCompany.Path.Length) == permissionCompany.Path)
				    {
					    return true;
				    }
			    }
		    }

		    return false;

	    }
    }
}