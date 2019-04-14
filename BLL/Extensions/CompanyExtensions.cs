﻿using System;
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
				Path = item.Path,
				id=item.Id
			};
		}

		public static CompanyDTO ToDTO(this Company item,bool isParent,IEnumerable<UserDTO>members)
		{
			if (item == null)
			{
				return null;
			}

			var company = ToDTO(item);
			company.IsParent = isParent;
			company.Members = members;
			return company;
		}
	}
}
