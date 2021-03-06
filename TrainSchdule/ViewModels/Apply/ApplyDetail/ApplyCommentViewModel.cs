﻿using BLL.Extensions;
using BLL.Extensions.Common;
using BLL.Helpers;
using DAL.DTO.Apply;
using DAL.DTO.User;
using DAL.Entities.ApplyInfo;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.User;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.Apply.ApplyDetail
{
    /// <summary>
    ///
    /// </summary>
    public static class ApplyCommentExtensions
    {
        /// <summary>
        /// 获取指定目标的评论
        /// </summary>
        /// <param name="i"></param>
        /// <param name="likesDb"></param>
        /// <param name="commentsDb"></param>
        /// <param name="currentUser"></param>
        /// <param name="deep"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static Tuple<IEnumerable<ApplyCommentVDataModel>, int> Replies(this ApplyComment i, IQueryable<ApplyCommentLike> likesDb, IQueryable<ApplyComment> commentsDb, string currentUser, int deep = 0, int pageIndex = 0, int pageSize = 3)
        {
            var list = commentsDb
                     .Where(c => c.ReplyId == i.Id)
                     .OrderByDescending(c => c.Create)
                     .SplitPage(pageIndex, pageSize);
            var result = list.Item1.AsEnumerable().Select(i => i.ToDataModel(likesDb, commentsDb, currentUser, deep - 1, pageIndex, pageSize)).ToList();
            return new Tuple<IEnumerable<ApplyCommentVDataModel>, int>(result, list.Item2);
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <param name="likesDb">点赞库</param>
        /// <param name="commentsDb"></param>
        /// <param name="currentUser">当前登录的用户</param>
        /// <param name="deep"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static ApplyCommentVDataModel ToDataModel(this ApplyComment i, IQueryable<ApplyCommentLike> likesDb, IQueryable<ApplyComment> commentsDb, string currentUser, int deep = 0, int pageIndex = 0, int pageSize = 3)
        => new ApplyCommentVDataModel()
        {
            Id = i.Id,
            Reply = i.ReplyId,
            Apply = i.Apply,
            Content = i.Content,
            Create = i.Create,
            From = i.AnonymousNick != null ? null : i.From.ToSummaryDto(),
            AnonymousNick = i.AnonymousNick,
            LastModify = i.LastModify,
            ModifyBy = i.ModifyBy.ToSummaryDto(),
            Like = i.Likes,
            MyLike = currentUser == null ? false : likesDb.Where(like => like.CommentId == i.Id).Any(like => like.CreateById == currentUser),
            Replies = deep > 0 ? i.Replies(likesDb, commentsDb, currentUser, deep, pageIndex, pageSize) : new Tuple<IEnumerable<ApplyCommentVDataModel>, int>(new List<ApplyCommentVDataModel>(), 0)
        };
        /// <summary>
        /// 
        /// </summary>x
        /// <returns></returns>
        public static ApplyComment ToModel(this ApplyCommentDataModel model, IQueryable<ApplyComment> comments, ApplyComment raw = null)
        {
            if (raw == null) raw = new ApplyComment();
            raw.Apply = model.Apply;
            raw.Content = model.Content;
            raw.Create = DateTime.Now;
            raw.IsRemoved = model.IsRemove;
            raw.AnonymousNick = model.AnonymousNick;
            if (model.Reply != null)
            {
                var target = comments.FirstOrDefault(c => c.Id == model.Reply);
                if (target != null) raw.ReplyId = model.Reply;
            }
            return raw;
        }
    }

    /// <summary>
    /// 点赞
    /// </summary>
    public class ApplyCommentLikeDataModel
    {
        /// <summary>
        /// 评论id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 赞/取消
        /// </summary>
        public bool Like { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ApplyCommentViewModel : GoogleAuthViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "评论信息未填写")]
        public ApplyCommentDataModel Data { get; set; }
    }
    /// <summary>
    ///
    /// </summary>
    public class ApplyCommentDataModel
    {
        /// <summary>
        /// 若修改/删除 则填写
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 作用对象
        /// </summary>
        public string Apply { get; set; }
        /// <summary>
        /// 回复某评论 使用则无需填写Apply
        /// </summary>
        public Guid? Reply { get; set; }
        /// <summary>
        /// 是否是删除
        /// </summary>
        public bool IsRemove { get; set; }
        /// <summary>
        ///
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 匿名昵称
        /// </summary>
        public string AnonymousNick { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class ApplyCommentVDataModel
    {
        /// <summary>
        ///
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? Reply { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Tuple<IEnumerable<ApplyCommentVDataModel>, int> Replies { get; set; }
        /// <summary>
        ///
        /// </summary>
        public UserSummaryDto From { get; set; }
        /// <summary>
        /// 匿名昵称
        /// </summary>
        public string AnonymousNick { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime Create { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime LastModify { get; set; }

        /// <summary>
        ///
        /// </summary>
        public UserSummaryDto ModifyBy { get; set; }

        /// <summary>
        /// 作用到
        /// </summary>
        public string Apply { get; set; }

        /// <summary>
        /// 点赞
        /// </summary>
        public int Like { get; set; }

        /// <summary>
        /// 当前用户是否点赞
        /// </summary>
        public bool MyLike { get; set; }
    }
}