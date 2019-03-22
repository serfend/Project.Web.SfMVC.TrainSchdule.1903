using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.DAL.Entities;

namespace TrainSchdule.Web.ViewModels.Student
{
	public class StudentViewModel
	{
		public string Alias { get; set; }
		public int ID { get; set; }
		public int Age { get; set; }
		public DateTime Birth { get; set; }
		public GenderEnum Gender { get; set; }
	}
}
