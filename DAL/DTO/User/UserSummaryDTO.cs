namespace DAL.DTO.User
{
	/// <summary>
	/// 仅提供基本信息
	/// </summary>
	public class UserSummaryDto
	{
		public string Id { get; set; }
		public string RealName { get; set; }
		public string Duties { get; set; }
		public string Company { get; set; }
	}
}
