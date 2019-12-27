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
		PostContent CreateContent(User createBy, Post targetPost, User ReplyTo, string title, string content);
	}
}
