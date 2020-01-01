﻿using BLL.Helpers;
using DAL.Entities;
using DAL.Entities.Duty;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.Company
{
	/// <summary>
	/// 职务列表
	/// </summary>
	public class DutiesViewModel:ApiResult
	{
		/// <summary>
		/// 
		/// </summary>
		public DutiesDataModel Data { get; set; }
	}
	/// <summary>
	/// 返回职务列表
	/// </summary>
	public class DutiesDataModel
	{
		/// <summary>
		/// 当前查询的分页中的数据
		/// </summary>
		public IEnumerable<DutyDataModel> List { get; set; }
		/// <summary>
		/// 当前查询的总数
		/// </summary>
		public int TotalCount { get; set; }
	}
	/// <summary>
	/// 
	/// </summary>
	public class DutyViewModel:ApiResult
	{
		/// <summary>
		/// 
		/// </summary>
		public DutyDataModel Data { get; set; }
	}
	/// <summary>
	/// 职务
	/// </summary>
	public class DutyDataModel
	{
		/// <summary>
		/// 职务代码
		/// </summary>
		public int Code { get; set; }
		/// <summary>
		/// 职务的名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 是否是主官
		/// </summary>
		public bool IsMajorManager { get; set; }
		public DutyTypeDataModel DutiesType { get; set; }
		public DutiesRawType DutyType { get; set; }
	}
	/// <summary>
	/// 职务类型
	/// </summary>
	public class DutyTypeDataModel
	{
		public int Code { get; set; }
		/// <summary>
		/// 职务类型名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 需要审批的层级数量，当为0时则按默认 zs=2，gb=3
		/// </summary>
		public int AuditLevelNum { get; set; }
		/// <summary>
		/// 职务原始类型
		/// </summary>
		public DutiesRawType DutiesRawType { get; set; }
	}
	
		public class UserTitleCompareer : IEqualityComparer<UserCompanyTitle>
	{
		private static UserTitleCompareer titleEqualComparer;
		public static UserTitleCompareer GetInstance()
		{
			if (titleEqualComparer == null) titleEqualComparer = new UserTitleCompareer();
			return titleEqualComparer;
		}
		public bool Equals(UserCompanyTitle x, UserCompanyTitle y)
		{
			return x.Name == y.Name;
		}

		public int GetHashCode(UserCompanyTitle obj)
		{
			return obj.Name.GetHashCode();
		}
	}
	public class DutiesEqualComparer : IEqualityComparer<Duties>
	{
		private static DutiesEqualComparer dutiesEqualComparer;
		public static DutiesEqualComparer GetInstance()
		{
			if (dutiesEqualComparer == null) dutiesEqualComparer = new DutiesEqualComparer();
			return dutiesEqualComparer;
		}
		public bool Equals(Duties x, Duties y)
		{
			return x.Name == y.Name;
		}

		public int GetHashCode(Duties obj)
		{
			return obj.Name.GetHashCode();
		}
	}
}