using DAL.Entities.BBS;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces.BBS
{
	public interface IPostServices
	{
		/// <summary>
		/// 创建一个动态
		/// </summary>
		/// <param name="content"></param>
		/// <returns></returns>
		Post CreatePost(PostContent content);

		/// <summary>
		/// 创建一个回复
		/// </summary>
		/// <param name="createBy"></param>
		/// <param name="targetPost"></param>
		/// <param name="ReplyTo"></param>
		/// <param name="title"></param>
		/// <param name="content"></param>
		/// <returns></returns>
		PostContent CreateReply(User createBy, Post targetPost, User ReplyTo, string title, string content);

		/// <summary>
		/// 删除回复/动态
		/// </summary>
		/// <param name="target"></param>
		void RemoveReply(PostContent target);

		/// <summary>
		/// 点/取消 赞     回复/动态
		/// </summary>
		/// <param name="target"></param>
		void LikeContent(PostContent target, User CreateBy, bool LikeAction);
	}
}