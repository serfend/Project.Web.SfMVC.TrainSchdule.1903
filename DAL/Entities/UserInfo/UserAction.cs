using DAL.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities.UserInfo
{
    public enum ActionRank
    {
        Debug = 32,
        Infomation = 16,
        Warning = 8,
        Danger = 4,
        Disaster = 0
    }

    public class UserAction : BaseEntityGuid, ICreateClientInfo
    {
        public string UserName { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>
        public UserOperation Operation { get; set; }

        /// <summary>
        /// 日志等级
        /// </summary>
        public ActionRank Rank { get; set; }

        /// <summary>
        /// 操作发生的时间
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 操作描述
        /// </summary>
        public string Description { get; set; }

        public string Ip { get; set; }
        public string Device { get; set; }
        public string UA { get; set; }
    }

    public enum UserOperation
    {
        Default = 0,
        Register = 1,
        Remove = 2,
        Restore = 10,
        ModifyUser = 3,
        Login = 4,
        Logout = 5,
        ModifyPsw = 8,
        CreateApply = 16,
        RemoveApply = 17,
        AuditApply = 18,
        ModifyApply = 19,
        AttachInfoToApply = 20,
        Permission = 32,
        FromUserReport = 64,
        FromSystemReport = 65,
        UserInfoUpdate = 72,
        FileInspect = 128,
        InvalidModel = 192,
        UpdateInfo = 194
    }
}