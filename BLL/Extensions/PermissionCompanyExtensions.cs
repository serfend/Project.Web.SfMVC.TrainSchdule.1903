using System;
using System.Collections.Generic;
using System.Text;
using TrainSchdule.BLL.DTO;
using TrainSchdule.DAL.Entities;

namespace TrainSchdule.BLL.Extensions
{
	public static class PermissionCompanyExtensions
	{
		public static PermissionCompanyDTO ToDTO(this PermissionCompany item)
		{
			if (item == null)
			{
				return null;
			}

			return new PermissionCompanyDTO
			{
				id=item.Id,
				Path = item.Path,
			};
		}

	}
}
