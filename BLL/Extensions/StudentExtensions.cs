using System;
using System.Collections.Generic;
using System.Text;
using TrainSchdule.BLL.DTO;
using TrainSchdule.DAL.Entities.UserInfo;

namespace TrainSchdule.BLL.Extensions
{
	public static class StudentExtensions
	{
		public static StudentDTO ToDTO(this Student item)
		{
			if (item == null) return null;
			return new StudentDTO()
			{
				Alias = item.Alias,
				birth = item.Birth,
				Gender = (GenderEnum)item.Gender,
				id = item.Id

			};
		}

		public static Student ToEntity(this StudentDTO item)
		{
			if (item == null) return null;
			var student = new Student()
			{
				//ID = item.id,
				Alias = item.Alias,
				Birth = item.birth,
				Gender = (int)item.Gender
			};
			return student;
		}
	}
}
