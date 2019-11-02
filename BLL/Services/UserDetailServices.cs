using BLL.Extensions;
using DAL.DTO.User;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
	public partial class UsersService
	{
		public IEnumerable<Company> InMyManage(string id)
		{
			var list = _context.CompanyManagers.Where(m => m.User.Id == id).Select(m=>m.Company);
			return list;
		}
		/// <summary>
		/// 获取全年休假天数同时，更新休假天数
		/// </summary>
		/// <param name="targetUser"></param>
		/// <returns></returns>
		public UserVocationInfoVDTO VocationInfo(User targetUser)
		{
			var applies = _context.Applies.Where<Apply>(a => a.BaseInfo.From.Id == targetUser.Id && a.Status == DAL.Entities.ApplyInfo.AuditStatus.Accept).ToList();
			int nowLength = 0;
			int nowTimes = 0;
			int onTripTime = 0;
			int yearlyLength = targetUser.SocialInfo.Settle.GetYearlyLength(out var maxOnTripTime);
			var f = applies.All<DAL.Entities.ApplyInfo.Apply>(a => {
				nowLength += a.RequestInfo.VocationLength;
				if (a.RequestInfo.OnTripLength > 0) onTripTime++;
				nowTimes++;
				return true;
			});
			var vocationInfo = new UserVocationInfoVDTO()
			{
				LeftLength = yearlyLength - nowLength,
				MaxTripTimes = maxOnTripTime,
				NowTimes = nowTimes,
				OnTripTimes = onTripTime,
				YearlyLength = yearlyLength
			};
			return vocationInfo;
		}
	}
}
