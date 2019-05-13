using System.Linq;
using BLL.Interfaces;
using DAL.DTO.Company;
using DAL.Entities;

namespace BLL.Extensions
{
	public static class CompanyExtensions
	{
		public static CompanyDto ToDto(this Company company, ICompaniesService companiesService)
		{
			var b=new CompanyDto()
			{
				Managers = companiesService.GetCompanyManagers(company.Code).Select(u=>u.ToDto()),
				Code = company.Code,
				Name = company.Name,
				CompanyParentTypeDesc =company.CompanyParentTypeDesc,
				CompanyTypeDesc = company.CompanyTypeDesc
			};
			return b;
		}
	}
}
