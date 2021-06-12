using Abp.Extensions;
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
		/// <param name="modifyUser"></param>
		/// <param name="requireAuthRank"></param>
		/// <returns>当无权限时返回-1，否则返回当前授权用户可操作单位与目标用户的级别差</returns>
		public int CheckAuthorizedToUser(User authUser, User modifyUser)
		{
			if (authUser?.Id == "root") return int.MaxValue;
			var result = InMyManage(authUser).Result;
			var myManages = result.Item1.ToList();
			if (result.Item2 == 0) return -1;
			if (modifyUser == null) return 1;
			var targetCompany = modifyUser.CompanyInfo.CompanyCode;
			// 判断是否有管理此单位的权限，并且级别高于此单位至少1级
			return myManages.Max(m => targetCompany.StartsWith(m.Code) ? targetCompany.Length - m.Code.Length : -1);
		}

		public async Task<Tuple<IEnumerable<Company>, int>> InMyManage(User user)
		{
			var result = await Task.Run(() =>
			 {
				 int totalCount = 0;
				 var list = new List<Company>();
				 var companyCode = user.CompanyInfo.CompanyCode;
				 if (!companyCode.IsNullOrEmpty())
				 {
					 list = _context.CompanyManagers.Where(m => m.UserId == user.Id).Select(m => m.Company).ToList();
					 // 所在单位的主管拥有此单位的管理权
					 var isManager = user.CompanyInfo.Duties.IsMajorManager;
					 if (isManager && list.All(c => c.Code != companyCode))
						 list.Add(user.CompanyInfo.Company);
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
		/// <param name="vacationYear">休假年份</param>
		/// <param name="vacationStatus">统计何种类型</param>
		/// <returns></returns>
		public UserVacationInfoVDto VacationInfo(User targetUser,int vacationYear, MainStatus vacationStatus)
		{
			if (targetUser == null) return null;
			var applies = _context.AppliesDb
				.Where(a => a.BaseInfo.FromId == targetUser.Id)
				.Where(a => a.Status == AuditStatus.Accept)
				.Where(a => a.MainStatus == vacationStatus)
				.Where(a => a.RequestInfo.StampLeave.Value.Year == vacationYear)
				.Where(a => _context.VacationTypes.Any(t => t.Primary && t.Name == a.RequestInfo.VacationType)).ToList(); // 仅主要假期计算天数
			var targetSocial = targetUser.SocialInfo;
			var targetSettle = targetSocial?.Settle;
			if (targetSettle == null) return null;
            var r = targetSettle.GetYearlyLength(targetUser, out bool requireUpdate);
            var requireAddRecord = r.Item2; var maxOnTripTime = r.Item3; var description = r.Item4;
			var yearlyLength = r.Item1 < 0 ? 0 : r.Item1;
			if (targetSettle.Lover?.Valid ?? false)
			{
				// 已婚
				requireUpdate |= (targetSocial.Status & (int)SocialStatus.IsMarried) == 0;
				targetSocial.Status |= (int)SocialStatus.IsMarried;

				if (targetSettle.Self.IsAllopatry(targetSettle.Lover))
				{
					// 分居
					requireUpdate |= (targetSocial.Status & (int)SocialStatus.IsApart) == 0;
					targetSocial.Status |= (int)SocialStatus.IsApart;
				}
				else
				{
					// 同居
					requireUpdate |= (targetSocial.Status & (int)SocialStatus.IsApart) > 0;
					targetSocial.Status -= (int)SocialStatus.IsApart;
				}
			}
			else
			{
				// 未婚
				requireUpdate |= (targetSocial.Status & (int)SocialStatus.IsMarried) > 0;
				targetSocial.Status -= (int)SocialStatus.IsMarried;
			}

			if (requireAddRecord != null)
			{
				var list = new List<AppUsersSettleModifyRecord>(targetSettle.PrevYealyLengthHistory ?? new List<AppUsersSettleModifyRecord>());
				list.Add(requireAddRecord);
				targetSettle.PrevYealyLengthHistory = list;
				_context.AppUserSocialInfoSettles.Update(targetSettle);
				requireUpdate |= true;
			}

			if (requireUpdate)
			{
				_context.AppUserSocialInfos.Update(targetSocial);
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
			var description = "";
			int nowLength = 0;
			int delayLength = 0;
			int nowTimes = 0;
			int onTripTime = 0;
			var f = applies.All<DAL.Entities.ApplyInfo.Apply>(a =>
			{
				nowLength += a.RequestInfo.VacationLength;
				if (a.RequestInfo.OnTripLength > 0) onTripTime++;
				nowTimes++;
				userAdditions.AddRange(a.RequestInfo.AdditialVacations);

				// 处理被召回的假期
				if (a.RecallId != null)
				{
					//不论用户是否休路途，均应该增加一次路途
					maxOnTripTimeGainForRecall++;
					var order = _context.RecallOrders.Find(a.RecallId);
					if (order == null) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.RecallMessage.IdRecordButNoData);
					// 归还天数=应归队 - 召回应归队
					var recallMiniusDay = a.RequestInfo.StampReturn.Value.Subtract(order.ReturnStamp).Days;
					nowLength -= recallMiniusDay;
					// 判断召回应归队到应归队之间的福利假天数
					var benefitDuringRecallAndStampReturn = _vacationCheckServices.GetVacationDates(order.ReturnStamp, recallMiniusDay, true).Sum(v => v.Length);
					if (benefitDuringRecallAndStampReturn > 0) nowLength += benefitDuringRecallAndStampReturn;
				}
				// 处理推迟的假期
				else if (((int)a.ExecuteStatus & (int)ExecuteStatus.Delay) > 0 && a.ExecuteStatusDetailId != null)
				{
					var order = _context.ApplyExcuteStatus.Find(a.ExecuteStatusDetailId);

					var length = order.ReturnStamp.Subtract(a.RequestInfo.StampReturn.Value).Days;
					delayLength += length;
				}
				return true;
			});
			if (maxOnTripTimeGainForRecall > 0) description = $"因期间被召回{maxOnTripTimeGainForRecall}次，全年可休路途次数相应增加。";
			if (delayLength > 0) description = $"{description}因期间归队时间推迟{delayLength}天，全年可休假天数相应减少";
			var vacationInfo = new UserVacationInfoVDto()
			{
				LeftLength = (int)Math.Floor(yearlyLength - nowLength - delayLength),
				MaxTripTimes = maxOnTripTimeGainForRecall,
				NowTimes = nowTimes,
				OnTripTimes = onTripTime,
				YearlyLength = (int)Math.Round(yearlyLength, 0),
				Description = description,
				Additionals = userAdditions
			};
			if (vacationInfo.LeftLength < 0) vacationInfo.LeftLength = 0;
			return vacationInfo;
		}

		public IEnumerable<AppUsersSettleModifyRecord> ModifyUserSettleModifyRecord(User user, Action<IEnumerable<AppUsersSettleModifyRecord>> Callback = null)
		{
			if (user == null) return null;
			var records = user.SocialInfo.Settle.PrevYealyLengthHistory;
			if (Callback != null)
			{
				Callback(records);
				user.SocialInfo.Settle.PrevYealyLengthHistory = records;
				_context.AppUserSocialInfoSettles.Update(user.SocialInfo.Settle);
				_context.SaveChanges();
			}
			return records;
		}

		public AppUsersSettleModifyRecord ModifySettleModifyRecord(int code, Action<AppUsersSettleModifyRecord> Callback = null, bool isDelete = false)
		{
			var record = _context.AppUsersSettleModifyRecordDb.Where(r => r.Code == code).FirstOrDefault();
			if (Callback != null || isDelete)
			{
				Callback?.Invoke(record);
				if (isDelete)
				{
					record.Remove();
					var settlePre = _context.AppUserSocialInfoSettles.Where(s => s.PrevYealyLengthHistory.Any(rec => rec.Code == code)).FirstOrDefault();
					settlePre.PrevYealyLengthHistory = settlePre.PrevYealyLengthHistory.Where(rec => rec.Code != code).ToList();
					_context.AppUserSocialInfoSettles.Update(settlePre);
				}
				_context.AppUsersSettleModifyRecord.Update(record);
				_context.SaveChanges();
			}
			return record;
		}
	}
}