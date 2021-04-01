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
		public QueryByGuid Id { get; set; }
		public QueryByPage Pages { get; set; }

		/// <summary>
		/// 申请的状态
		/// </summary>
		public QueryByIntOrEnum Status { get; set; }
		/// <summary>
		/// 申请的主状态
		/// </summary>
		public QueryByIntOrEnum MainStatus { get; set; }
		/// <summary>
		/// 落实状态，单选
		/// </summary>

		public QueryByString ExecuteStatus { get; set; }

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
		/// 申请人状态
		/// </summary>
		public QueryByIntOrEnum UserStatus { get; set; }

		/// <summary>
		/// 申请创建人所在的单位(多选)
		/// </summary>
		public QueryByString CreateCompany { get; set; }

		/// <summary>
		/// 创建人的职务类型
		/// </summary>
		public QueryByString DutiesType { get; set; }

		/// <summary>
		/// 创建人的单位类型
		/// </summary>
		public QueryByString CompanyType { get; set; }

		/// <summary>
		/// 单位状态
		/// </summary>
		public QueryByIntOrEnum CompanyStatus { get; set; }

		/// <summary>
		/// 具有审批此申请权限的审批人
		/// </summary>
		public QueryByString AuditBy { get; set; }

		/// <summary>
		/// 当前审批人
		/// </summary>
		public QueryByString NowAuditBy { get; set; }

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