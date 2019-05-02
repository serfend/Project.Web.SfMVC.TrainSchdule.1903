using System;
using System.Collections.Generic;
using System.Text;
using TrainSchdule.BLL.DTO;
using TrainSchdule.DAL.Entities;

namespace TrainSchdule.BLL.Extensions
{
	/// <summary>
	/// Methods for mapping user entities to user data transfer objects.
	/// </summary>
	public static class CompanyExtension
	{
		public static bool IsParent(this Company item, Company compareCompany) =>
			compareCompany.Code.StartsWith(item.Code);
		public static CompanyDTO ToDTO(this Company item)
		{
			if (item == null)
			{
				return null;
			}
			var child=new List<string>();
			return new CompanyDTO
			{
				Name = item.Name,
				Path = item.Code,
				Code= item.Code,
				IsPrivate = item.IsPrivate
			};
		}

		public static CompanyDTO ToDTO(this Company item,IEnumerable<UserDTO>members)
		{
			if (item == null)
			{
				return null;
			}

			var company = ToDTO(item);
			company.Members = members;
			return company;
		}
	}
}
