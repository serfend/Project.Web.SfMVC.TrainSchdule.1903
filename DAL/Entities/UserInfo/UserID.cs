using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities.UserInfo
{
	public class UserID
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		[Key]
		public string Id { get; set; }
	}
}
