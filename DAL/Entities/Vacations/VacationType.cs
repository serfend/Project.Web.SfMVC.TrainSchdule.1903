using DAL.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.Vacations
{
	public interface IVacationType: IRegion
	{

		/// <summary>
		/// 假期名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 别名
		/// </summary>
		public string Alias { get; set; }

		/// <summary>
		/// 描述信息
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// 是否禁用（前端隐藏）
		/// </summary>
		public bool Disabled { get; set; }

		/// <summary>
		/// 背景图文件名
		/// </summary>
		public string Background { get; set; }
	}
	public class VacationTypeBase: BaseEntityInt,IVacationType
    {

		/// <summary>
		/// 假期名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 别名
		/// </summary>
		public string Alias { get; set; }

		/// <summary>
		/// 描述信息
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// 是否禁用（前端隐藏）
		/// </summary>
		public bool Disabled { get; set; }

		/// <summary>
		/// 背景图文件名
		/// </summary>
		public string Background { get; set; }
        public string RegionOnCompany { get; set; }
    }
	public class VacationIndayType: VacationTypeBase {
		/// <summary>
		/// 允许跨天天数
		/// </summary>
		public int PermitCrossDay { get; set; }
		/// <summary>
		/// TODO 疫情定制 - 请假是否需要跟踪出行方式
		/// </summary>
		public bool NeedTrace { get; set; }
		/// <summary>
		/// 默认申请时间，使用js回调
		/// js:(now)=>start,end
		/// </summary>
		public string DefaultDateRange { get; set; }
	}
	public class VacationType : VacationTypeBase 
	{
		/// <summary>
		/// 最长长度
		/// </summary>
		public int MaxLength { get; set; }

		/// <summary>
		/// 最短长度
		/// </summary>
		public int MinLength { get; set; }

		/// <summary>
		/// 是否计入主假期
		/// </summary>
		public bool Primary { get; set; }

		/// <summary>
		/// 是否允许在正休未休完前休
		/// </summary>
		public bool AllowBeforePrimary { get; set; }

		/// <summary>
		/// 是否计算福利假
		/// </summary>
		public bool CaculateBenefit { get; set; }

		/// <summary>
		/// 是否可休路途
		/// </summary>
		public bool CanUseOnTrip { get; set; }

		/// <summary>
		/// TODO 当不计入当年时（非主假期）是否在次年减去
		/// </summary>
		public bool MinusNextYear { get; set; }

		/// <summary>
		/// 是否禁止跨年
		/// </summary>
		public bool NotPermitCrossYear { get; set; }


	}
}