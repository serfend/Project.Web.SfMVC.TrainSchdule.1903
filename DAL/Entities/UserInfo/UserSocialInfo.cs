namespace DAL.Entities.UserInfo
{
	public class UserSocialInfo : BaseEntityGuid
	{
		public string Phone { get; set; }
		public virtual Settle.Settle Settle { get; set; }
		public virtual AdminDivision Address { get; set; }
		public string AddressDetail { get; set; }

		/// <summary>
		/// SocialStatus【冗余字段 需要联动更新】
		/// </summary>
		public int Status { get; set; }
	}

	public enum SocialStatus
	{
		IsMarried = 1,
		IsApart = 2
	}
}