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
		/// 优先级 0-255 升序
		/// </summary>
		public byte Priority { get; set; }
		/// <summary>
		/// 全局优先值 自动计算
		/// </summary>
		public uint PrioritySum { get; set; }

		/// <summary>
		/// 单位标识，用于进行分组，以##分割
		/// </summary>
		public string Tag { get; set; }

		public string Description { get; set; }

		/// <summary>
		/// 单位地点
		/// </summary>
		public virtual AdminDivision Location { get; set; }

		/// <summary>
		/// 单位属性
		/// </summary>
		public CompanyStatus CompanyStatus { get; set; }
	}

	public enum CompanyStatus
	{
		IsRemote = 1,
		IsPrivate = 2,
		IgnoreCaculateStatistics = 4,
		IsRemoved = 8
	}
}