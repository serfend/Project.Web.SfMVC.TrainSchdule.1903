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

		public CompanyLevel Level { get; set; }
		public bool IsPrivate { get; set; }
		public string Description { get; set; }
	}
	public enum CompanyLevel
	{
		JW,
		DJQ,
		FJQ,
		ZJ,
		FJ,
		ZS,
		FS,
		ZT,
		FT,
		ZY,
		FY,
		ZL,
		FL,
		ZP,
		None
	}
}
