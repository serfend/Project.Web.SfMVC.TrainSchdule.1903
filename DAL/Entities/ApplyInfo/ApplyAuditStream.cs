using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Entities.ApplyInfo
{
	public class ApplyAuditStreamSolutionRule : MembersFilter
	{
		[Key]
		public Guid Id { get; set; }

		/// <summary>
		/// 规则名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 规则描述
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// 规则的优先级
		/// </summary>
		public int Priority { get; set; }

		/// <summary>
		/// 规则是否启用
		/// </summary>
		public bool Enable { get; set; }

		/// <summary>
		/// 规则对应的方案
		/// </summary>
		public virtual ApplyAuditStream Solution { get; set; }

		public DateTime Create { get; set; }
	}

	/// <summary>
	/// 审批流方案
	/// </summary>
	public class ApplyAuditStream
	{
		[Key]
		public Guid Id { get; set; }

		/// <summary>
		/// 审批方案名称
		/// </summary>
		public string Name { get; set; }

		public string Description { get; set; }

		/// <summary>
		/// 审批流全流节点,以##分割表示多个
		/// </summary>
		public string Nodes { get; set; }

		public DateTime Create { get; set; }
	}

	/// <summary>
	/// 单个审批节点，通过Filter选出需要进行审批的成员
	/// </summary>
	public class ApplyAuditStreamNodeAction : MembersFilter
	{
		[Key]
		public Guid Id { get; set; }

		/// <summary>
		/// 审批节点的名称
		/// </summary>
		///
		public string Name { get; set; }

		/// <summary>
		/// 说明
		/// </summary>
		public string Description { get; set; }

		public DateTime Create { get; set; }
	}

	/// <summary>
	/// 通过条件设置，筛选出符合条件的人
	/// </summary>
	public class MembersFilter
	{
		/// <summary>
		/// 职务范围,int类型，以##分割
		/// </summary>
		public virtual string Duties { get; set; }

		/// <summary>
		/// 职务类型范围，以##分割
		/// </summary>
		public string DutiesTags { get; set; }

		/// <summary>
		/// 职务主官选择
		/// </summary>
		public DutiesIsMajor DutyIsMajor { get; set; }

		/// <summary>
		/// 单位范围，以##分割
		/// </summary>
		public virtual string Companies { get; set; }

		/// <summary>
		/// 可以设置为self或parent或null，当设置非null，则Companies字段失效
		/// </summary>
		public string CompanyRefer { get; set; }

		/// <summary>
		/// 可以设置单位类型，用于区分各类单位，以##分割
		/// </summary>
		public string CompanyTags { get; set; }

		/// <summary>
		/// 可以设置单位代码长度进行筛选，int类型 以##分割
		/// </summary>
		public string CompanyCodeLength { get; set; }

		/// <summary>
		/// 设置本节点需要审批的成员数量
		/// </summary>
		public int AuditMembersCount { get; set; }

		/// <summary>
		/// 精确设置需要审批的人，当设置此属性，其他设置均失效，以##分割
		/// </summary>
		public string AuditMembers { get; set; }
	}

	public enum DutiesIsMajor
	{
		BothCanGo = 0,
		OnlyUnMajor = 1,
		OnlyMajor = 2
	}
}