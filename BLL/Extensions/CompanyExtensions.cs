using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BLL.Interfaces;
using DAL.DTO.Company;
using DAL.Entities;
using DAL.Entities.UserInfo;

namespace BLL.Extensions
{
	public static class CompanyExtensions
	{

		public static IEnumerable<User> CompanyWithChildCompanyMembers(this Company company,ICompaniesService companiesService,IUsersService usersService)
		{
			var list = new List<User>();
			var r=companiesService.FindAllChild(company.Code).All<Company>(c=> {
				list.AddRange(c.CompanyWithChildCompanyMembers(companiesService, usersService));
				return true;
   });
			list.AddRange(company.CompanyMembers(usersService));
			return list;
			 
		}
		public static IEnumerable<User>CompanyMembers(this Company company,IUsersService usersService)=> usersService.Find(u => u.CompanyInfo.Company.Code == company.Code);
		public static CompanyDto ToDto(this Company company, ICompaniesService companiesService)
		{
			var b=new CompanyDto()
			{
				Managers = companiesService?.GetCompanyManagers(company?.Code).Select(u=>u.ToSummaryDto()),
				Code = company?.Code,
				Name = company?.Name,
				Description=company?.Description
			};
			return b;
		}
	}
}
