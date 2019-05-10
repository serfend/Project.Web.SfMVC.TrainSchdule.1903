namespace DAL.Entities.UserInfo
{
	public class UserBaseInfo : BaseEntity
	{
		/// <summary>
		/// 居民身份证
		/// </summary>
		public string Cid { get; set; }
		public string RealName { get; set; }
		public string Avatar { get; set; }
		public GenderEnum Gender { get; set; }
		public bool PrivateAccount { get; set; }
	}
	public enum GenderEnum
	{
		Unknown = 0,
		Male = 1,
		Female = 2,

	}
}
