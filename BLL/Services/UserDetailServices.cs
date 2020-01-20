using BLL.Extensions;
using BLL.Helpers;
using DAL.DTO.User;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using DAL.Entities.UserInfo.Settle;
using DAL.Entities.Vocations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
	public partial class UsersService
	{
		/// <summary>
		/// 检查是否有权限操作用户
		/// </summary>
		/// <param name="authUser"></param>
		/// <param name="modefyUser"></param>
		/// <param name="requireAuthRank"></param>
		/// <returns>当无权限时返回-1，否则返回当前授权用户可操作单位与目标用户的级别差</returns>
		public int CheckAuthorizedToUser(User authUser, User modefyUser)
		{
			var myManages = InMyManage(authUser, out var totalCount)?.ToList();
			if (myManages == null || myManages.Count == 0) return -1;
			if (modefyUser == null) return 1;
			var targetCompany = modefyUser.CompanyInfo.Company.Code;
			// 判断是否有管理此单位的权限，并且级别高于此单位至少1级
			return myManages.Max(m => targetCompany.StartsWith(m.Code) ? targetCompany.Length - m.Code.Length : -1);
		}
		public IEnumerable<Company> InMyManage(User user, out int totalCount)
		{
			totalCount = 0;
			var list = new List<Company>();

			if (user == null || user.CompanyInfo?.Company == null) return list;
			list = _context.CompanyManagers.Where(m => m.User.Id == user.Id).Select(m => m.Company).ToList();
			// 所在单位的主管拥有此单位的管理权
			var companyCode = user.CompanyInfo.Company.Code;

			if (user.CompanyInfo.Duties.IsMajorManager && list.All(c => c.Code != companyCode)) list.Add(user.CompanyInfo.Company);
			totalCount = list == null ? 0 : list.Count;
			return list;
		}
		/// <summary>
		/// 获取全年休假天数同时，更新休假天数
		/// </summary>
		/// <param name="targetUser"></param>
		/// <returns></returns>
		public UserVocationInfoVDto VocationInfo(User targetUser)
		{
			if (targetUser == null) return null;
			var applies = _context.Applies.Where<Apply>(a => a.BaseInfo.From.Id == targetUser.Id && a.Status == DAL.Entities.ApplyInfo.AuditStatus.Accept && a.Create.Value.Year == DateTime.Now.AddDays(5).Year && a.RequestInfo.VocationType == "正休").ToList();//仅正休计算天数
			int nowLength = 0;
			int nowTimes = 0;
			int onTripTime = 0;
			var yearlyLength = targetUser.SocialInfo.Settle.GetYearlyLength(targetUser, out var lastModefy, out var newModefy, out var maxOnTripTime, out var description);
			if (yearlyLength < 0) yearlyLength = 0;
			if (lastModefy == null || (yearlyLength != lastModefy?.Length && lastModefy.UpdateDate != newModefy.UpdateDate) || !targetUser.SocialInfo.Settle.PrevYealyLengthHistory.Any(p => p.UpdateDate.Year == DateTime.Now.AddDays(5).Year))
			{
				var list = new List<VacationModefyRecord>(targetUser.SocialInfo.Settle.PrevYealyLengthHistory);
				list.Add(newModefy);
				targetUser.SocialInfo.Settle.PrevYealyLengthHistory = list;
				_context.AUserSocialInfoSettles.Update(targetUser.SocialInfo.Settle);
				_context.SaveChanges();
			}
			var userAdditions = new List<VocationAdditional>();
			var f = applies.All<DAL.Entities.ApplyInfo.Apply>(a =>
			{
				nowLength += a.RequestInfo.VocationLength;
				if (a.RequestInfo.OnTripLength > 0) onTripTime++;
				nowTimes++;
				userAdditions.AddRange(a.RequestInfo.AdditialVocations);
				if (a.RecallId != null)
				{
					if (a.RequestInfo.OnTripLength > 0) onTripTime--;
					var order = _context.RecallOrders.Find(a.RecallId);
					if (order == null) throw new ActionStatusMessageException(ActionStatusMessage.Apply.Recall.IdRecordButNoData);
					nowLength -= a.RequestInfo.StampReturn.Value.Subtract(order.ReturnStramp).Days;
				}
				return true;
			});
			var vocationInfo = new UserVocationInfoVDto()
			{
				LeftLength = (int)Math.Ceiling(yearlyLength - nowLength),
				MaxTripTimes = maxOnTripTime,
				NowTimes = nowTimes,
				OnTripTimes = onTripTime,
				YearlyLength = (int)Math.Ceiling(yearlyLength),
				Description = description,
				Additionals = userAdditions
			};
			return vocationInfo;
		}
	}
}
