using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

	   
		 
    }
}