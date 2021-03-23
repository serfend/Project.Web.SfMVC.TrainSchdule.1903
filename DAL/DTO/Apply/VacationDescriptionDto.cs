using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Apply
{
    public class VacationDescriptionDto
	{
        
		public int Id { get; set; }

		public DateTime Start { get; set; }

		public int Length { get; set; }
		public string Name { get; set; }
		public int UseLength { get; set; }
}
	public static class VacationDescriptionExtensions {
		public static VacationDescriptionDto AttachUserSet(this VacationDescriptionDto model, int userSet)
		{
			if (userSet < 0) model.UseLength = 0;
			else if (userSet > model.Length) model.UseLength = model.Length;
			else model.UseLength = userSet;
			return model;
		}
		public static VacationDescriptionDto ToModel(this VacationDescription model,int? length = null) => new VacationDescriptionDto()
		{
			Id = model.Id,
			Length = length ?? model.Length,
			Name = model.Name,
			Start = model.Start,
			UseLength = length ?? model.Length
		};
		public static VacationDescription ToModel(this VacationDescriptionDto model)
        {
			return new VacationDescription()
			{
				Id= model.Id,
				Length=model.Length,
				Name=model.Name,
				Start=model.Start
			};
        }
	}
}
