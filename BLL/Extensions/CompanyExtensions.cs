using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BLL.Interfaces;
using DAL.DTO.Company;
using DAL.Entities;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace BLL.Extensions
{

	public static class CompanyExtensions
	{
		public static IEnumerable<string> DistinctByCompany(this IEnumerable<string> list) => list.DistinctByCompany(o => o);
		public static IEnumerable<T> DistinctByCompany<T>(this IEnumerable<T> list, Func<T, string> propGetter) where T : class => list.DistinctByCompany<T>(propGetter,(a, b) =>a.StartsWith(b));
		public static IEnumerable<T> DistinctByCompany<T>(this IEnumerable<T> list,Func<T,string> propGetter,Func<string,string,bool>checkContain)where T:class
		{
			var result = list.ToList();
			for (var i = 0; i < result.Count; i++)
			{
				for (var j = i + 1; j < result.Count; j++)
				{
					var pi = propGetter(result[i]);
					var pj = propGetter(result[j]);
					if (checkContain(pj,pi))
					{
						result.RemoveAt(j--);
						continue;
					}
					if (checkContain(pi,pj))
					{
						result.RemoveAt(i--);
						break;
					}
				}
			}
			return result;
		}
	public static IEnumerable<User> CompanyWithChildCompanyMembers(this Company company, IUsersService usersService) => company.CompanyMembers(usersService, 999);

		public static IEnumerable<User> CompanyMembers(this Company company, IUsersService usersService, int includeChild)
			=> usersService.Find(u => u.CompanyInfo.CompanyCode.Length <= company.Code.Length + includeChild && u.CompanyInfo.CompanyCode.StartsWith(company.Code));

		public static CompanyDto ToDto(this Company company, ICompaniesService companiesService = null)
		{
			if (company == null) return null;
			var b = new CompanyDto()
			{
				Managers = companiesService?.GetCompanyManagers(company.Code, null).Select(u => u.ToSummaryDto()),
				Code = company.Code,
				Name = company.Name,
				Description = company.Description,
				CompanyStatus = company.CompanyStatus,
				Location = company.Location,
				Tags = (company.Tag?.Length ?? 0) == 0 ? Array.Empty<string>() : company.Tag.Split("##")
			};
			return b;
		}

		public static DutiesDto ToDto(this Duties model)
		{
			if (model == null) return null;
			return new DutiesDto()
			{
				Code = model.Code,
				IsMajorManager = model.IsMajorManager,
				Name = model.Name,
				Tags = (model.Tags?.Length ?? 0) == 0 ? Array.Empty<string>() : model.Tags.Split("##")
			};
		}
	}
}