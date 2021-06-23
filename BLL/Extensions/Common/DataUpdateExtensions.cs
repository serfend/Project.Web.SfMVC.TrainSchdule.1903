using BLL.Interfaces.Common;
using DAL.Entities;
using DAL.Entities.Permisstions;
using DAL.Entities.UserInfo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace BLL.Extensions.Common
{

    /// <summary>
    /// 
    /// </summary>
    public static class EntityModifyExtensions
    {
        /// <summary>
        /// 权限认定
        /// </summary>
        public class PermissionJudgeItem<T> where T : BaseEntityGuid
        {
            /// <summary>
            /// 
            /// </summary>
            public PermissionFlag Flag { get; set; }
            /// <summary>
            /// 定义如何获取用于授权的单位
            /// </summary>
            public Func<T, string> CompanyGetter { get; set; }
            /// <summary>
            /// 定义如何获取到目标人
            /// </summary>
            public Func<T,string> UserGetter { get; set; }
            /// <summary>
            /// 授权权限
            /// </summary>
            public Permission Permission { get; set; }
            /// <summary>
            /// 本次编辑描述
            /// </summary>
            public string Description { get; set; }
        }
        /// <summary>
        /// 认定方式
        /// </summary>
        [Flags]
        public enum PermissionFlag
        {
            /// <summary>
            /// 
            /// </summary>
            None = 0,
            /// <summary>
            /// 是否反向验证 即设定为block时才阻止
            /// 否则则默认放行
            /// </summary>
            GlobalReverse = 1,
            /// <summary>
            /// 
            /// </summary>
            ReadReverse = 2,
            /// <summary>
            /// 
            /// </summary>
            WriteReverse = 4,
            /// <summary>
            /// 直接放行读取
            /// </summary>
            ReadDirectAllow = 8,
            /// <summary>
            /// 直接放行编辑
            /// </summary>
            WriteDirectAllow = 16,
            /// <summary>
            /// 直接放行读取本人内容
            /// </summary>
            ReadSelfDirectAllow = 32,
            /// <summary>
            /// 直接放行操作本人内容
            /// </summary>
            WriteSelfDirectAllow = 64
        }

        /// <summary>
        /// 
        /// </summary>
        public enum ActionType
        {
            /// <summary>
            /// 
            /// </summary>
            Add = 0,
            /// <summary>
            /// 
            /// </summary>
            Remove = 1,
            /// <summary>
            /// 
            /// </summary>
            Update = 2
        }
        /// <summary>
        /// 
        /// </summary>
        public record DataUpdateModel<T> where T : BaseEntityGuid
        {
            /// <summary>
            /// 更新项
            /// </summary>
            public T Item { get; set; }
            /// <summary>
            /// 数据库
            /// </summary>
            public DbSet<T> Db { get; set; }
            /// <summary>
            /// 定义如何获取原项
            /// </summary>
            public Expression<Func<T, bool>> QueryItemGetter { get; set; }
            /// <summary>
            /// 更新 权限要求（默认权限）
            /// </summary>
            public PermissionJudgeItem<T> UpdateJudge { get; set; }
            /// <summary>
            /// 读取 权限要求
            /// </summary>
            public PermissionJudgeItem<T> ReadJudge { get; set; }
            /// <summary>
            /// 移除 权限要求
            /// </summary>
            public PermissionJudgeItem<T> RemoveJudge { get; set; }
            /// <summary>
            /// 添加 权限要求
            /// </summary>
            public PermissionJudgeItem<T> AddJudge { get; set; }
            /// <summary>
            /// 授权人
            /// </summary>
            public User AuthUser { get; set; }
            /// <summary>
            /// T cur,T prev
            /// </summary>
            public Action<T, T> BeforeModify { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Action<T> BeforeAdd { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="dataUpdateServices"></param>
        /// <returns></returns>
        public static (ActionType, T) UpdateGuidEntity<T>(this DataUpdateModel<T> model, IDataUpdateServices dataUpdateServices) where T : BaseEntityGuid
        => dataUpdateServices.Update<T>(model);
    }
}
