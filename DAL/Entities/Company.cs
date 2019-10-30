using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{

	public class Company
	{
		public string Name { get; set; }
		/// <summary>
		/// 单位类型 连/科 组/部 部 首长
		/// </summary>
		public string CompanyTypeDesc { get; set; }
		public string CompanyParentTypeDesc { get; set; }
		public bool IsPrivate { get; set; }
		/// <summary>
		/// 单位代码
		/// </summary>
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		[Key]
		public string Code{get; set;}
	}
		
}
