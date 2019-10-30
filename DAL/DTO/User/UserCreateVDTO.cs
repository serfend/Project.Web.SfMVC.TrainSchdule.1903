using DAL.Entities.UserInfo;
using DAL.Entities.UserInfo.Settle;

namespace DAL.DTO.User
{

	public class UserCreateVdto
	{
		public string Id { get; set; }
		public string Cid { get; set; }
		public string RealName { get; set; }
		public string Phone { get; set; }
		public GenderEnum Gender { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
		public int HomeAddress { get; set; }
		public string HomeDetailAddress { get; set; }
		public string Duties { get; set; }
		public string Company { get; set; }
		public string InvitedBy { get; set; }
		public Settle Settle { get; set; }
	}
}
