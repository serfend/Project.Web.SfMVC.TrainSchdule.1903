using DAL.Entities;
using DAL.Entities.ApplyInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces
{
	public interface IApplyAuditStreamServices
	{
		ApplyAuditStreamSolutionRule NewSolutionRule(ApplyAuditStream solution, MembersFilter filter, string name, string description = null, int priority = 0, bool enable = false);

		ApplyAuditStream NewSolution(IEnumerable<ApplyAuditStreamNodeAction> Nodes, string name, string description = null);

		ApplyAuditStreamNodeAction NewNode(MembersFilter filter, string name, string description = null);

		void EditSolutionRule(string solutionRuleName, Func<ApplyAuditStreamSolutionRule, bool> callback);

		void EditSolution(string solutionName, Func<ApplyAuditStream, bool> callback);

		void EditNode(string nodeName, Func<ApplyAuditStreamNodeAction, bool> callback);

		/// <summary>
		/// 通过用户信息，决定此用户需要使用哪种审批流规则
		/// </summary>
		/// <param name="company"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		ApplyAuditStreamSolutionRule GetAuditSolutionRule(DAL.Entities.UserInfo.User user);

		/// <summary>
		/// 在当前单位中寻找符合进行审批条件的人员
		/// </summary>
		/// <param name="company"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		IEnumerable<string> GetToAuditMembers(string company, MembersFilter filter);
	}
}