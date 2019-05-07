using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;

namespace TrainSchdule.ViewModels.Apply
{
	public class SubmitBaseInfoViewModel
	{
		[Required]
		public string Id { get; set; }
		[Required]
		public string RealName { get; set; }
		[Required]
		public string Company { get; set; }
		[Required]
		public string Duties { get; set; }
		[Required]
		public int HomeAddress { get; set; }
		public string HomeDetailAddress { get; set; }

		public string Phone { get; set; }
		public SettleDownEnum Settle { get; set; }
	}



}
