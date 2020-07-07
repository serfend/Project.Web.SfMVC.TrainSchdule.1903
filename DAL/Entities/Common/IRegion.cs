using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Entities.Common
{
	/// <summary>
	/// 作用域，仅单位可用
	/// </summary>
	public interface IRegion
	{
		/// <summary>
		/// 作用域
		/// </summary>
		string RegionOnCompany { get; set; }
	}
}