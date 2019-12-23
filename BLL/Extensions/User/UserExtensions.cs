using BLL.Interfaces;
using DAL.DTO.User;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Hosting;
using Remotion.Linq.Parsing.ExpressionVisitors.MemberBindings;

namespace BLL.Extensions
{
	public static class UserExtensions
	{
	
		public static UserSummaryDto ToSummaryDto(this User user, IHostingEnvironment env)
		{
			if (user == null) return null;
			var b=new UserSummaryDto()
			{
				About = user.DiyInfo?.About??"无简介",
				Avatar = user.DiyInfo?.Avatar?.Id.ToString(),
				CompanyCode = user.CompanyInfo?.Company?.Code,
				DutiesCode = user.CompanyInfo?.Duties?.Code,
				CompanyName = user.CompanyInfo?.Company?.Name?? "无单位",
				DutiesName = user.CompanyInfo?.Duties?.Name??"无职务",
				Gender = user.BaseInfo.Gender,
				RealName = user.BaseInfo?.RealName??"无姓名",
				Id = user.Id,
				IsInitPassword=user.BaseInfo.PasswordModefy,
				InviteBy=user.Application?.InvitedBy
			};
			return b;
		}


	}
}
