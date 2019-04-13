using System;
using System.Collections.Generic;
using System.Text;
using TrainSchdule.DAL.Entities;

namespace DAL.Entities
{
	public class Apply: BaseEntity
	{
		/// <summary>
		/// 休假申请
		/// </summary>
		public ApplyRequest Request { get; set; }
		/// <summary>
		/// 休假类别
		/// </summary>
		public XJLB xjlb { get; set; }
		/// <summary>
		/// 附加记录
		/// </summary>
		public Stamp stamp { get; set; }

	}
	/// <summary>
	/// 附加记录
	/// </summary>
	public class Stamp
	{
		public long ldsj;
	}
	/// <summary>
	/// 休假类别
	/// </summary>
	public enum XJLB
	{
		未知=0,
		正常休假=10,
		探亲=11,
		事假=20,
		结婚=21
	}
}
