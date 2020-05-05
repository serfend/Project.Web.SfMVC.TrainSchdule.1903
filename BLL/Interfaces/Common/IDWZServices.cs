using DAL.Entities.Common;
using DAL.Entities.UserInfo;
using DAL.QueryModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.Common
{
	public interface IDWZServices
	{
		/// <summary>
		/// 创建一个短网址
		/// </summary>
		/// <param name="createBy">创建人</param>
		/// <param name="target">目标网址</param>
		/// <param name="Expire">过期时间</param>
		/// <param name="key">短网址，为空时系统生成</param>
		/// <returns></returns>
		Task<ShortUrl> Create(User createBy, string target, DateTime Expire, string key);

		/// <summary>
		/// 查询原始网址
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<Tuple<IEnumerable<ShortUrl>, int>> Query(QueryDwzViewModel model);

		/// <summary>
		/// 获取单个短网址
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		Task<ShortUrl> Load(string key);

		/// <summary>
		/// 移除短网址
		/// </summary>
		/// <param name="model"></param>
		Task Remove(ShortUrl model);

		/// <summary>
		/// 访问短网址
		/// </summary>
		/// <param name="model"></param>
		/// <param name="ViewUser">来访者，可为空</param>
		Task<string> Open(ShortUrl model, User ViewUser);

		/// <summary>
		/// 查询短链接统计
		/// </summary>
		/// <param name="shortUrl"></param>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<Tuple<IEnumerable<ShortUrlStatistics>, int>> QueryStatistics(ShortUrl shortUrl, QueryDwzStatisticsViewModel model);
	}
}