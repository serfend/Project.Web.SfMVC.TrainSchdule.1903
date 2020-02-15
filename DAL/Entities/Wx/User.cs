using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.Wx
{
	public class User:BaseEntity
	{
		public string NickName { get; set; }
		public string Avatar { get; set; }
	}
}
