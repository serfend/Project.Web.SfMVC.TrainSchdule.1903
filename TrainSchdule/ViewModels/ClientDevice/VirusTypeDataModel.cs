using DAL.Entities.ClientDevice;
using DAL.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.ClientDevice
{
    /// <summary>
    /// 病毒类型
    /// </summary>
    public class VirusTypeDataModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 病毒类型 如为未识别的类型则按Sha1、(终端ip+时间出现(60秒)分类
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 仅病毒类型无法分类时使用
        /// </summary>
        public string Sha1 { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Create { get; set; }
        /// <summary>
        /// 别称
        /// </summary>
        public string Alias { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 【冗余】是否自动分配
        /// </summary>
        public bool IsAutoDispatch { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsRemoved { get; set; }
    }
    /// <summary>
    /// 类型查询
    /// </summary>
    public class VirusTypeQueryDataModel {

        /// <summary>
        /// 分页
        /// </summary>
        public QueryByPage Pages { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public QueryByDate Create { get; set; }
        /// <summary>
        /// ID
        /// </summary>
        public QueryByGuid Id { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public QueryByString Type { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public QueryByString Description { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ClientVirusTypeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="isAutoDispatch"></param>
        /// <returns></returns>
        public static VirusTypeDataModel ToModel(this VirusTrace client,bool isAutoDispatch)
        {
            return new VirusTypeDataModel()
            {
                Id = client.Id,
                Create = client.Create,
                Alias=client.Alias,
                Description=client.Description,
                Sha1=client.Sha1,
                Type=client.Type,
                IsAutoDispatch=isAutoDispatch
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="raw"></param>
        /// <returns></returns>
        public static VirusTrace ToModel(this VirusTypeDataModel client,  VirusTrace raw = null)
        {
            if (raw == null) raw = new VirusTrace();
            raw.Type = client.Type;
            raw.Sha1 = client.Sha1;
            raw.Alias = client.Alias;
            raw.Create = client.Create;
            raw.Description = client.Description;
            return raw;
        }
    }
}
