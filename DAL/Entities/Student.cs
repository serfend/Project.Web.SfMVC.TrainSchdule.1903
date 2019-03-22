using System;
using System.Collections.Generic;
using System.Text;

namespace TrainSchdule.DAL.Entities
{
	public class Student:BaseEntity
	{
		public string Alias { get; set; }
		public DateTime Birth { get; set; }
		public int Gender { get; set; }

		
	}
	public enum GenderEnum
	{
		Unknown = 0,
		Male = 1,
		Female = 2,
		
	}
}
