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
using TrainSchdule.WEB.Extensions;

namespace TrainSchdule.Web.Controllers
{
	[Authorize]
    [Route("[controller]/[action]")]
    public class CompanyController : ControllerBase
    {
	    private readonly ICompaniesService _companiesService;
	    private readonly ICurrentUserService _currentUserService;
	    private readonly IUsersService _usersService;

	    public CompanyController(ICompaniesService companiesService, ICurrentUserService currentUserService, IUsersService usersService)
	    {
		    _companiesService = companiesService;
		    _currentUserService = currentUserService;
		    _usersService = usersService;
	    }

	    [HttpGet]
	    public IActionResult AllMembers(string code,int page,int pageSize=20)
	    {
		    if (code == null)
		    {
			    var currentUser = _currentUserService.CurrentUser;
				if(currentUser==null)return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
				code = currentUser.Company.Code;
			}
			if (pageSize>20)return  new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
		    var cmp = _companiesService.Get(code);
			if(cmp==null)return new JsonResult(ActionStatusMessage.Company.NotExist);
			var users = _usersService.Find(u => u.Company.Code == code).Skip(page*pageSize).Take(pageSize).ToList();
			return new JsonResult(new CompanyMembersViewModel()
			{
				Data = new CompanyMembersDataModel()
				{
					List = users.Select(u=>u.ToCompanyMembersDataModel())
				}
			});

	    }

	    [HttpGet]
	    public IActionResult Child(string path,string showPrivate=null)
	    {
		    if (path == null)path = "Root";
		    bool canShowPrivate = showPrivate == "201700816";
		    var nowCompany = _companiesService.Get(path);
		    if (nowCompany != null)
		    {
			    var list = _companiesService.FindAllChild(nowCompany.Code).Where(item=> canShowPrivate || !item.IsPrivate);
				return new JsonResult(new GetDicViewModel()
				{
					Data = new GetDicDataModel() {
						Child = list,
						Name = nowCompany.Name,
						Path = path
					}
				});
		    }
		    else
		    {
			    return new JsonResult(ActionStatusMessage.Company.NotExist);
		    }

	    }

	    [HttpGet]
		public IActionResult Detail(string path)
	    {
		    if (path == null) path = _currentUserService.CurrentUser.Company.Code;
		    var cmp = _companiesService.Get(path);
			if(cmp==null)return new JsonResult(ActionStatusMessage.Company.NotExist);
			return new JsonResult(new CompanyDetailViewModel()
			{
				data=cmp
			});
	    }

		[HttpPost]
		public async Task<IActionResult> Create(CompanyViewModel company)
	    {
			if (!ModelState.IsValid) return new JsonResult(new Status(ActionStatusMessage.Fail.status, JsonConvert.SerializeObject(ModelState.AllModelStateErrors())));
			var currentUser = _currentUserService.CurrentUser;
			if (company.Code == null||!currentUser.Permission.Company.单位信息.Create.Check(company.Code))
				return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
		    var anyExist = _companiesService.Get(company.Code);
		    if (anyExist == null)
		    {
			    var newCompanyDTO= await _companiesService.CreateAsync(company.Name,company.Code);
			    try
			    {
				    await _companiesService.EditAsync(newCompanyDTO.Code, item =>
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
		 
    }
}