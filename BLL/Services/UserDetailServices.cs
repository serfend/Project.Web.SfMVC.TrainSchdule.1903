using BLL.Extensions;
using BLL.Helpers;
using DAL.DTO.User;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using DAL.Entities.Vocations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
	public partial class UsersService
	{
		public IEnumerable<Company> InMyManage(User user,out int totalCount)
		{
			totalCount = 0;
			var list = new List<Company>();

			if (user == null) return list;
			list = _context.CompanyManagers.Where(m => m.User.Id == user.Id).Select(m => m.Company).ToList();
			// 所在单位的主管拥有此单位的管理权
			var companyCode = user.CompanyInfo.Company.Code;

			if (user.CompanyInfo.Duties.IsMajorManager && list.All(c => c.Code != companyCode)) list.Add(user.CompanyInfo.Company);
			totalCount = list==null?0:list.Count;
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
			var applies = _context.Applies.Where<Apply>(a => a.BaseInfo.From.Id == targetUser.Id && a.Status == DAL.Entities.ApplyInfo.AuditStatus.Accept&&a.Create.Value.Year==DateTime.Now.Year&&a.RequestInfo.VocationType=="正休").ToList();//仅正休计算天数
			int nowLength = 0;
			int nowTimes = 0;
			int onTripTime = 0;
			int yearlyLength = targetUser.SocialInfo.Settle.GetYearlyLength(targetUser, out var maxOnTripTime, out var description);
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
				LeftLength = yearlyLength - nowLength,
				MaxTripTimes = maxOnTripTime,
				NowTimes = nowTimes,
				OnTripTimes = onTripTime,
				YearlyLength = yearlyLength,
				Description = description,
				Additionals=userAdditions
			};
			return vocationInfo;
		}
	}
}
