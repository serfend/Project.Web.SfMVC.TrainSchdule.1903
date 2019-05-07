using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TrainSchdule.Controllers
{
	public partial class ApplyController
	{
		[HttpGet]
		public IActionResult FromUser(string id)
		{
			id = id ?? _currentUserService.CurrentUser?.Id;
		}
	}
}
