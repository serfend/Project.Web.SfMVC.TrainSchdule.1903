using System;
using System.Collections.Generic;
using System.Text;
using TrainSchdule.DAL.Entities.UserInfo;

namespace TrainSchdule.BLL.DTO
{
	public class PermissionCompanyDTO:BaseEntity
	{
		/// <summary>
		/// 允许操作的路径
		/// </summary>
		public string Path { get; set; }
		public Guid AuthBy { get; set; }
	}
}
