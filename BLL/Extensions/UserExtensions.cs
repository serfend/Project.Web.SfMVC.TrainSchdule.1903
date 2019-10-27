using DAL.DTO.User;
using DAL.Entities.UserInfo;

namespace BLL.Extensions
{
	public static class UserExtensions
	{
		public static UserSummaryDto ToDto(this User user)
		{
			if (user == null) return null;
			var b=new UserSummaryDto()
			{
				Company = user.CompanyInfo.Company?.Name??"无单位",
				Duties = user.CompanyInfo.Duties?.Name??"无职务",
				Id = user.Id,
				RealName = user.BaseInfo.RealName??"无姓名"
			};
			return b;
		}


	}
}
