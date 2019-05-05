using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities.UserInfo;

namespace DAL.DTO.User
{
	
	public class UserCreateDTO
	{
		public string Id { get; set; }
		public string RealName { get; set; }
		public string Phone { get; set; }
		public GenderEnum Gender { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
		public string HomeAddress { get; set; }
		public string HomeDetailAddress { get; set; }
		public string Duties { get; set; }
		public string Company { get; set; }
		public string InvitedBy { get; set; }
	}
}
