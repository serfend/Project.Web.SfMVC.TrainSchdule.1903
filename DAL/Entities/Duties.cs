using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
	public class Duties 
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int Code { get; set; }
		/// <summary>
		/// 职务的名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 是否是主官
		/// </summary>
		public bool IsMajorManager { get; set; }
	}
}
