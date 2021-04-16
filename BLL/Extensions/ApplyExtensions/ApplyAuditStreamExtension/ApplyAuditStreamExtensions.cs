using BLL.Interfaces;
using DAL.DTO.Apply.ApplyAuditStreamDTO;
using DAL.Entities.ApplyInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Extensions.ApplyExtensions.ApplyAuditStreamExtension
{
	public static class ApplyAuditStreamExtensions
	{
		public static ApplyAuditStreamDto ToDtoModel(this ApplyAuditStream model)
		{
			if (model == null) return null;
			return new ApplyAuditStreamDto()
			{
				Id = model.Id,
				Create = model.Create,
				Description = model.Description,
				Name = model.Name,
				Nodes = (model.Nodes?.Length ?? 0) == 0 ? Array.Empty<string>() : model.Nodes.Split("##"),
				CompanyRegion = model.RegionOnCompany
			};
		}

		public static ApplyAuditStreamVDto ToVDtoModel(this ApplyAuditStreamDto model, IApplyAuditStreamServices applyAuditStreamServices, IUsersService usersService, ICompaniesService companiesService,string entityType)
		{
			if (model == null) return null;
			return new ApplyAuditStreamVDto()
			{
				Id = model.Id,
				Create = model.Create,
				Description = model.Description,
				Name = model.Name,
				Nodes = model.Nodes?.Select(n => applyAuditStreamServices.EditNode(n, entityType).ToNodeDtoModel().ToNodeVDtoModel(usersService, companiesService)),
				RegionOnCompany = model.CompanyRegion
			};
		}
	}
}