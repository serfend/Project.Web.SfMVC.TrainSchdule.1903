using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using DAL.DTO.Company;
using DAL.Entities;

namespace BLL.Interfaces
{
	public interface ICompanyManagerServices
	{
		CompanyManagers Get(Guid id);
		IEnumerable<CompanyManagers> GetAll(Expression<Func<CompanyManagers, bool>> predicate, int page, int pageSize);
		int Edit(CompanyManagers model);
		CompanyManagers CreateManagers(CompanyManagerVDTO model);
		CompanyManagers Create(CompanyManagers model);
		int Delete(CompanyManagers model);
		CompanyManagers GetManagerByUC(string userId, string companyCode);
	}
}
