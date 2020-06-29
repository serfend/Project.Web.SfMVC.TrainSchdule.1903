using BLL.Extensions;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.DTO.User;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using DAL.Entities.UserInfo.Settle;
using DAL.Entities.Vacations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
			var result = InMyManage(authUser).Result;
			var myManages = result.Item1.ToList();
			if (result.Item2 == 0) return -1;
			if (modefyUser == null) return 1;
			var targetCompany = modefyUser.CompanyInfo.Company.Code;
			// 判断是否有管理此单位的权限，并且级别高于此单位至少1级
			return myManages.Max(m => targetCompany.StartsWith(m.Code) ? targetCompany.Length - m.Code.Length : -1);
		}

		public async Task<Tuple<IEnumerable<Company>, int>> InMyManage(User user)
		{
			var result = await Task.Run(() =>
			 {
				 int totalCount = 0;
				 var list = new List<Company>();
				 if (user?.CompanyInfo?.Company != null)
				 {
					 list = _context.CompanyManagers.Where(m => m.User.Id == user.Id).Select(m => m.Company).ToList();
					 // 所在单位的主管拥有此单位的管理权
					 var companyCode = user.CompanyInfo.Company.Code;
					 if (user.CompanyInfo.Duties.IsMajorManager && list.All(c => c.Code != companyCode))
					 {
						 list.Add(user.CompanyInfo.Company);
					 }
					 totalCount = list.Count;
				 }
				 return new Tuple<IEnumerable<Company>, int>(list, totalCount);
			 }).ConfigureAwait(false);
			return result;
		}

		/// <summary>
		/// 获取全年休假天数同时，更新休假天数
		/// </summary>
		/// <param name="targetUser"></param>
		/// <returns></returns>
		public UserVacationInfoVDto VacationInfo(User targetUser)
		{
			if (targetUser == null) return null;
			var applies = _context.AppliesDb
				.Where(a => a.BaseInfo.From.Id == targetUser.Id)
				.Where(a => a.Status == AuditStatus.Accept)
				.Where(a => a.RequestInfo.StampLeave.Value.Year == DateTime.Now.AddDays(5).Year)
				.Where(a => a.RequestInfo.VacationType == "正休").ToList(); // 仅正休计算天数

			var r = targetUser.SocialInfo.Settle.GetYearlyLength(targetUser);
			var requireAddRecord = r.Item2; var maxOnTripTime = r.Item3; var description = r.Item4;
			var yearlyLength = r.Item1 < 0 ? 0 : r.Item1;
			if (requireAddRecord != null)
			{
				var list = new List<AppUsersSettleModefyRecord>(targetUser.SocialInfo.Settle.PrevYealyLengthHistory ?? new List<AppUsersSettleModefyRecord>());
				list.Add(requireAddRecord);
				targetUser.SocialInfo.Settle.PrevYealyLengthHistory = list;
				_context.AUserSocialInfoSettles.Update(targetUser.SocialInfo.Settle);
				_context.SaveChanges();
			}
			var vacationInfo = VacationInfoInRange(applies, yearlyLength);
			vacationInfo.Description = $"{description} {vacationInfo.Description ?? ""}";
			vacationInfo.MaxTripTimes += maxOnTripTime;
			return vacationInfo;
		}

		private UserVacationInfoVDto VacationInfoInRange(IEnumerable<Apply> applies, double yearlyLength)
		{
			var userAdditions = new List<VacationAdditional>();
			var maxOnTripTimeGainForRecall = 0;//应召回而增加路途次数
			var maxOnTripTimeGainForRecallDescription = "";
			int nowLength = 0;
			int nowTimes = 0;
			int onTripTime = 0;
			var f = applies.All<DAL.Entities.ApplyInfo.Apply>(a =>
			{
				nowLength += a.RequestInfo.VacationLength;
				if (a.RequestInfo.OnTripLength > 0) onTripTime++;
				nowTimes++;
				userAdditions.AddRange(a.RequestInfo.AdditialVacations);

				//处理被召回的假期
				if (a.RecallId != null)
				{
					//不论用户是否休路途，均应该增加一次路途
					maxOnTripTimeGainForRecall++;
					var order = _context.RecallOrders.Find(a.RecallId);
					if (order == null) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Recall.IdRecordButNoData);
					//此处减去召回时间应注意是否在福利假内部
					var dayComsumeBeforeRecall = order.ReturnStramp.Subtract(a.RequestInfo.StampLeave.Value).Days;
					var containsLawVacations = _vacationCheckServices.GetVacationDates(a.RequestInfo.StampLeave.Value, dayComsumeBeforeRecall, true).ToList();
					var containsLawVacationsLength = containsLawVacations.Sum(v => v.Length);
					var realComsumeMainVacation = dayComsumeBeforeRecall - containsLawVacationsLength - a.RequestInfo.OnTripLength;
					if (realComsumeMainVacation > 0) nowLength -= realComsumeMainVacation;
				}
				return true;
			});
			if (maxOnTripTimeGainForRecall > 0) maxOnTripTimeGainForRecallDescription = $"因期间被召回{maxOnTripTimeGainForRecall}次，全年可休路途次数相应增加。";
			var vacationInfo = new UserVacationInfoVDto()
			{
				LeftLength = (int)Math.Floor(yearlyLength - nowLength),
				MaxTripTimes = maxOnTripTimeGainForRecall,
				NowTimes = nowTimes,
				OnTripTimes = onTripTime,
				YearlyLength = (int)Math.Floor(yearlyLength),
				Description = maxOnTripTimeGainForRecallDescription,
				Additionals = userAdditions
			};
			return vacationInfo;
		}

		public IEnumerable<AppUsersSettleModefyRecord> ModefyUserSettleModefyRecord(User user, Action<IEnumerable<AppUsersSettleModefyRecord>> Callback = null)
		{
			if (user == null) return null;
			var records = user.SocialInfo.Settle.PrevYealyLengthHistory;
			if (Callback != null)
			{
				Callback(records);
				user.SocialInfo.Settle.PrevYealyLengthHistory = records;
				_context.AUserSocialInfoSettles.Update(user.SocialInfo.Settle);
				_context.SaveChanges();
			}
			return records;
		}

		public AppUsersSettleModefyRecord ModefySettleModeyRecord(int code, Action<AppUsersSettleModefyRecord> Callback = null, bool isDelete = false)
		{
			var record = _context.AppUsersSettleModefyRecordDb.Where(r => r.Code == code).FirstOrDefault();
			if (Callback != null || isDelete)
			{
				Callback?.Invoke(record);
				if (isDelete)
				{
					record.Remove();
					var settlePre = _context.AUserSocialInfoSettles.Where(s => s.PrevYealyLengthHistory.Any(rec => rec.Code == code)).FirstOrDefault();
					settlePre.PrevYealyLengthHistory = settlePre.PrevYealyLengthHistory.Where(rec => rec.Code != code).ToList();
					_context.AUserSocialInfoSettles.Update(settlePre);
				}
				_context.AppUsersSettleModefyRecord.Update(record);
				_context.SaveChanges();
			}
			return record;
		}
	}
}