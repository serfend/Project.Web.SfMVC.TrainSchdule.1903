using BLL.Helpers;
using DAL.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Common
{
	/// <summary>
	///
	/// </summary>
	public class ApplicationUpdateRecordViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public ApplicationUpdateRecordDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class ApplicationUpdateRecordDataModel
	{
		/// <summary>
		///
		/// </summary>
		public IEnumerable<ApplicationUpdateRecord> List { get; set; }

		/// <summary>
		///
		/// </summary>
		public int TotalCount { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class ApplicationUpdateRecordUpdateViewModel : GoogleAuthViewModel
	{
		/// <summary>
		///
		/// </summary>
		[Required]
		public ApplicationUpdateRecordUpdateDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class ApplicationUpdateRecordUpdateDataModel
	{
		/// <summary>
		///
		/// </summary>
		public IEnumerable<SingleRecord> List { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class SingleRecord
	{
		/// <summary>
		/// 主键 版本号
		/// </summary>
		[Required]
		public string Version { get; set; }

		/// <summary>
		/// 版本描述
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime Create { get; set; }

		/// <summary>
		/// 是否删除
		/// </summary>
		public bool IsRemoved { get; set; }
	}

	/// <summary>
	///
	/// </summary>

	public static class UpdateVersionExtensions
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <param name="raw"></param>
		/// <returns></returns>
		public static ApplicationUpdateRecord ToModel(this ApplicationUpdateRecordUpdateDataModel model, ApplicationUpdateRecord raw)
		{
			if (model == null) return null;
			if (raw == null) raw = new ApplicationUpdateRecord();
			raw.Create = model.Create;
			raw.Description = model.Description;
			raw.IsRemoved = model.IsRemoved;
			raw.Version = model.Version;
			return raw;
		}
	}
}