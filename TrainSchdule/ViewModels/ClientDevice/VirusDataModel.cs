using DAL.Entities.ClientDevice;
using DAL.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.ClientDevice
{
    /// <summary>
    /// 
    /// </summary>
    public class VirusDataModel
    {        
        /// <summary>
        /// id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 出现时间【冗余】
        /// </summary>
        public DateTime Create { get; set; }
        /// <summary>
        /// 状态变更时间【冗余】
        /// </summary>
        public DateTime HandleDate { get; set; }
        /// <summary>
        /// 病毒单一性描述
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 病毒路径
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 病毒类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Sha1
        /// </summary>
        public string Sha1 { get; set; }
        /// <summary>
        /// 处置状态
        /// </summary>
        public VirusStatus Status { get; set; }
        /// <summary>
        /// 来源终端mid
        /// </summary>
        public string Client { get; set; }
        /// <summary>
        /// 来源终端的ip【计算项】
        /// </summary>
        public string ClientIp { get; set; }
        /// <summary>
        /// 责任人【冗余】
        /// </summary>
        public string Owner { get; set; }
        /// <summary>
        /// 责任单位【冗余】
        /// </summary>
        public string Company { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class VirusQueryDataModel
    {
        /// <summary>
        /// 创建人用户名
        /// </summary>
        public QueryByString CreateBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public QueryByDate Create { get; set; }
        /// <summary>
        /// <see cref="VirusStatus"/>
        /// </summary>
        public QueryByIntOrEnum Status { get; set; }
        /// <summary>
        /// 来源终端的mid
        /// </summary>
        public QueryByString Client { get; set; }
        /// <summary>
        /// 来源终端的ip
        /// </summary>
        public QueryByString Ip { get; set; }
        /// <summary>
        /// 病毒名
        /// </summary>
        public QueryByString FileName { get; set; }
        /// <summary>
        /// 病毒类型
        /// </summary>
        public QueryByString Type { get; set; }

        /// <summary>
        /// 分页
        /// </summary>
        public QueryByPage Pages { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public static class VirusExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="clients"></param>
        /// <param name="raw"></param>
        /// <returns></returns>
        public static Virus ToModel(this VirusDataModel model,IQueryable<Client>clients,Virus raw=null) {
            if (raw == null) raw = new Virus();
            raw.Client = clients.FirstOrDefault(i=>i.MachineId==model.Client);
            if (raw.Client != null)
            {
                raw.ClientIp = raw.Client.Ip;
                raw.Owner = raw.Client.OwnerId;
                raw.Company = raw.Client.Company.Code;
            }
            raw.ClientMachineId = raw.Client.MachineId; // cache client info
            raw.Create = model.Create;
            raw.FileName = model.FileName;
            raw.HandleDate = model.HandleDate;
            raw.Sha1 = model.Sha1;
            raw.Key = model.Key;
            raw.Type = model.Type;
            // if (raw.Status != model.Status) raw.HandleDate = DateTime.Now;
            // raw.Status = model.Status; // 不应由用户编辑
            return raw;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static VirusDataModel ToModel(this Virus model)
        {
            return new VirusDataModel()
            {
                Id=model.Id,
                HandleDate = model.HandleDate,
                Client = model.ClientMachineId,
                ClientIp=model.ClientIp,
                Company=model.Company,
                Owner=model.Owner,
                Create = model.Create,
                FileName = model.FileName,
                Key=model.Key,
                Sha1 = model.Sha1,
                Status = model.Status,
                Type = model.Type
            };
        }
    }
}
