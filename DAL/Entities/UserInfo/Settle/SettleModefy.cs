using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Entities.UserInfo.Settle
{
	public class AppUsersSettleModefyRecord : BaseEntity
	{
		/// <summary>
		/// 不使用id
		/// </summary>
		[NotMapped]
		[JsonIgnore]
		public override Guid Id { get; set; }

		[Key]
		public int Code { get; set; }

		/// <summary>
		/// 长度
		/// </summary>
		public double Length { get; set; }

		/// <summary>
		/// 生效时间
		/// </summary>
		public DateTime UpdateDate { get; set; }

		/// <summary>
		/// 本次变动后产生的描述内容
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// 是否是新年初始化的数据
		/// </summary>
		public bool IsNewYearInitData { get; set; }
	}
}