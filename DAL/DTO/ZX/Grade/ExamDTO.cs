using DAL.Entities.ZX.Grade;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.DTO.ZX.Grade
{
	/// <summary>
	/// 考核
	/// </summary>
	public class ExamDTO
	{
		/// <summary>
		/// 名称
		/// </summary>
		[Required]
		public string Name { get; set; }

		/// <summary>
		/// 描述
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// 举办单位
		/// </summary>
		[Required]
		public string HoldBy { get; set; }

		/// <summary>
		/// 创建人
		/// </summary>

		public string CreateBy { get; set; }

		/// <summary>
		/// 考核处理人
		/// </summary>
		[Required]
		public string HandleBy { get; set; }

		/// <summary>
		/// 举办时间
		/// </summary>
		[Required]
		public DateTime? ExecuteTime { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? Create { get; set; }

		/// <summary>
		/// 是否删除
		/// </summary>
		public bool IsRemoved { get; set; }
	}

	public static class ExamDTOExtensions
	{
		public static ExamDTO ToDTO(this GradeExam model) => new ExamDTO()
		{
			Create = model.Create,
			CreateBy = model.CreateById,
			Description = model.Description,
			ExecuteTime = model.ExecuteTime,
			HandleBy = model.HandleBy?.Id,
			HoldBy = model.HoldBy?.Code,
			IsRemoved = model.IsRemoved,
			Name = model.Name
		};
	}
}