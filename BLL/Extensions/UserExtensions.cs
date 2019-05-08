using DAL.DTO.User;
using DAL.Entities.UserInfo;

namespace BLL.Extensions
{
	public static class UserExtensions
	{
		public static UserSummaryDto ToDTO(this User user)
		{
			if (user == null) return null;
			var b=new UserSummaryDto()
			{
				Company = user.CompanyInfo.Company.Name,
				Duties = user.CompanyInfo.Duties.Name,
				Id = user.Id,
				RealName = user.BaseInfo.RealName
			};
			return b;
		}


	}
}
