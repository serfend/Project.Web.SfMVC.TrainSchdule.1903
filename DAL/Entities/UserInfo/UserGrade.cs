using System;

namespace DAL.Entities.UserInfo
{
	public class UserGrade : BaseEntityGuid
	{
		/// <summary>
		/// 成绩创建时间
		/// </summary>
		public DateTime Create { get; set; }
	}
}