using DAL.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.BBS
{
	/// <summary>
	/// 创建动态
	/// </summary>
	public class PostCreateDataModel
	{
		/// <summary>
		/// 动态标题/可为空
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// 动态内容
		/// </summary>
		public string Content { get; set; }
	}

	/// <summary>
	/// 查询动态
	/// </summary>
	public class PostQueryDataModel
	{
		/// <summary>
		/// 创建人用户名
		/// </summary>
		public QueryByString CreateBy { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		public QueryByDate Create { get; set; }

		/// <summary>
		/// 分页
		/// </summary>
		public QueryByPage Pages { get; set; }

		/// <summary>
		/// 内容
		/// </summary>
		public QueryByString Content { get; set; }

		/// <summary>
		/// 点赞数量
		/// </summary>
		public QueryByIntOrEnum Likes { get; set; }
	}
}