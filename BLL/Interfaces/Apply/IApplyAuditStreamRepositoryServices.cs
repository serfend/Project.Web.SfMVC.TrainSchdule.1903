using DAL.Entities.ApplyInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces
{
	public interface IApplyAuditStreamRepositoryServices
	{
		ApplyAuditStreamNodeAction GetNode(Guid id);

		ApplyAuditStreamSolutionRule GetRule(Guid id);

		ApplyAuditStream GetSolution(Guid id);
	}
}