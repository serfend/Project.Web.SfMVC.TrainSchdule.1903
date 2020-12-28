using DAL.Entities.BBS;
using DAL.Entities.UserInfo;
using DAL.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Interfaces.BBS
{
	public interface IPostServices
	{
		/// <summary>
		/// 通过id获取动态
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		PostContent GetPostById(Guid id);
		/// <summary>
		/// 条件查询动态
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Tuple<IQueryable<PostContent>, int> QueryPost(QueryContentViewModel model);
		/// <summary>
		/// 创建一个回复
		/// </summary>
		/// <param name="targetPost"></param>
		/// <returns></returns>
		PostContent CreatePost( PostContent targetPost);

		/// <summary>
		/// 删除回复/动态
		/// </summary>
		/// <param name="target"></param>
		void RemoveContent(PostContent target);

		/// <summary>
		/// 点/取消 赞     回复/动态
		/// </summary>
		/// <param name="target"></param>
		void LikeContent(PostContent target, User CreateBy, bool LikeAction);
	}
}