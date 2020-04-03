﻿using DAL.Entities.ApplyInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.DTO.Apply
{
	public class MembersFilterDto
	{
		/// <summary>
		/// 职务范围
		/// </summary>
		public IEnumerable<int> Duties { get; set; }

		/// <summary>
		/// 职务主官选择
		/// </summary>
		public DutiesIsMajor DutyIsMajor { get; set; }

		/// <summary>
		/// //TODO 职务类型范围
		/// </summary>
		public IEnumerable<string> DutyTypes { get; set; }

		/// <summary>
		/// 单位范围
		/// </summary>
		public IEnumerable<string> Companies { get; set; }

		/// <summary>
		/// 可以设置为self或parent或null，当设置非null，则Companies字段失效
		/// </summary>
		public string CompanyRefer { get; set; }

		/// <summary>
		/// 可以设置单位类型，用于区分各类单位，以##分割
		/// </summary>
		public IEnumerable<string> CompanyTags { get; set; }

		/// <summary>
		/// 可以设置单位代码长度进行筛选，int类型 以##分割
		/// </summary>
		public IEnumerable<int> CompanyCodeLength { get; set; }

		/// <summary>
		/// 设置本节点需要审批的成员数量
		/// </summary>
		public int AuditMembersCount { get; set; }

		/// <summary>
		/// 精确设置需要审批的人，当设置此属性，其他设置均失效
		/// </summary>
		public IEnumerable<string> AuditMembers { get; set; }
	}
}