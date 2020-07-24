using DAL.Entities;
using DAL.Entities.ApplyInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces
{
	public interface IApplyAuditStreamServices : IApplyAuditStreamRepositoryServices
	{
		ApplyAuditStreamSolutionRule NewSolutionRule(ApplyAuditStream solution, IMembersFilter filter, string name, string description = null, int priority = 0, bool enable = false);

		ApplyAuditStream NewSolution(IEnumerable<ApplyAuditStreamNodeAction> Nodes, string name, string description = null);

		ApplyAuditStreamNodeAction NewNode(IMembersFilter filter, string name, string description = null);

		ApplyAuditStreamSolutionRule EditSolutionRule(string solutionRuleName, Func<ApplyAuditStreamSolutionRule, bool> callback = null);

		ApplyAuditStream EditSolution(string solutionName, Func<ApplyAuditStream, bool> callback = null);

		ApplyAuditStreamNodeAction EditNode(string nodeName, Func<ApplyAuditStreamNodeAction, bool> callback = null);

		/// <summary>
		/// 通过用户信息，决定此用户需要使用哪种审批流规则
		/// </summary>
		/// <param name="user"></param>
		/// <param name="CheckInvalidAccount"></param>
		/// <returns></returns>
		ApplyAuditStreamSolutionRule GetAuditSolutionRule(DAL.Entities.UserInfo.User user, bool CheckInvalidAccount);

		/// <summary>
		/// 在当前单位中寻找符合进行审批条件的人员
		/// </summary>
		/// <param name="company"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		IEnumerable<string> GetToAuditMembers(string company, IMembersFilter filter, bool CheckInvalidAccount);
	}
}