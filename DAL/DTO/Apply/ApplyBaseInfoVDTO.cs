using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities.UserInfo;

namespace DAL.DTO.Apply
{
	public class ApplyBaseInfoVDTO
	{
		public Entities.UserInfo.User From { get; set; }
		public string RealName { get; set; }
		public string Company { get; set; }
		public int Duties { get; set; }
		public int HomeAddress { get; set; }
		public string HomeDetailAddress { get; set; }
		public string Phone { get; set; }
		public SettleDownEnum Settle { get; set; }
	}
}
