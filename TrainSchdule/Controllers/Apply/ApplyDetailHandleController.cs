using BLL.Extensions;
using BLL.Extensions.ApplyExtensions;
using BLL.Extensions.Common;
using BLL.Helpers;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.Apply.ApplyDetail;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.Controllers.Apply
{
	public partial class ApplyController
	{
		/// <summary>
		/// 获取假期评论列表
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Comment(string id, int pageIndex = 0, int pageSize = 20, string order = null)
		{
			var list_raw = context.ApplyCommentsDb.Where(a => a.Apply == id);
			Tuple<IQueryable<ApplyComment>, int> list;
			if ("as_date" == order) list = list_raw.OrderByDescending(a => a.Create).SplitPage(pageIndex, pageSize);
			else list = list_raw.OrderByDescending(a => a.Likes).SplitPage(pageIndex, pageSize);
			// TODO if need to extend , then put it to services
			var db = context.ApplyCommentLikes.AsQueryable();
			var currentUser = currentUserService.CurrentUser?.Id;
			return new JsonResult(new EntitiesListViewModel<ApplyCommentVDataModel>(list.Item1.AsEnumerable().Select(i => i.ToDataModel(db, currentUser)), list.Item2));
		}

		/// <summary>
		/// 添加/删除假期评论
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult Comment([FromBody] ApplyCommentDataModel model)
		{
			ApplyComment m = null;
			if (model?.Id !=Guid.Empty)
				m = context.ApplyCommentsDb.FirstOrDefault(i => i.Id == model.Id);
			var actionUser = currentUserService.CurrentUser;
			if (actionUser == null) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
			var apply =model.IsRemove?null:  applyService.GetById(Guid.Parse(model.Apply));
			//if (apply == null&&!model.IsRemove) return new JsonResult(ActionStatusMessage.ApplyMessage.NotExist);
			var act = model.IsRemove ? "删除" : "添加";
			var ua = userActionServices.Log(UserOperation.AttachInfoToApply, actionUser.Id, $"{act}假期{apply?.Id.ToString()??m?.Apply}的评论");
			if (m == null)
			{
				if (model.IsRemove) return new JsonResult(userActionServices.LogNewActionInfo(ua, ActionStatusMessage.StaticMessage.ResourceNotExist));
				else
				{
					m = new ApplyComment()
					{
						Content = model.Content,
						From = actionUser,
						Apply = model.Apply,
						Create = DateTime.Now
					};
					context.ApplyComments.Add(m);
				}
			}
			else
			{
				var permission = actionUser.Id == m.From.Id || userActionServices.Permission(actionUser.Application.Permission, DictionaryAllPermission.Apply.AttachInfo, model.IsRemove ? Operation.Remove : Operation.Update, actionUser.Id, m.From.CompanyInfo.Company.Code);
				if (permission)
				{
					m.LastModify = DateTime.Now;
					m.ModifyBy = actionUser;
					if (model.IsRemove)
						m.Remove();
					else
					{
						m.Content = model.Content;
						context.ApplyComments.Update(m);
					}
				}
			}
			context.SaveChanges();
			userActionServices.Status(ua, true);
			return new JsonResult(new EntityViewModel<ApplyCommentVDataModel>(m.ToDataModel(context.ApplyCommentLikes, actionUser.Id)));
		}

		/// <summary>
		/// 点赞/取消
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult CommentLike([FromBody] ApplyCommentLikeDataModel model)
		{
			var currentUser = currentUserService.CurrentUser;
			if (currentUser == null) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
			var comment = context.ApplyCommentsDb.FirstOrDefault(i => i.Id == model.Id);
			if (comment == null) return new JsonResult(ActionStatusMessage.StaticMessage.ResourceNotExist);
			var like = context.ApplyCommentLikes.Where(i => i.Comment.Id == comment.Id).FirstOrDefault(i => i.CreateBy.Id == currentUser.Id);
			if (like == null && model.Like) context.ApplyCommentLikes.Add(new ApplyCommentLike() { Comment = comment, Create = DateTime.Now, CreateBy = currentUser });
			else if (like != null && !model.Like) context.ApplyCommentLikes.Remove(like);
			else return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			comment.Likes += model.Like ? 1 : -1;
			context.ApplyComments.Update(comment);
			context.SaveChanges();
			return new JsonResult(ActionStatusMessage.Success);
		}
	}
}