using DAL.Entities.Common.Message;
using DAL.Entities.UserInfo;
using DAL.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.Common
{
    public enum Action
    {
        CheckExist = 0,
        Recall = 1,
        Delete = 2
    }
    public interface IAppMessageServices
    {
        /// <summary>
        /// 发送一个新的消息
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="content"></param>
        /// <param name="isSystem">是否是系统消息</param>
        /// <returns></returns>
        AppMessage Send(string from,string to,string content,bool isSystem);
        /// <summary>
        /// 手动修改消息状态
        /// </summary>
        /// <param name="message">消息id</param>
        /// <param name="action"></param>
        /// <returns></returns>
        AppMessage Action(string user,Guid message, Action action);
        /// <summary>
        /// 获取指定信息的内容
        /// </summary>
        /// <param name="id">消息id</param>
        /// <returns></returns>
        AppMessage GetDetail(Guid id);
        /// <summary>
        /// 获取未读项
        /// </summary>
        /// <returns></returns>
        IQueryable<AppMessage>  GetUnread(string user);
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Tuple<IQueryable<AppMessage>, int> Query(MessageQueryItem item);

    }

    /// <summary>
    /// 消息查询
    /// </summary>
    public class MessageQueryItem 
    {
        /// <summary>
        /// 
        /// </summary>
        public QueryByPage Pages { get; set; }
        /// <summary>
        /// 按内容查询
        /// </summary>
        public QueryByString Content { get; set; }
        /// <summary>
        /// 按消息来源查询，默认为当前登录账号
        /// </summary>
        public QueryByString From { get; set; }
        /// <summary>
        /// 按接收人查询
        /// </summary>
        public QueryByString To { get; set; }
        /// <summary>
        /// 按创建时间查询
        /// </summary>
        public QueryByDate Create { get; set; }
    }
}
