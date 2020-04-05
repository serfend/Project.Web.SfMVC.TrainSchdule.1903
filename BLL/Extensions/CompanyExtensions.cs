using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BLL.Interfaces;
using DAL.DTO.Company;
using DAL.Entities;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Hosting;

namespace BLL.Extensions
{
	public static class CompanyExtensions
	{
		public static IEnumerable<User> CompanyWithChildCompanyMembers(this Company company, ICompaniesService companiesService, IUsersService usersService)
		{
			var list = new List<User>();
			var r = companiesService?.FindAllChild(company?.Code).All<Company>(c =>
			{
				list.AddRange(c.CompanyWithChildCompanyMembers(companiesService, usersService));
				return true;
			});
			list.AddRange(company.CompanyMembers(usersService));
			return list;
		}

		public static IEnumerable<User> CompanyMembers(this Company company, IUsersService usersService) => usersService?.Find(u => u.CompanyInfo.Company.Code == company.Code);

		public static CompanyDto ToDto(this Company company, ICompaniesService companiesService = null)
		{
			if (company == null) return null;
			var b = new CompanyDto()
			{
				Managers = companiesService?.GetCompanyManagers(company.Code).Select(u => u.ToSummaryDto()),
				Code = company.Code,
				Name = company.Name,
				Description = company.Description,
				Tags = (company.Tag?.Length ?? 0) == 0 ? Array.Empty<string>() : company.Tag.Split("##")
			};
			return b;
		}

		public static string GetCode(this Company company) => company?.Code;

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