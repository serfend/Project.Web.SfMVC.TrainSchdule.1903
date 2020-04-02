using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities.UserInfo
{
	public class UserID
	{
		/// <summary>
		/// 用户名
		/// </summary>
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		[Key]
		public string Id { get; set; }
	}
}