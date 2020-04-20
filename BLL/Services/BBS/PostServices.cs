using BLL.Interfaces.BBS;
using DAL.Data;
using DAL.Entities.BBS;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
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

		public PostContent CreateReply(User createBy, Post targetPost, User ReplyTo, string title, string content)
		{
			var model = new PostContent()
			{
				ReplySubject = targetPost,
				Create = DateTime.Now,
				Contents = content,
				CreateBy = createBy,
				ReplyTo = ReplyTo,
				Title = title
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

		public void RemoveReply(PostContent target)
		{
			var reply = _context.PostContentsDb.Where(p => p.Id == target.Id).FirstOrDefault();
			if (reply == null) return;
			reply.Remove();
			_context.PostContents.Update(reply);
			_context.SaveChanges();
		}

		public void LikeContent(PostContent target, User CreateBy, bool LikeAction)
		{
			var like = _context.PostLikes.Where(l => l.Content.Id == target.Id).FirstOrDefault();
			if (like == null && LikeAction)
			{
				like = new Like()
				{
					CreateBy = CreateBy,
					Content = target,
					Create = DateTime.Now,
				};
				_context.PostLikes.Add(like);
			}
			else if (like != null && !LikeAction)
			{
				_context.PostLikes.Remove(like);
			}
			else return;
			_context.SaveChanges();
		}
	}
}