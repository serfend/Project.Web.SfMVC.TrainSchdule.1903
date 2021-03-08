using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DAL.DTO.Company;
using DAL.Entities;
using DAL.Entities.UserInfo;

namespace BLL.Interfaces
{
	public interface ICompanyManagerServices
	{
		CompanyManagers GetById(Guid id);

		IEnumerable<CompanyManagers> GetAll(Expression<Func<CompanyManagers, bool>> predicate, int page, int pageSize);

		int Edit(CompanyManagers model);

		CompanyManagers CreateManagers(CompanyManagerVdto model);

		CompanyManagers Create(CompanyManagers model);

		int Delete(CompanyManagers model);

		/// <summary>
		/// 获取单位所有的管理员
		/// </summary>
		/// <param name="companyCode"></param>
		/// <returns></returns>
		IQueryable<CompanyManagers> GetManagers(string companyCode);
		/// <summary>
		/// 获取单位的成员
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		IQueryable<User> GetMembers(string code);
	}
}