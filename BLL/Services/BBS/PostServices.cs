using BLL.Interfaces.BBS;
using DAL.Data;
using DAL.Entities.BBS;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services.BBS
{
	public class PostServices : IPostServices
	{
		private readonly ApplicationDbContext _context;

		public PostServices(ApplicationDbContext context)
		{
			_context = context;
		}

		public PostContent CreateContent(User createBy, Post targetPost, User ReplyTo, string title,string content)
		{
			var model = new PostContent()
			{
				ReplySubject = targetPost,
				Create=DateTime.Now,
				Contents = content,
				CreateBy=createBy,
				ReplyTo=ReplyTo,
				Title=title
			};
			_context.PostContents.Add(model);
			_context.SaveChanges();
			return model;
		}

		public Post CreatePost(PostContent content)
		{
			if (content == null) return null;
			var post = (Post)content;
			post.ChildContents = new List<PostContent>();
			_context.Posts.Add(post);
			_context.SaveChanges();
			return post;
		}
	}
}
