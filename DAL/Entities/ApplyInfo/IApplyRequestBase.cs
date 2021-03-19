using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
   public  interface IApplyRequestBase
    {
		public DateTime? StampLeave { get; set; }
		public DateTime? StampReturn { get; set; }
		/// <summary>
		/// 休假地点
		/// </summary>
		public AdminDivision VacationPlace { get; set; }

		/// <summary>
		/// 休假地点（详细地址）
		/// </summary>
		public string VacationPlaceName { get; set; }

		/// <summary>
		/// 休假原因（用户设置）
		/// </summary>
		public string Reason { get; set; }
	}
}
