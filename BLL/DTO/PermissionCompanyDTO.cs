using System;
using System.Collections.Generic;
using System.Text;

namespace TrainSchdule.BLL.DTO
{
	public class PermissionCompanyDTO
	{
		public Guid id { get; set; }
		/// <summary>
		/// 允许操作的路径
		/// </summary>
		public string Path;

		public UserDTO owner;
	}
}
