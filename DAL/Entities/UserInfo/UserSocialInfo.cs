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
		Unmarried,
		Difference,
		Same,
	}
}
