using DAL.Entities.UserInfo;

namespace DAL.DTO.Apply
{
	public class ApplyBaseInfoVdto
	{
		public Entities.UserInfo.User From { get; set; }
		public string RealName { get; set; }
		public string Company { get; set; }
		public string Duties { get; set; }
		public int HomeAddress { get; set; }
		public string HomeDetailAddress { get; set; }
		public string Phone { get; set; }
		public SettleDownEnum Settle { get; set; }
	}
}
