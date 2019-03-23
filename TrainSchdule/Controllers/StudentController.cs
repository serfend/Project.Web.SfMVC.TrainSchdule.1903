using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.BLL.DTO;
using TrainSchdule.BLL.Interfaces;
using TrainSchdule.Web.ViewModels.Student;
using TrainSchdule.WEB.ViewModels.Student;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrainSchdule.WEB.Controllers
{
	[Authorize]
	[Route("[controller]/[action]")]
	public class StudentController : Controller
	{

		private readonly IStudentService _studentService;
		private readonly IUsersService _usersService;
		private readonly IHostingEnvironment _environment;
		private readonly ICurrentUserService _currentUserService;

		public StudentController(ICurrentUserService currentUserService, IHostingEnvironment environment, IUsersService usersService, IStudentService studentService)
		{
			_currentUserService = currentUserService;
			_environment = environment;
			_usersService = usersService;
			_studentService = studentService;
		}

		// GET: /<controller>/
		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public IActionResult SingleTest()
		{
			var v = new ViewTestViewModel {Name = "233"};
			return new ObjectResult(v);
		}

		[HttpGet]
		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(StudentViewModel std)
		{
			_studentService.Create(new StudentDTO()
			{
				Alias = std.Alias,
				birth = std.Birth,
				Gender = std.Gender
			});
			return RedirectToAction(nameof(Detail),new {id=std.id});
		}

		[HttpGet]
		[AutoValidateAntiforgeryToken]
		public ActionResult Detail(Guid id)
		{
			var studentDto = _studentService.Get(id);
			if(studentDto==null)studentDto=new StudentDTO(){id = id};
			var std = new StudentViewModel()
			{
				id = studentDto.id,
				Age = (int)(studentDto.birth.Subtract(DateTime.Now).TotalDays / 365),
				Alias = studentDto.Alias
			};
			return View(std);
		}
	}
}
