using DAL.DTO.Apply.ApplyAuditStreamDTO;
using DAL.Entities.ApplyInfo;
using System;
using System.Collections.Generic;
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
				Create = model.Create,
				Description = model.Description,
				Name = model.Name,
				Nodes = model.Nodes?.Length == 0 ? Array.Empty<string>() : model.Nodes.Split("##")
			};
		}
	}
}