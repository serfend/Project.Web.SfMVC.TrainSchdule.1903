using BLL.Helpers;
using DAL.DTO.Apply;
using DAL.DTO.User;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.User;

namespace TrainSchdule.ViewModels.Apply.ApplyDetail
{
	/// <summary>
	///
	/// </summary>
	public class ApplyCommentViewModel : ApiResult
	{
	}

	/// <summary>
	/// 点赞
	/// </summary>
	public class ApplyCommentLikeDataModel
	{
		/// <summary>
		/// 评论id
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// 赞/取消
		/// </summary>
		public bool Like { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class ApplyCommentDataModel
	{
		/// <summary>
		/// 若修改/删除 则填写
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// 作用对象
		/// </summary>
		public Guid Apply { get; set; }

		/// <summary>
		/// 是否是删除
		/// </summary>
		public bool IsRemove { get; set; }

		/// <summary>
		///
		/// </summary>
		public string Content { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class ApplyCommentVDataModel
	{
		/// <summary>
		///
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		///
		/// </summary>
		public UserSummaryDto From { get; set; }

		/// <summary>
		///
		/// </summary>
		public DateTime Create { get; set; }

		/// <summary>
		///
		/// </summary>
		public string Content { get; set; }

		/// <summary>
		///
		/// </summary>
		public DateTime LastModify { get; set; }

		/// <summary>
		///
		/// </summary>
		public UserSummaryDto ModifyBy { get; set; }

		/// <summary>
		/// 作用到
		/// </summary>
		public Guid Apply { get; set; }

		/// <summary>
		/// 点赞
		/// </summary>
		public int Like { get; set; }

		/// <summary>
		/// 当前用户是否点赞
		/// </summary>
		public bool MyLike { get; set; }
	}
}