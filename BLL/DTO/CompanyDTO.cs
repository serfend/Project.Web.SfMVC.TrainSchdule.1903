using System;
using System.Collections.Generic;
using System.Text;

namespace TrainSchdule.BLL.DTO
{
	public class CompanyDTO
	{
		public string Code { get; set; }
		public string Name { get; set; }
		public string Path { get; set; }
		public bool IsPrivate { get; set; }
		/// <summary>
		/// 此单位包含的用户
		/// </summary>
		public  IEnumerable<UserDTO> Members { get; set; }
	}
}
