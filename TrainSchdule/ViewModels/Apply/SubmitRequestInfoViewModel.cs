﻿using DAL.Entities.ApplyInfo;
using DAL.Entities.ApplyInfo.DailyApply;
using DAL.Entities.Vacations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrainSchdule.ViewModels.Apply
{
	/// <summary>
	/// 基础请求
	/// </summary>
	public class SubmitRequestInfoBaseViewModel{

		/// <summary>
		/// 用户id
		/// </summary>
		[Required(ErrorMessage = "用户ID未填写")]
		public string Id { get; set; }

		/// <summary>
		/// 交通工具 0:无 1:汽车 2:火车 3:飞机 -1：其他
		/// </summary>
		[Required(ErrorMessage = "乘坐交通工具未填写")]
		public Transportation ByTransportation { get; set; }

		/// <summary>
		/// 离队时间
		/// </summary>
		[Required(ErrorMessage = "离队时间未填写")]
		public DateTime? StampLeave { get; set; }

		/// <summary>
		/// 休假去向
		/// </summary>
		[Required(ErrorMessage = "去向未填写")]
		public string VacationPlace { get; set; }

		/// <summary>
		/// 休假详细地址
		/// </summary>
		public string VacationPlaceName { get; set; }

		/// <summary>
		/// 休假原因
		/// </summary>
		public string Reason { get; set; }

	}
	/// <summary>
	/// 请假请求提交
	/// </summary>
	public class SubmitIndayRequestInfoViewModel: SubmitRequestInfoBaseViewModel
    {

		/// <summary>
		/// 离队时间
		/// </summary>
		[Required(ErrorMessage = "归队时间未填写")]
		public DateTime? StampReturn { get; set; }
		/// <summary>
		/// 请假类别
		/// </summary>
		[Required(ErrorMessage = "请假类别未填写")]
		public string RequestType { get; set; }
	}
	/// <summary>
	/// 休假请求提交
	/// </summary>
	public class SubmitRequestInfoViewModel: SubmitRequestInfoBaseViewModel
	{
		/// <summary>
		/// 休假时长
		/// </summary>
		[Required(ErrorMessage = "休假时长未填写")]
		public int VacationLength { get; set; }

		/// <summary>
		/// 路途时间
		/// </summary>
		[Required(ErrorMessage = "路途时间未填写")]
		public int OnTripLength { get; set; }

		/// <summary>
		/// 休假类型
		/// </summary>
		[Required(ErrorMessage = "休假类别未填写")]
		public string VacationType { get; set; }

		/// <summary>
		/// 是否是休假计划
		/// </summary>
		[Required(ErrorMessage = "未选择填报类型")]
		public bool? IsPlan { get; set; }
		/// <summary>
		/// 用户需要休的福利假列表
		/// </summary>
		public IEnumerable<VacationAdditional> VacationAdditionals { get; set; }
		/// <summary>
		/// 用户指定 id,length
		/// </summary>
		[Required(ErrorMessage = "用户指定未填写")]
		public Dictionary<int, int> LawVacationSet { get; set; }
	}
}