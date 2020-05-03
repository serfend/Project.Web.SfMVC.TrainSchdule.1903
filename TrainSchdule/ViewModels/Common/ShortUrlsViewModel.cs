using BLL.Helpers;
using DAL.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.Common
{
	/// <summary>
	///
	/// </summary>
	public class ShortUrlsViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public ShortUrlsDataModel Data { get; set; }
	}

	/// <summary>
	/// 短网址单个
	/// </summary>
	public class ShortUrlViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public ShortUrlCreateDataModel Data { get; set; }
	}

	/// <summary>
	/// 创建短网址实体
	/// </summary>
	public class ShortUrlCreateDataModel
	{
		/// <summary>
		/// 短链，可不传
		/// </summary>
		public string Key { get; set; }

		/// <summary>
		/// 目标
		/// </summary>
		public string Target { get; set; }

		/// <summary>
		/// 创建人，系统生成
		/// </summary>
		public string CreateBy { get; set; }

		/// <summary>
		/// 创建时间，系统生成
		/// </summary>
		public DateTime Create { get; set; }

		/// <summary>
		/// 过期期限
		/// </summary>
		public DateTime Expire { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class ShortUrlsDataModel
	{
		/// <summary>
		/// 当前查询
		/// </summary>
		public IEnumerable<ShortUrlCreateDataModel> List { get; set; }

		/// <summary>
		/// 总数
		/// </summary>
		public int TotalCount { get; set; }
	}

	/// <summary>
	/// 范围内短网址统计
	/// </summary>
	public class ShortUrlStatisticsViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public ShortUrlStatisticsDataModel Data { get; set; }
	}

	/// <summary>
	/// 短网址统计
	/// </summary>
	public class ShortUrlStatisticsDataModel
	{
		/// <summary>
		/// 查询实体
		/// </summary>
		public ShortUrlCreateDataModel ShortUrl { get; set; }

		/// <summary>
		/// 查询范围中访问情况
		/// </summary>
		public IEnumerable<ShortUrlStatisticsSingleDataModel> Statistics { get; set; }

		/// <summary>
		///
		/// </summary>
		public int TotalCount { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class ShortUrlStatisticsSingleDataModel : ICreateClientInfo
	{
		/// <summary>
		/// 访客
		/// </summary>
		public string ViewBy { get; set; }

		/// <summary>
		/// 访问时间
		/// </summary>
		public DateTime Create { get; set; }

		/// <summary>
		/// 访问ip
		/// </summary>
		public string Ip { get; set; }

		/// <summary>
		/// 访问设备
		/// </summary>
		public string Device { get; set; }

		/// <summary>
		/// 访问ua
		/// </summary>
		public string UA { get; set; }
	}
}