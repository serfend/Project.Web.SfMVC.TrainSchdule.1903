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
		public IActionResult Comment(Guid id, int pageIndex = 0, int pageSize = 20, string order = null)
		{
			var list_raw = _context.ApplyCommentsDb.Where(a => a.Apply.Id == id);
			Tuple<IQueryable<ApplyComment>, int> list;
			if ("as_date" == order) list = list_raw.OrderByDescending(a => a.Create).SplitPage(pageIndex, pageSize);
			else list = list_raw.OrderByDescending(a => a.Likes).SplitPage(pageIndex, pageSize);
			// TODO if need to extend , then put it to services
			var db = _context.ApplyCommentLikes.AsQueryable();
			var currentUser = _currentUserService.CurrentUser?.Id;
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
			if (model.Id != null)
				m = _context.ApplyCommentsDb.FirstOrDefault(i => i.Id == model.Id);
			var actionUser = _currentUserService.CurrentUser;
			if (actionUser == null) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
			var apply =model.IsRemove?null:  _applyService.GetById(model.Apply);
			if (apply == null&&!model.IsRemove) return new JsonResult(ActionStatusMessage.ApplyMessage.NotExist);
			var act = model.IsRemove ? "删除" : "添加";
			var ua = _userActionServices.Log(UserOperation.AttachInfoToApply, actionUser.Id, $"{act}假期{apply?.Id??m?.Apply?.Id}的评论");
			if (m == null)
			{
				if (model.IsRemove) return new JsonResult(_userActionServices.LogNewActionInfo(ua, ActionStatusMessage.StaticMessage.ResourceNotExist));
				else
				{
					m = new ApplyComment()
					{
						Content = model.Content,
						From = actionUser,
						Apply = apply,
						Create = DateTime.Now
					};
					_context.ApplyComments.Add(m);
				}
			}
			else
			{
				var permission = actionUser.Id == m.From.Id || _userActionServices.Permission(actionUser.Application.Permission, DictionaryAllPermission.Apply.AttachInfo, model.IsRemove ? Operation.Remove : Operation.Update, actionUser.Id, m.From.CompanyInfo.Company.Code);
				if (permission)
				{
					m.LastModify = DateTime.Now;
					m.ModifyBy = actionUser;
					if (model.IsRemove)
						m.Remove();
					else
					{
						m.Content = model.Content;
						_context.ApplyComments.Update(m);
					}
				}
			}
			_context.SaveChanges();
			_userActionServices.Status(ua, true);
			return new JsonResult(new EntityViewModel<ApplyCommentVDataModel>(m.ToDataModel(_context.ApplyCommentLikes, actionUser.Id)));
		}

		/// <summary>
		/// 点赞/取消
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult CommentLike([FromBody] ApplyCommentLikeDataModel model)
		{
			var currentUser = _currentUserService.CurrentUser;
			if (currentUser == null) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
			var comment = _context.ApplyCommentsDb.FirstOrDefault(i => i.Id == model.Id);
			if (comment == null) return new JsonResult(ActionStatusMessage.StaticMessage.ResourceNotExist);
			var like = _context.ApplyCommentLikes.Where(i => i.Comment.Id == comment.Id).FirstOrDefault(i => i.CreateBy.Id == currentUser.Id);
			if (like == null && model.Like) _context.ApplyCommentLikes.Add(new ApplyCommentLike() { Comment = comment, Create = DateTime.Now, CreateBy = currentUser });
			else if (like != null && !model.Like) _context.ApplyCommentLikes.Remove(like);
			else return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			comment.Likes += model.Like ? 1 : -1;
			_context.ApplyComments.Update(comment);
			_context.SaveChanges();
			return new JsonResult(ActionStatusMessage.Success);
		}
	}
}