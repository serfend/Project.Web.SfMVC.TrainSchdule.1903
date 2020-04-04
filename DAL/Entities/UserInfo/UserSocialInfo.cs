namespace DAL.Entities.UserInfo
{
	public class UserSocialInfo : BaseEntity
	{
		public string Phone { get; set; }
		public virtual Settle.Settle Settle { get; set; }
		public virtual AdminDivision Address { get; set; }
		public string AddressDetail { get; set; }
	}
}