﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.BLL.DTO;
using TrainSchdule.Web.ViewModels;

namespace TrainSchdule.ViewModels.User
{
	public class UserDetailViewModel:APIViewModel
	{
		public UserDetailsDTO data { get; set; }
	}
}