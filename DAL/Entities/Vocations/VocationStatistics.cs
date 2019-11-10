using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Entities.Vocations
{
	public class VocationStatistics:BaseEntity
	{
		public string Code { get; set; }
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
		public virtual IEnumerable<VocationStatisticsDescription> vocationStatisticsDescription { get; set; }
	}
	public class VocationStatisticsDescription
	{
		/// <summary>
		/// 单位下属单位休假情况
		/// </summary>
		public virtual IEnumerable<VocationDescription> Childs { get; set; }
		/// <summary>
		/// 单位名称
		/// </summary>
		public string CompanyFullName { get; set; }
		/// <summary>
		/// 单位本级人员数量
		/// </summary>
		public int MembersCount { get; set; }
		public virtual IEnumerable<ApplyInfo.Apply> Applies { get; set; }
		//单位人员总数
		//休满假数（率）
		//应（实）休总天数
		//未休满假数（率）
		//休假率低于60%数
	}

}
