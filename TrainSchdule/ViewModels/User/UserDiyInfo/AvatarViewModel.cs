using BLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.ViewModels.User
{
	/// <summary>
	/// 返回头像
	/// </summary>
	public class AvatarViewModel : ApiResult
	{
		/// <summary>
		/// 头像信息
		/// </summary>
		public AvatarDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class AvatarListViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public AvatarListDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class AvatarListDataModel
	{
		/// <summary>
		///
		/// </summary>
		public IEnumerable<AvatarDataModel> List { get; set; }
	}

	/// <summary>
	/// 返回头像
	/// </summary>
	public class AvatarDataModel : ResponseImgDataModel
	{
		/// <summary>
		/// 头像创建时间
		/// </summary>
		public DateTime Create { get; set; }

		/// <summary>
		/// 头像路径
		/// </summary>
		public string Path { get; set; }
	}

	/// <summary>
	/// 返回图片
	/// </summary>
	public class ResponseImgDataModel
	{
		/// <summary>
		/// 图片链接
		/// </summary>
		public string Url { get; set; }
	}
}