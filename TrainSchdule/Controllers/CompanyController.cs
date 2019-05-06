using System.Collections.Generic;
using System.Linq;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.Extensions;
using TrainSchdule.ViewModels.Company;

namespace TrainSchdule.Web.Controllers
{
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
	    public IActionResult Child(string id)
	    {
		  var  companys=_companiesService.FindAllChild(id);
		 
		  return new JsonResult(new AllChildViewModel()
		  {
			  Data = new AllChildDataModel()
			  {
				  List = companys.Select(c=>c.ToCompanyModel())
			  }
		  });
	    }
	   
		 
    }
}