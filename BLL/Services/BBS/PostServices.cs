using BLL.Extensions.Common;
using BLL.Interfaces.BBS;
using DAL.Data;
using DAL.Entities.BBS;
using DAL.Entities.UserInfo;
using DAL.QueryModel;
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

		public PostContent CreatePost(PostContent targetPost)
		{
			_context.PostContents.Add(targetPost);
			targetPost.ReplyCount++;
			_context.PostContents.Update(targetPost);
			_context.SaveChanges();
			return targetPost;
		}


		public void RemoveContent(PostContent target)
		{
			var reply = _context.PostContentsDb.Where(p => p.Id == target.Id).FirstOrDefault();
			if (reply == null) return;
			reply.Remove();
			var c = reply.ReplySubject;
			if (c!=null)c.ReplyCount--;
			_context.PostContents.Update(reply);
			_context.SaveChanges();
		}

		public void LikeContent(PostContent target, User CreateBy, bool LikeAction)
		{
			var like = _context.PostInteracts.Where(l => l.Content.Id == target.Id).FirstOrDefault();
			if (like == null && LikeAction)
			{
				like = new PostInteractStatus()
				{
					CreateBy = CreateBy,
					Content = target,
					Create = DateTime.Now,
				};
				_context.PostInteracts.Add(like);
				target.LikeCount++;
			}
			else if (like != null && !LikeAction)
			{
				_context.PostInteracts.Remove(like);
				target.LikeCount--;
			}
			else return;
			_context.SaveChanges();
		}

        public PostContent GetPostById(Guid id)
        {
			if (id == Guid.Empty) return null;
			return _context.PostContentsDb.FirstOrDefault(i => i.Id == id);
        }

        public Tuple<IQueryable<PostContent>,int> QueryPost(QueryContentViewModel model)
        {
			var db = _context.PostContentsDb;
			if (model.Create != null)
				db = db.Where(i => i.Create >= model.Create.Start).Where(i => i.Create <= model.Create.End);
			if (model.CreateBy != null)
				db = db.Where(i => i.CreateBy.BaseInfo.RealName.Contains(model.CreateBy.Value));
			if(model.ReplyTo!=null)
				db = db.Where(i=>i.ReplyTo!=null).Where(i => i.ReplyTo.BaseInfo.RealName.Contains(model.ReplyTo.Value));
			if (model.ReplySubject != null)
				db = db.Where(i => i.ReplySubject.Id == Guid.Parse(model.ReplySubject.Value));
            if (model.Page?.PageSize <= 0) {
				model.Page = new QueryByPage();
			}
			return db.SplitPage(model.Page);
		}
	}
}