using System;
using System.Collections.Generic;
using System.Text;

namespace TrainSchdule.BLL.DTO
{
	public class CompanyDTO
	{

		public string Name { get; set; }
		public string Path { get; set; }
		/// <summary>
		/// 此单位是否是当前父单位
		/// </summary>
		public bool IsParent {get; set; }
		/// <summary>
		/// 此单位包含的用户
		/// </summary>
		public  IEnumerable<UserDTO> Members { get; set; }
	}
}
