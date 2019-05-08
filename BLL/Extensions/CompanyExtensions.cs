using System.Linq;
using BLL.Interfaces;
using DAL.DTO.Company;
using DAL.Entities;

namespace BLL.Extensions
{
	public static class CompanyExtensions
	{
		public static CompanyDto ToDTO(this Company company, ICompaniesService companiesService)
		{
			var b=new CompanyDto()
			{
				Managers = companiesService.GetCompanyManagers(company.Code).Select(u=>u.ToDTO()),
				Code = company.Code,
				Name = company.Name
			};
			return b;
		}
	}
}
