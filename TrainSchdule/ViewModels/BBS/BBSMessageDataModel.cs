using BLL.Interfaces.Common;
using DAL.Entities;
using DAL.Entities.Common.Message;
using DAL.Entities.UserInfo.UserAppMessage;
using DAL.QueryModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.BBS
{
    /// <summary>
    /// 
    /// </summary>
    public class QueryAppMessageModel : GoogleAuthViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public MessageQueryItem Item { get; set; }
    }

    /// <summary>
    /// 用户消息设置
    /// </summary>
    public class UserAppMessageInfoViewModel : BaseEntityGuid
    {
        /// <summary>
        /// 用户设置
        /// </summary>
        public AppMessageSetting Setting { get; set; } = (int)AppMessageSetting.AllowAddByScan + AppMessageSetting.AllowStrangerMessage;
        /// <summary>
        /// 【冗余】粉丝数
        /// </summary>
        public int FansCount { get; set; }
        /// <summary>
        /// 【冗余】关注数
        /// </summary>
        public int FollowCount { get; set; }
        /// <summary>
        /// 【冗余】未读消息
        /// </summary>
        public int UnreadMessage { get; set; }
    }
    /// <summary>
    /// 消息
    /// </summary>
    public class AppMessageViewModel : BaseEntityGuid
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Create { get; set; }
        /// <summary>
        /// 消息来源用户
        /// </summary>
        public string FromId { get; set; }
        /// <summary>
        /// 消息目标用户
        /// </summary>
        public string ToId { get; set; }
        /// <summary>
        /// 消息状态
        /// </summary>
        public AppMessageStatus Status { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public virtual AppMessageContent Content { get; set; }

        /// <summary>
        /// 【冗余】消息长度
        /// </summary>
        public int Length { get; set; }
    }
    /// <summary>
    /// 用户关系
    /// </summary>
    public class AppUserRelateViewModel
    {
        /// <summary>
        /// 发起用户
        /// </summary>
        public string FromId { get; set; }
        /// <summary>
        /// 目标用户
        /// </summary>
        public string ToId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Create { get; set; }
        /// <summary>
        /// 关系状态
        /// </summary>
        public Relation Relation { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public static class UserAppMessageExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static UserAppMessageInfoViewModel ToViewModel(this UserAppMessageInfo model) => new UserAppMessageInfoViewModel() {
            Id = model.Id,
            FansCount = model.FansCount,
            FollowCount = model.FollowCount,
            Setting = model.Setting,
            UnreadMessage = model.UnreadMessage
        };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static AppMessageViewModel ToViewModel(this AppMessage model)
        {
            var is_recalled = model.Status.HasFlag(AppMessageStatus.Recall);
            var result = new AppMessageViewModel()
            {
                Id = model.Id,
                Content = is_recalled?null: model.Content,
                Create = model.Create,
                FromId = model.FromId,
                Length = is_recalled?0:model.Length,
                Status = model.Status,
                ToId = model.ToId
            };
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static AppUserRelateViewModel ToViewModel(this AppUserRelate model) => new AppUserRelateViewModel() {
            //Id=model.Id,
            Create=model.Create,
            FromId=model.FromId,
            Relation=model.Relation,
            ToId=model.ToId
        };
    }
    /// <summary>
    /// 
    /// </summary>
    public class NewMessageModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string To { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Content { get; set; }
    }
    /// <summary>
    /// 用户关系操作
    /// </summary>
    public class BBSMessageUserRelationModel
    {
        /// <summary>
        /// 目标对象
        /// </summary>
        public  string Target { get; set; }
        /// <summary>
        /// 关系
        /// </summary>
        public Relation Relation { get; set; }
        /// <summary>
        /// 附加或移除此关系
        /// </summary>
            public bool IsAppend { get; set; }
    }
    /// <summary>
    /// 消息状态变更
    /// </summary>
    public class BBSMessageActionModel
    {
        /// <summary>
        /// 消息实体
        /// </summary>
        public Guid Message { get; set; }
        /// <summary>
        /// 动作
        /// </summary>
        public BLL.Interfaces.Common.Action Action { get; set; }
    }
    /// <summary>
    /// 站内信标头
    /// </summary>
    public class BBSMessageShadowDataModel: BBSMessageBaseDataModel
    {
    }
    /// <summary>
    /// 站内信
    /// </summary>
    public class BSSMessageDataModel : BBSMessageBaseDataModel
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
    }
    /// <summary>
    /// 基础信息
    /// </summary>
    public class BBSMessageBaseDataModel
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Create { get; set; }
        /// <summary>
        /// 消息来源用户
        /// </summary>
        public string FromId { get; set; }
        /// <summary>
        /// 消息目标用户
        /// </summary>
        public string ToId { get; set; }
        /// <summary>
        /// 消息状态
        /// </summary>
        public AppMessageStatus Status { get; set; }

        /// <summary>
        /// 消息长度
        /// </summary>
        public int Length { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public static class BBSMessageExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static T ToModel<T>(this AppMessage model) where T: BBSMessageBaseDataModel,new() => model==null?null: new T()
        {
               Create=model.Create,
               FromId=model.FromId,
               Length=model.Length,
               Status=model.Status,
               ToId=model.ToId
        };

    }
}
