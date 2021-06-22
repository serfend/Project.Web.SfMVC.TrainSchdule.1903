using BLL.Extensions;
using BLL.Extensions.ApplyExtensions;
using BLL.Extensions.Common;
using BLL.Helpers;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.Permisstions;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.Extensions.Common;
using TrainSchdule.ViewModels.Apply.ApplyDetail;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

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
            var list_raw = context.ApplyCommentsDb.Where(a => a.Apply == id).Where(a => a.ReplyId == null);
            Tuple<IQueryable<ApplyComment>, int> list;
            if ("as_date" == order) list = list_raw.OrderByDescending(a => a.Create).SplitPage(pageIndex, pageSize);
            else list = list_raw.OrderByDescending(a => a.Likes).SplitPage(pageIndex, pageSize);
            // TODO if need to extend , then put it to services
            var db = context.ApplyCommentLikesDb;
            var dbComments = context.ApplyCommentsDb;
            var currentUser = currentUserService.CurrentUser?.Id;
            return new JsonResult(new EntitiesListViewModel<ApplyCommentVDataModel>(list.Item1.AsEnumerable().Select(i => i.ToDataModel(db, dbComments, currentUser, 1)), list.Item2));
        }

        /// <summary>
        /// 添加/删除假期评论
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Comment([FromBody] ApplyCommentViewModel model)
        {
            var actionUser = currentUserService.CurrentUser;
            if (actionUser == null) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
            var item = model.Data.ToModel(context.ApplyCommentsDb);
            var m = dataUpdateServices.Update(new EntityModifyExtensions.DataUpdateModel<ApplyComment>()
            {
                Item = item,
                Db = context.ApplyComments,
                AuthUser = model.Auth.AuthUser(authService, usersService, actionUser),
                BeforeAdd = (e) =>
                {
                    e.Create = DateTime.Now;
                    e.From = actionUser;
                },
                BeforeModify = (cur, prev) =>
                {
                    prev.Content = cur.Content;
                    prev.ModifyBy = actionUser;
                    prev.ReplyId = cur.ReplyId;
                    prev.LastModify = DateTime.Now;
                },
                PermissionJudgeItem = new EntityModifyExtensions.PermissionJudgeItem<ApplyComment>()
                {
                    CompanyGetter = c => c?.From?.CompanyInfo?.CompanyCode ?? string.Empty,
                    Description = "评论",
                    Flag =  EntityModifyExtensions.PermissionFlag.WriteSelfDirectAllow | EntityModifyExtensions.PermissionFlag.GlobalReverse,
                    Permission = ApplicationPermissions.Apply.Vacation.AttachInfo.Item,
                },
                QueryItemGetter = c => c.Id == model.Data.Id
            });
            context.SaveChanges();
            return new JsonResult(new EntityViewModel<ApplyCommentVDataModel>(m.Item2.ToDataModel(context.ApplyCommentLikesDb, context.ApplyCommentsDb, actionUser.Id, 1)));
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
            var like = context.ApplyCommentLikes.Where(i => i.CommentId == comment.Id).FirstOrDefault(i => i.CreateById == currentUser.Id);
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