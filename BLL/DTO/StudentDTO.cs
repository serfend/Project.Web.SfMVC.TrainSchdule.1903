using System;
using System.Collections.Generic;
using System.Text;
using TrainSchdule.DAL.Entities.UserInfo;

namespace TrainSchdule.BLL.DTO.UserInfo
{
	public class StudentDTO
	{
		public string Alias { get; set; }
		public Guid id { get; set; }
		public DateTime birth { get; set; }
		public GenderEnum Gender { get; set; }
	}
}
