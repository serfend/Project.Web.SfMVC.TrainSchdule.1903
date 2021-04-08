using DAL.Entities;
using DAL.Entities.ApplyInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces
{
	public interface IApplyAuditStreamServices : IApplyAuditStreamRepositoryServices
	{
		ApplyAuditStreamSolutionRule NewSolutionRule(ApplyAuditStream solution, IMembersFilter filter, string name, string companyRegion, string description = null, int priority = 0, bool enable = false,string entityType=null);

		ApplyAuditStream NewSolution(IEnumerable<ApplyAuditStreamNodeAction> Nodes, string name, string companyRegion, string description = null, string entityType = null);

		ApplyAuditStreamNodeAction NewNode(IMembersFilter filter, string name, string companyRegion, string description = null, string entityType = null);

		ApplyAuditStreamSolutionRule EditSolutionRule(string solutionRuleName, string entityType = null, Func<ApplyAuditStreamSolutionRule, bool> callback = null);

		ApplyAuditStream EditSolution(string solutionName, string entityType = null, Func<ApplyAuditStream, bool> callback = null);

		ApplyAuditStreamNodeAction EditNode(string nodeName, string entityType = null, Func<ApplyAuditStreamNodeAction, bool> callback = null);

		/// <summary>
		/// 通过用户信息，决定此用户需要使用哪种审批流规则
		/// </summary>
		/// <param name="user"></param>
		/// <param name="entityType">审批流作用类型</param>
		/// <param name="CheckInvalidAccount"></param>
		/// <returns></returns>
		ApplyAuditStreamSolutionRule GetAuditSolutionRule(DAL.Entities.UserInfo.User user,string entityType, bool CheckInvalidAccount);

		/// <summary>
		/// 在当前单位中寻找符合进行审批条件的人员
		/// </summary>
		/// <param name="company">当前审批单位</param>
		/// <param name="companyRegion">单位作用域</param>
		/// <param name="filter">人员过滤器</param>
		/// <param name="CheckInvalidAccount">是否检查无效账号</param>
		/// <returns></returns>
		IEnumerable<string> GetToAuditMembers(string company, string companyRegion, IMembersFilter filter, bool CheckInvalidAccount);
	}
}