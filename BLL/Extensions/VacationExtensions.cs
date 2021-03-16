using DAL.DTO.Apply;
using DAL.DTO.User;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Extensions
{
	public static class VacationExtensions
	{
		public static string VacationDescription(this UserVacationInfoVDto info)
		{
			return $"全年假期{info?.YearlyLength}天,已休{info.NowTimes}次,{info.YearlyLength - info.LeftLength}天,剩余{info.LeftLength}天。其中可休路途{info.MaxTripTimes}次,已休{info.OnTripTimes}次";
		}

		public static string CombineVacationDescription(this IEnumerable<VacationDescriptionDto> model)
		{
			if (model == null) return "无福利假";
			var cs = new StringBuilder();
			foreach (var i in model)
			{
				cs.Append(i.Name).Append(i.Length).AppendLine("天");
			}
			return cs.ToString();
		}
		public struct DateTimeRange
		{
			public DateTime Start { get; set; }
			public DateTime End { get; set; }
		}
		/// <summary>
		/// 获取当前范围在日期范围中重叠的长度
		/// </summary>
		/// <param name="a">当前范围</param>
		/// <param name="b">日期范围</param>
		/// <returns></returns>
		public static int CoverDays(this DateTimeRange a, DateTimeRange b)
		{
			var a1 = a.Start;
			var a2 = a.End;
			var b1 = b.Start;
			var b2 = b.End;
			// b1 b2 a1 a2
			if (b1 <= b2 && b2 <= a1 && a1 <= a2) return 0;
			// b1 a1 b2 a2
			if (b1 <= a1 && a1 <= b2 && b2 <= a2) return b.End.Subtract(a.Start).Days;
			// b1 a1 a2 b2
			if (b1 <= a1 && a1 <= a2 && a2 <= b2) return a.End.Subtract(a.Start).Days;
			// a1 b1 b2 a2
			if (a1 <= b1 && b1 <= b2 && b2 <= a2) return b.End.Subtract(b.Start).Days;
			// a1 b1 a2 b2
			if (a1 <= b2 && b1 <= a2 && a2 <= b2) return a.End.Subtract(b.Start).Days;
			// a1 a2 b1 b2
			if (a1 <= a2 && a2 <= b1 && b1 <= b2) return 0;
			return -1;
		}
	}

}