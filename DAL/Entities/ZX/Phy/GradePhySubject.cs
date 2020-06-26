using System.Collections.Generic;

namespace DAL.Entities.ZX.Phy
{
	public class GradePhySubject : BaseEntity
	{
		/// <summary>
		/// 科目名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 科目中文名称
		/// </summary>
		public string Alias { get; set; }

		/// <summary>
		/// 科目所属分组
		/// </summary>
		public string Group { get; set; }

		public ValueFormat ValueFormat { get; set; }

		/// <summary>
		/// 是否为值越小成绩越好
		/// </summary>
		public bool CountDown { get; set; }

		private IEnumerable<GradePhyStandard> standards;

		public virtual IEnumerable<GradePhyStandard> Standards
		{
			get => standards; set
			{
				foreach (var i in value) { i.BelongTo = this; i.GradePairs = i.GradePairs; }
				standards = value;
			}
		}
	}
}