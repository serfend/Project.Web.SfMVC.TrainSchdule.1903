using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{

	public class Company
	{
		public string Name { get; set; }

		public bool IsPrivate { get; set; }
		/// <summary>
		/// 单位代码
		/// </summary>
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		[Key]
		public string Code{get; set;}
	}
		
}
