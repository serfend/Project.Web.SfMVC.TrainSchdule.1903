using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Entities.UserInfo
{
	public class UserCompanyTitle
	{
		[Key]
		public int Code { get; set; }
		public string Name { get; set; }
		public int Level { get; set; }
	}
}
