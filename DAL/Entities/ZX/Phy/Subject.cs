using System.Collections.Generic;

namespace DAL.Entities.ZX.Phy
{
	public class Subject : BaseEntity
	{
		/// <summary>
		/// 科目名称
		/// </summary>
		public string Name { get; set; }

		public ValueFormat ValueFormat { get; set; }

		/// <summary>
		/// 是否为值越小成绩越好
		/// </summary>
		public bool CountDown { get; set; }

		private IEnumerable<Standard> standards;

		public virtual IEnumerable<Standard> Standards
		{
			get => standards; set
			{
				foreach (var i in value) { i.BelongTo = this; i.GradePairs = i.GradePairs; }
				standards = value;
			}
		}
	}
}