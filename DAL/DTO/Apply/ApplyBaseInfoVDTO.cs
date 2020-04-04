using DAL.Entities.UserInfo.Settle;

namespace DAL.DTO.Apply
{
	public class ApplyBaseInfoVdto
	{
		public Entities.UserInfo.User From { get; set; }
		public Entities.UserInfo.User CreateBy { get; set; }
		public string RealName { get; set; }
		public string Company { get; set; }
		public string Duties { get; set; }
		public string Phone { get; set; }
		public Settle Settle { get; set; }

		/// <summary>
		/// 休假去向
		/// </summary>
		public int VocationTargetAddress { get; set; }

		public string VocationTargetAddressDetail { get; set; }
	}
}