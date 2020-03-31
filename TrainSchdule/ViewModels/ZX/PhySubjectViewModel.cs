﻿using BLL.Helpers;
using DAL.Entities.ZX.Phy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.ZX
{
	/// <summary>
	/// 成绩查询
	/// </summary>
	public class PhySubjectViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public Subject Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class PhySubjectDataModel : GoogleAuthViewModel
	{
		/// <summary>
		///
		/// </summary>
		public Subject Subject { get; set; }
	}
}