using DAL.Data;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using DAL.Entities.Vocations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Extensions
{
	public static class VocationStatisticsExtensions
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
			int ApplyCountAccess = 0, ApplyCountAuditing = 0, ApplyCountDeny = 0, ApplyMembersCountAccess = 0, ApplyMembersCountAuditing = 0, ApplyMembersCountDeny = 0, ApplySumDayCountAccess = 0, ApplySumDayCountAuditing = 0, ApplySumDayCountDeny = 0, MembersCount = 0, CompleteYearlyVocationCount = 0, MembersVocationDayLessThanP60 = 0, CompleteVocationExpectDayCount = 0, CompleteVocationRealDayCount = 0;
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
						var stampReturn = a.RequestInfo.StampLeave.Value.AddDays(a.RequestInfo.VocationLength) > DateTime.Now ? DateTime.Now : a.RequestInfo.StampReturn.Value;//截止今日
						CompleteVocationRealDayCount += stampReturn.Subtract(a.RequestInfo.StampLeave.Value).Days;//时间段内休假天数
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
				var memberYearlyLength = (int) history?.OrderByDescending(rec => rec.UpdateDate).FirstOrDefault()?.Length;
				
				CompleteVocationExpectDayCount += memberYearlyLength; // 单位全年应休假天数
				var membersApplies = context.AppliesDb.Where<Apply>(a => a.BaseInfo.From.Id == member.Id && a.Create.Value.Year == currentYear);
				//全年休假天数
				var memberCompleteVocation = membersApplies.Sum<Apply>(a => a.Status == AuditStatus.Accept ? a.RequestInfo.VocationLength : 0);
				if (memberCompleteVocation >= memberYearlyLength) CompleteYearlyVocationCount++;
				else if (memberCompleteVocation < 0.6f * memberYearlyLength) MembersVocationDayLessThanP60++;

				ApplySumDayCountAccess += memberCompleteVocation;
				ApplySumDayCountDeny = membersApplies.Sum<Apply>(a => a.Status == AuditStatus.Denied ? a.RequestInfo.VocationLength : 0);
				ApplySumDayCountAuditing = membersApplies.Sum<Apply>(a => a.Status == AuditStatus.Auditing ? a.RequestInfo.VocationLength : 0);

				membersApplies = model.Applies.Where<Apply>(a => a.BaseInfo.From
				  .Id == member.Id);
				if (membersApplies.Any<Apply>(a => a.Status == AuditStatus.Accept)) ApplyMembersCountAccess++;
				if (membersApplies.Any<Apply>(a => a.Status == AuditStatus.Denied)) ApplyMembersCountDeny++;
				if (membersApplies.Any<Apply>(a => a.Status == AuditStatus.Auditing)) ApplyMembersCountAuditing++;
			}
			VocationStatisticsData tmp = null;
			InputStatisticsData(ref tmp, ApplyCountAccess, ApplyCountAuditing, ApplyCountDeny, ApplyMembersCountAccess, ApplyMembersCountAuditing, ApplyMembersCountDeny, ApplySumDayCountAccess, ApplySumDayCountAuditing, ApplySumDayCountDeny, MembersCount, CompleteYearlyVocationCount, MembersVocationDayLessThanP60, CompleteVocationExpectDayCount, CompleteVocationRealDayCount);
			model.CurrentLevelStatistics = tmp;
		}

		private static void CaculateChildLevel(ref VacationStatisticsDescription model)
		{
			int ApplyCountAccess = 0, ApplyCountAuditing = 0, ApplyCountDeny = 0, ApplyMembersCountAccess = 0, ApplyMembersCountAuditing = 0, ApplyMembersCountDeny = 0, ApplySumDayCountAccess = 0, ApplySumDayCountAuditing = 0, ApplySumDayCountDeny = 0, MembersCount = 0, CompleteYearlyVocationCount = 0, MembersVocationDayLessThanP60 = 0, CompleteVocationExpectDayCount = 0, CompleteVocationRealDayCount = 0;
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
			CompleteYearlyVocationCount = model.CurrentLevelStatistics.CompleteYearlyVocationCount + model.Childs.Sum<VacationStatisticsDescription>(v => v.IncludeChildLevelStatistics.CompleteYearlyVocationCount);
			MembersVocationDayLessThanP60 = model.CurrentLevelStatistics.MembersVocationDayLessThanP60 + model.Childs.Sum<VacationStatisticsDescription>(v => v.IncludeChildLevelStatistics.MembersVocationDayLessThanP60);
			CompleteVocationExpectDayCount = model.CurrentLevelStatistics.CompleteVocationExpectDayCount + model.Childs.Sum<VacationStatisticsDescription>(v => v.IncludeChildLevelStatistics.CompleteVocationExpectDayCount);
			CompleteVocationRealDayCount = model.CurrentLevelStatistics.CompleteVocationRealDayCount + model.Childs.Sum<VacationStatisticsDescription>(v => v.IncludeChildLevelStatistics.CompleteVocationRealDayCount);

			VocationStatisticsData tmp = null;
			InputStatisticsData(ref tmp, ApplyCountAccess, ApplyCountAuditing, ApplyCountDeny, ApplyMembersCountAccess, ApplyMembersCountAuditing, ApplyMembersCountDeny, ApplySumDayCountAccess, ApplySumDayCountAuditing, ApplySumDayCountDeny, MembersCount, CompleteYearlyVocationCount, MembersVocationDayLessThanP60, CompleteVocationExpectDayCount, CompleteVocationRealDayCount);
			model.IncludeChildLevelStatistics = tmp;
		}

		private static void InputStatisticsData(ref VocationStatisticsData model, int ApplyCountAccess, int ApplyCountAuditing, int ApplyCountDeny, int ApplyMembersCountAccess, int ApplyMembersCountAuditing, int ApplyMembersCountDeny, int ApplySumDayCountAccess, int ApplySumDayCountAuditing, int ApplySumDayCountDeny, int MembersCount, int CompleteYearlyVocationCount, int MembersVocationDayLessThanP60, int CompleteVocationExpectDayCount, int CompleteVocationRealDayCount)
		{
			model = new VocationStatisticsData()
			{
				ApplyCount = new VocationStatisticsDescriptionDataStatusCount()
				{
					Access = ApplyCountAccess,
					Auditing = ApplyCountAuditing,
					Deny = ApplyCountDeny
				},
				ApplyMembersCount = new VocationStatisticsDescriptionDataStatusCount()
				{
					Access = ApplyMembersCountAccess,
					Auditing = ApplyMembersCountAuditing,
					Deny = ApplyMembersCountDeny
				},
				ApplySumDayCount = new VocationStatisticsDescriptionDataStatusCount()
				{
					Access = ApplySumDayCountAccess,
					Auditing = ApplySumDayCountAuditing,
					Deny = ApplySumDayCountDeny
				},
				MembersCount = MembersCount,
				CompleteYearlyVocationCount = CompleteYearlyVocationCount,
				MembersVocationDayLessThanP60 = MembersVocationDayLessThanP60,
				CompleteVocationExpectDayCount = CompleteVocationExpectDayCount,
				CompleteVocationRealDayCount = CompleteVocationRealDayCount
			};
		}
	}
}