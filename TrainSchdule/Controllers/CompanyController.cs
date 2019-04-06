using System;
using System.Collections.Generic;
using System.Linq;
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
			    var list = _companyService.FindAllChild(path);
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
			
		    var currentCompany = _currentUserService.CurrentUser.Company.Path;
		    var newCompany = company.ParentPath;
		    if (newCompany.Substring(0, currentCompany.Length) == currentCompany)
		    {
			    var newCompanyDTO= await _companyService.CreateAsync(company.Name, company.ParentPath);
			    await _companyService.SetParentAsync(newCompanyDTO.Path,company.ParentPath);
				return new JsonResult(ActionStatusMessage.Success);
		    }
		    else
		    {
				return new JsonResult(ActionStatusMessage.AccountAuth_Forbidden);
		    }
	    }
    }
}