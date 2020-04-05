using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
	public class Company
	{
		/// <summary>
		/// 单位代码
		/// </summary>
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		[Key]
		public string Code { get; set; }

		public string Name { get; set; }

		/// <summary>
		/// 单位标识，用于进行分组，以##分割
		/// </summary>
		public string Tag { get; set; }

		public bool IsPrivate { get; set; }
		public string Description { get; set; }
	}
}