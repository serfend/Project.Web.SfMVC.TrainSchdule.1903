using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using TrainSchdule.ViewModels.Static;

namespace TrainSchdule.ViewModels.Apply
{
	public class ApplySubmitViewModel:VerifyViewModel
	{
		public ApplySubmitData Param { get; set; }

		public bool NotAutoStart { get; set; }
	}

	public class ApplySubmitData
	{
		/// <summary>
		/// 需要提交申请的用户名，默认为当前登录的用户
		/// </summary>
		public string UserName { get; set; }
		[DisplayName("request")]
		public ApplyRequest Request { get; set; }

		public  string Reason { get; set; }
		public string xjlb { get; set; }
		[DisplayName("stamp")]
		public ApplyStamp Stamp { get; set; }
		[DisplayName("to")]
		public IEnumerable<int> To { get; set; }
	}
}
