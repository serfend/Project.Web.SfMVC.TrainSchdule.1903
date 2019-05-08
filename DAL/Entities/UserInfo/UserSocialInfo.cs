namespace DAL.Entities.UserInfo
{
	public class UserSocialInfo : BaseEntity
	{
		public string Phone { get; set; }
		public SettleDownEnum Settle { get; set; }
		public virtual AdminDivision Address { get; set; }
		public string AddressDetail { get; set; }

	}

	public enum SettleDownEnum
	{
		不符合随军=0,
		符合随军未随军同地=1,
		符合随军未随军异地=2,
		已随军=3,
		双军人同地=4,
		双军人异地=5
	}
}
