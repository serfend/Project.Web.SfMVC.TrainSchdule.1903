using DAL.DTO.User;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Extensions
{
	public static class VocationExtensions
	{
		public static string VocationDescription(this UserVocationInfoVDto info)
		{
			return $"全年假期{info.YearlyLength}天,已休{info.NowTimes}次,{info.YearlyLength - info.LeftLength}天,剩余{info.LeftLength}天。其中可休路途{info.MaxTripTimes}次,已休{info.OnTripTimes}次";
		}
		public static string CombineVocationDescription(this IEnumerable<VocationDescription> model)
		{
			var cs = new StringBuilder();
			foreach(var i in model)
			{
				cs.Append(i.Name).Append(i.Length).AppendLine("天");
			}
			return cs.ToString();
		}
	}
}
