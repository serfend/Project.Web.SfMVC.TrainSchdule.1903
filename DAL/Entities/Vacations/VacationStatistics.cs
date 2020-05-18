using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DAL.Entities.Vacations
{
	[Table("VocationStatistics")]
	public class VacationStatistics
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public string Id { get; set; }

		public DateTime Start { get; set; }
		public DateTime End { get; set; }

		/// <summary>
		/// 统计期间使用的年度
		/// </summary>
		public int CurrentYear { get; set; }

		/// <summary>
		/// 通常自动生成，也可手动修改
		/// </summary>
		public string Description { get; set; }

		public virtual VacationStatisticsDescription RootCompanyStatistics { get; set; }
	}

	[Table("VocationStatisticsDescriptions")]
	public class VacationStatisticsDescription : BaseEntity
	{
		public string StatisticsId { get; set; }

		/// <summary>
		/// 单位下属单位休假情况
		/// </summary>
		[NotMapped]
		public virtual IEnumerable<VacationStatisticsDescription> Childs { get; set; }

		/// <summary>
		/// 单位
		/// </summary>
		public virtual Company Company { get; set; }

		/// <summary>
		/// 单位本级的休假记录
		/// </summary>
		[NotMapped]
		public virtual IQueryable<ApplyInfo.Apply> Applies { get; set; }

		/// <summary>
		/// 单位本级数据
		/// </summary>
		public virtual VacationStatisticsData CurrentLevelStatistics { get; set; }

		/// <summary>
		/// 包含子单位数据
		/// </summary>
		public virtual VacationStatisticsData IncludeChildLevelStatistics { get; set; }
	}

	[Table("VocationStatisticsDatas")]
	public class VacationStatisticsData : BaseEntity
	{
		/// <summary>
		/// 单位本级人员数量<see cref="单位人员总数"/>
		/// </summary>
		public int MembersCount { get; set; }

		/// <summary>
		/// 已完成全年休假指标的人数<see cref="休满假数（率）"/>
		/// </summary>
		public int CompleteYearlyVacationCount { get; set; }

		/// <summary>
		/// 应当休假总天数<see cref="应（实）休总天数"/>
		/// </summary>
		public int CompleteVacationExpectDayCount { get; set; }

		/// <summary>
		/// 实际休假总天数<see cref="应（实）休总天数"/>
		/// </summary>
		public int CompleteVacationRealDayCount { get; set; }

		/// <summary>
		/// 休假率低于60%人员数<see cref="休假率低于60%数"/>
		/// </summary>
		public int MembersVacationDayLessThanP60 { get; set; }

		/// <summary>
		/// 新增申请的数量
		/// </summary>
		public virtual VacationStatisticsDescriptionDataStatusCount ApplyCount { get; set; }

		/// <summary>
		/// 新增申请的人数
		/// </summary>
		public virtual VacationStatisticsDescriptionDataStatusCount ApplyMembersCount { get; set; }

		/// <summary>
		/// 申请天数
		/// </summary>
		public virtual VacationStatisticsDescriptionDataStatusCount ApplySumDayCount { get; set; }
	}

	[Table("VocationStatisticsDescriptionDataStatusCounts")]
	public class VacationStatisticsDescriptionDataStatusCount : BaseEntity
	{
		public int Access { get; set; }
		public int Deny { get; set; }
		public int Auditing { get; set; }
		public int Sum { get => Access + Deny + Auditing; }
	}
}