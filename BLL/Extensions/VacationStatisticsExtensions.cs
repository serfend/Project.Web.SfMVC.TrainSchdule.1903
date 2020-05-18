using DAL.Data;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using DAL.Entities.Vacations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Extensions
{
	public static class VacationStatisticsExtensions
	{
		public static void StatisticsInit(ref VacationStatisticsDescription model, ApplicationDbContext context, int currentYear, string statisticsId)
		{
			if (context == null) return;
			if (model == null) return;
			model.StatisticsId = statisticsId;
			var tmpList = new List<VacationStatisticsDescription>();
			foreach (var m in model.Childs)
			{
				VacationStatisticsDescription tmp = m;
				StatisticsInit(ref tmp, context, currentYear, statisticsId);
				tmpList.Add(tmp);
			}
			model.Childs = tmpList;
			CaculateCurrentLevel(ref model, context, currentYear);
			CaculateChildLevel(ref model);
		}

		private static void CaculateCurrentLevel(ref VacationStatisticsDescription model, ApplicationDbContext context, int currentYear)
		{
			int ApplyCountAccess = 0, ApplyCountAuditing = 0, ApplyCountDeny = 0, ApplyMembersCountAccess = 0, ApplyMembersCountAuditing = 0, ApplyMembersCountDeny = 0, ApplySumDayCountAccess = 0, ApplySumDayCountAuditing = 0, ApplySumDayCountDeny = 0, MembersCount = 0, CompleteYearlyVacationCount = 0, MembersVacationDayLessThanP60 = 0, CompleteVacationExpectDayCount = 0, CompleteVacationRealDayCount = 0;
			foreach (var a in model.Applies)
			{
				if (a.Status == AuditStatus.NotPublish || a.Status == AuditStatus.NotSave || a.Status == AuditStatus.Withdrew) continue;
				else
				{
					if (a.Status == AuditStatus.Denied)
					{
						ApplyCountDeny++;//时间段内未通过审批的申请数量
					}
					else if (a.Status == AuditStatus.Accept)
					{
						ApplyCountAccess++;
						var stampReturn = a.RequestInfo.StampLeave.Value.AddDays(a.RequestInfo.VacationLength) > DateTime.Now ? DateTime.Now : a.RequestInfo.StampReturn.Value;//截止今日
						CompleteVacationRealDayCount += stampReturn.Subtract(a.RequestInfo.StampLeave.Value).Days;//时间段内休假天数
					}
					else
					{
						ApplyCountAuditing++;//时间段内审批中的申请
					}
				}
			}
			var companyCode = model.Company.Code;
			var users = context.AppUsers.Where<User>(u => u.CompanyInfo.Company.Code == companyCode);
			var allUsers = users.ToList(); // 执行查询，取消懒加载
			foreach (var member in allUsers)
			{
				MembersCount++;
				var history = member.SocialInfo?.Settle?.PrevYealyLengthHistory;
				var memberYearlyLength = ((int?)history?.OrderByDescending(rec => rec.UpdateDate).FirstOrDefault()?.Length) ?? 0;

				CompleteVacationExpectDayCount += memberYearlyLength; // 单位全年应休假天数
				var membersApplies = context.AppliesDb.Where<Apply>(a => a.BaseInfo.From.Id == member.Id && a.Create.Value.Year == currentYear);
				//全年休假天数
				var memberCompleteVacation = membersApplies.Sum<Apply>(a => a.Status == AuditStatus.Accept ? a.RequestInfo.VacationLength : 0);
				if (memberCompleteVacation >= memberYearlyLength) CompleteYearlyVacationCount++;
				else if (memberCompleteVacation < 0.6f * memberYearlyLength) MembersVacationDayLessThanP60++;

				ApplySumDayCountAccess += memberCompleteVacation;
				ApplySumDayCountDeny = membersApplies.Sum<Apply>(a => a.Status == AuditStatus.Denied ? a.RequestInfo.VacationLength : 0);
				ApplySumDayCountAuditing = membersApplies.Sum<Apply>(a => a.Status == AuditStatus.Auditing ? a.RequestInfo.VacationLength : 0);

				membersApplies = model.Applies.Where<Apply>(a => a.BaseInfo.From
				  .Id == member.Id);
				if (membersApplies.Any<Apply>(a => a.Status == AuditStatus.Accept)) ApplyMembersCountAccess++;
				if (membersApplies.Any<Apply>(a => a.Status == AuditStatus.Denied)) ApplyMembersCountDeny++;
				if (membersApplies.Any<Apply>(a => a.Status == AuditStatus.Auditing)) ApplyMembersCountAuditing++;
			}
			VacationStatisticsData tmp = null;
			InputStatisticsData(ref tmp, ApplyCountAccess, ApplyCountAuditing, ApplyCountDeny, ApplyMembersCountAccess, ApplyMembersCountAuditing, ApplyMembersCountDeny, ApplySumDayCountAccess, ApplySumDayCountAuditing, ApplySumDayCountDeny, MembersCount, CompleteYearlyVacationCount, MembersVacationDayLessThanP60, CompleteVacationExpectDayCount, CompleteVacationRealDayCount);
			model.CurrentLevelStatistics = tmp;
		}

		private static void CaculateChildLevel(ref VacationStatisticsDescription model)
		{
			int ApplyCountAccess = 0, ApplyCountAuditing = 0, ApplyCountDeny = 0, ApplyMembersCountAccess = 0, ApplyMembersCountAuditing = 0, ApplyMembersCountDeny = 0, ApplySumDayCountAccess = 0, ApplySumDayCountAuditing = 0, ApplySumDayCountDeny = 0, MembersCount = 0, CompleteYearlyVacationCount = 0, MembersVacationDayLessThanP60 = 0, CompleteVacationExpectDayCount = 0, CompleteVacationRealDayCount = 0;
			ApplyCountAccess = model.CurrentLevelStatistics.ApplyCount.Access + model.Childs.Sum<VacationStatisticsDescription>(v => v.IncludeChildLevelStatistics.ApplyCount.Access);
			ApplyCountAuditing = model.CurrentLevelStatistics.ApplyCount.Auditing + model.Childs.Sum<VacationStatisticsDescription>(v => v.IncludeChildLevelStatistics.ApplyCount.Auditing);
			ApplyCountDeny = model.CurrentLevelStatistics.ApplyCount.Deny + model.Childs.Sum<VacationStatisticsDescription>(v => v.IncludeChildLevelStatistics.ApplyCount.Deny);
			ApplyMembersCountAccess = model.CurrentLevelStatistics.ApplyMembersCount.Access + model.Childs.Sum<VacationStatisticsDescription>(v => v.IncludeChildLevelStatistics.ApplyMembersCount.Access);
			ApplyMembersCountAuditing = model.CurrentLevelStatistics.ApplyMembersCount.Auditing + model.Childs.Sum<VacationStatisticsDescription>(v => v.IncludeChildLevelStatistics.ApplyMembersCount.Auditing);
			ApplyMembersCountDeny = model.CurrentLevelStatistics.ApplyMembersCount.Deny + model.Childs.Sum<VacationStatisticsDescription>(v => v.IncludeChildLevelStatistics.ApplyMembersCount.Deny);
			ApplySumDayCountAccess = model.CurrentLevelStatistics.ApplySumDayCount.Access + model.Childs.Sum<VacationStatisticsDescription>(v => v.IncludeChildLevelStatistics.ApplySumDayCount.Access);
			ApplySumDayCountAuditing = model.CurrentLevelStatistics.ApplySumDayCount.Auditing + model.Childs.Sum<VacationStatisticsDescription>(v => v.IncludeChildLevelStatistics.ApplySumDayCount.Auditing);
			ApplySumDayCountDeny = model.CurrentLevelStatistics.ApplySumDayCount.Deny + model.Childs.Sum<VacationStatisticsDescription>(v => v.IncludeChildLevelStatistics.ApplySumDayCount.Deny);
			MembersCount = model.CurrentLevelStatistics.MembersCount + model.Childs.Sum<VacationStatisticsDescription>(v => v.IncludeChildLevelStatistics.MembersCount);
			CompleteYearlyVacationCount = model.CurrentLevelStatistics.CompleteYearlyVacationCount + model.Childs.Sum<VacationStatisticsDescription>(v => v.IncludeChildLevelStatistics.CompleteYearlyVacationCount);
			MembersVacationDayLessThanP60 = model.CurrentLevelStatistics.MembersVacationDayLessThanP60 + model.Childs.Sum<VacationStatisticsDescription>(v => v.IncludeChildLevelStatistics.MembersVacationDayLessThanP60);
			CompleteVacationExpectDayCount = model.CurrentLevelStatistics.CompleteVacationExpectDayCount + model.Childs.Sum<VacationStatisticsDescription>(v => v.IncludeChildLevelStatistics.CompleteVacationExpectDayCount);
			CompleteVacationRealDayCount = model.CurrentLevelStatistics.CompleteVacationRealDayCount + model.Childs.Sum<VacationStatisticsDescription>(v => v.IncludeChildLevelStatistics.CompleteVacationRealDayCount);

			VacationStatisticsData tmp = null;
			InputStatisticsData(ref tmp, ApplyCountAccess, ApplyCountAuditing, ApplyCountDeny, ApplyMembersCountAccess, ApplyMembersCountAuditing, ApplyMembersCountDeny, ApplySumDayCountAccess, ApplySumDayCountAuditing, ApplySumDayCountDeny, MembersCount, CompleteYearlyVacationCount, MembersVacationDayLessThanP60, CompleteVacationExpectDayCount, CompleteVacationRealDayCount);
			model.IncludeChildLevelStatistics = tmp;
		}

		private static void InputStatisticsData(ref VacationStatisticsData model, int ApplyCountAccess, int ApplyCountAuditing, int ApplyCountDeny, int ApplyMembersCountAccess, int ApplyMembersCountAuditing, int ApplyMembersCountDeny, int ApplySumDayCountAccess, int ApplySumDayCountAuditing, int ApplySumDayCountDeny, int MembersCount, int CompleteYearlyVacationCount, int MembersVacationDayLessThanP60, int CompleteVacationExpectDayCount, int CompleteVacationRealDayCount)
		{
			model = new VacationStatisticsData()
			{
				ApplyCount = new VacationStatisticsDescriptionDataStatusCount()
				{
					Access = ApplyCountAccess,
					Auditing = ApplyCountAuditing,
					Deny = ApplyCountDeny
				},
				ApplyMembersCount = new VacationStatisticsDescriptionDataStatusCount()
				{
					Access = ApplyMembersCountAccess,
					Auditing = ApplyMembersCountAuditing,
					Deny = ApplyMembersCountDeny
				},
				ApplySumDayCount = new VacationStatisticsDescriptionDataStatusCount()
				{
					Access = ApplySumDayCountAccess,
					Auditing = ApplySumDayCountAuditing,
					Deny = ApplySumDayCountDeny
				},
				MembersCount = MembersCount,
				CompleteYearlyVacationCount = CompleteYearlyVacationCount,
				MembersVacationDayLessThanP60 = MembersVacationDayLessThanP60,
				CompleteVacationExpectDayCount = CompleteVacationExpectDayCount,
				CompleteVacationRealDayCount = CompleteVacationRealDayCount
			};
		}
	}
}