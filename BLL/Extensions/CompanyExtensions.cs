using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Interfaces;
using DAL.DTO.Company;
using DAL.Entities;

namespace BLL.Extensions
{
	public static class CompanyExtensions
	{
		public static CompanyDTO ToDTO(this Company company, ICompaniesService companiesService)
		{
			var b=new CompanyDTO()
			{
				Managers = companiesService.GetCompanyManagers(company.Code).Select(u=>u.ToDTO()),
				Code = company.Code,
				Name = company.Name
			};
			return b;
		}
	}
}
