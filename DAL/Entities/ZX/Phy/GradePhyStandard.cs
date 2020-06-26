using DAL.Entities.UserInfo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DAL.Entities.ZX.Phy
{
	public class GradePhyStandard : BaseEntity
	{
		[NotMapped]
		[JsonIgnore]
		public virtual GradePhySubject BelongTo { get; set; }

		[NotMapped]
		[JsonIgnore]
		public SortedDictionary<int, int> GradePairsInner { get; set; }

		private string GradePairsStr;

		/// <summary>
		/// 分数对     成绩:得分|成绩:得分|成绩:得分|...
		/// </summary>
		public string GradePairs
		{
			get => GradePairsStr;
			set
			{
				GradePairsStr = value;
				if (BelongTo == null) return;
				GradePairsInner = new SortedDictionary<int, int>();
				var list = value.Split('|');
				foreach (var p in list)
				{
					var i = p.Split(':');
					var key = this.ToValue(i[0]);
					if (i.Length == 2) GradePairsInner[key] = Convert.ToInt32(i[1]);
				}
				if (BaseStandard == 0) BaseStandard = 60;
			}
		}

		public string ExpressionWhenFullGrade { get; set; }
		public int minAge { get; set; }
		public int maxAge { get; set; }
		public GenderEnum gender { get; set; }

		/// <summary>
		/// 达到及格所需成绩
		/// </summary>
		public int BaseStandard { get; set; }
	}

	public enum ValueFormat
	{
		Default,
		TimeBase,
		SecondBase
	}

	public static class StandardExtensions
	{
		public static int ToValue(this GradePhyStandard standard, string raw)
		{
			if (standard == null) return 0;
			switch (standard.BelongTo.ValueFormat)
			{
				case ValueFormat.Default: return Convert.ToInt32(raw);
				case ValueFormat.SecondBase:
				case ValueFormat.TimeBase:
					{
						var t = new TimeSpan();
						var minute = raw.Split('\'');
						if (minute.Length > 1)
						{
							t = t.Add(TimeSpan.FromMinutes(Convert.ToInt32(minute[0])));
							raw = minute[1];
						}
						var ms = Convert.ToInt32(System.Decimal.Parse((raw.Replace("\"", ".")), System.Globalization.NumberStyles.Float));
						t = t.Add(TimeSpan.FromMilliseconds(ms * 1000));
						return (int)(t.TotalMilliseconds * (standard.BelongTo.CountDown ? -1 : 1));
					}
			}
			return 0;
		}

		public static string ToRawValue(this GradePhyStandard standard, int expectGrade = 0)
		{
			if (standard == null) return "无此标准";
			int value = 0;
			if (expectGrade == 0) expectGrade = standard.BaseStandard;
			bool anyMatch = false;
			foreach (var v in standard.GradePairsInner) if (v.Value >= expectGrade) { value = v.Key; anyMatch = true; break; }
			if (!anyMatch) value = standard.GradePairsInner.Keys.Max();
			value = Math.Abs(value);
			switch (standard.BelongTo.ValueFormat)
			{
				case ValueFormat.Default: return Convert.ToString(value);
				case ValueFormat.SecondBase:
				case ValueFormat.TimeBase:
					{
						var t = TimeSpan.FromMilliseconds(value);
						return $"{Math.Floor(t.TotalMinutes)}'{t.Seconds}";
					}
			}
			return "未设计分法";
		}

		public static int Age(this DateTime birthDay)
		{
			DateTime now = DateTime.Now;
			int age = now.Year - birthDay.Year;
			if (birthDay > now.AddYears(-age)) age--;
			return age;
		}
	}
}