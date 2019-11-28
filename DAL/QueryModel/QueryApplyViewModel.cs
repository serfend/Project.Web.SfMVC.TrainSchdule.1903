using DAL.DTO.Apply;
using DAL.DTO.User;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.QueryModel
{
	/// <summary>
	/// 查询申请列表接口
	/// </summary>
	public class QueryApplyDataModel
	{
		public QueryByPage Pages { get; set; }
		/// <summary>
		/// 申请的状态
		/// </summary>
		public QueryByIntOrEnum Status { get; set; }
		/// <summary>
		/// 申请的创建时间
		/// </summary>
		public QueryByDate Create { get; set; }
		/// <summary>
		/// 何人休假
		/// </summary>
		public QueryByString CreateFor { get; set; }
		/// <summary>
		/// 申请的创建人
		/// </summary>
		public QueryByString CreateBy { get; set; }
		/// <summary>
		/// 申请创建人所在的单位
		/// </summary>
		public QueryByString CreateCompany { get; set; }
		/// <summary>
		/// 具有审批此申请权限的审批人
		/// </summary>
		public QueryByString AuditBy { get; set; }
		/// <summary>
		/// 具有审批此申请权限的审批人所在单位
		/// </summary>
		public QueryByString AuditByCompany { get; set; }
		/// <summary>
		/// 休假开始时间
		/// </summary>
		public QueryByDate StampLeave { get; set; }
		/// <summary>
		/// 休假结束时间
		/// </summary>
		public QueryByDate StampReturn { get; set; }
	}
}
