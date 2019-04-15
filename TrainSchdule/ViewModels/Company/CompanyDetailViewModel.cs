using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.BLL.DTO;
using TrainSchdule.Web.ViewModels;

namespace TrainSchdule.ViewModels.Company
{
	public class CompanyDetailViewModel:APIViewModel
	{
		public CompanyDTO data { get; set; }
	}
}
