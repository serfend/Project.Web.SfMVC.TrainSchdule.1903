using System;
using System.Collections.Generic;
using System.Text;
using TrainSchdule.DAL.Entities;

namespace TrainSchdule.BLL.DTO
{
	public class StudentDTO
	{
		public string Alias { get; set; }
		public int id { get; set; }
		public DateTime birth { get; set; }
		public GenderEnum Gender { get; set; }
	}
}
