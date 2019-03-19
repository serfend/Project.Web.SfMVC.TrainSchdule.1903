using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.WEB.ViewModels.My;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrainSchdule.WEB.Controllers
{
	public class MyController : Controller
	{
		// GET: /<controller>/
		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public IActionResult Test()
		{
			var v = new ViewTestViewModel();
			v.Name = "233";
			return View(v);
		}
	}
}
