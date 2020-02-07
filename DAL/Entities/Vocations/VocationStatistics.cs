using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DAL.Entities.Vocations
{
	public class VocationStatistics
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
		public virtual VocationStatisticsDescription RootCompanyStatistics { get; set; }
	}
	public class VocationStatisticsDescription:BaseEntity
	{
		public string StatisticsId { get; set; }
		/// <summary>
		/// 单位下属单位休假情况
		/// </summary>
		[NotMapped]
		public virtual IEnumerable<VocationStatisticsDescription> Childs { get; set; }
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
		public virtual VocationStatisticsData CurrentLevelStatistics { get; set; }

		/// <summary>
		/// 包含子单位数据
		/// </summary>
		public virtual VocationStatisticsData IncludeChildLevelStatistics { get; set; }
	}
	public class VocationStatisticsData:BaseEntity
	{

		/// <summary>
		/// 单位本级人员数量<see cref="单位人员总数"/>
		/// </summary>
		public int MembersCount { get; set; }
		/// <summary>
		/// 已完成全年休假指标的人数<see cref="休满假数（率）"/>
		/// </summary>
		public int CompleteYearlyVocationCount { get; set; }
		/// <summary>
		/// 应当休假总天数<see cref="应（实）休总天数"/>
		/// </summary>
		public int CompleteVocationExpectDayCount { get; set; }
		/// <summary>
		/// 实际休假总天数<see cref="应（实）休总天数"/>
		/// </summary>
		public int CompleteVocationRealDayCount { get; set; }
		/// <summary>
		/// 休假率低于60%人员数<see cref="休假率低于60%数"/>
		/// </summary>
		public int MembersVocationDayLessThanP60 { get; set; }
		/// <summary>
		/// 新增申请的数量
		/// </summary>
		public virtual VocationStatisticsDescriptionDataStatusCount ApplyCount { get; set; }
		/// <summary>
		/// 新增申请的人数
		/// </summary>
		public virtual VocationStatisticsDescriptionDataStatusCount ApplyMembersCount { get; set; }
		/// <summary>
		/// 申请天数
		/// </summary>
		public virtual VocationStatisticsDescriptionDataStatusCount ApplySumDayCount { get; set; }
	}
	public class VocationStatisticsDescriptionDataStatusCount : BaseEntity
	{
		public int Access { get; set; }
		public int Deny { get; set; }
		public int Auditing { get; set; }
		public int Sum { get => Access + Deny + Auditing;  }
	}
}
