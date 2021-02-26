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
    public class VirusHandleRecordDataModel
    {        
        /// <summary>
        /// 处置记录的id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Create { get; set; }
        /// <summary>
        /// 所属病毒key
        /// </summary>
        public string Virus { get; set; }
        /// <summary>
        /// 操作状态
        /// </summary>
        public VirusHandleStatus HandleStatus { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String Remark { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsRemoved { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class VirusHandleRecordQueryDataModel
    {
        /// <summary>
        /// 处置状态 array
        /// </summary>
        public QueryByIntOrEnum HandleStatus { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public QueryByDate Create { get; set; }
        /// <summary>
        /// 所属病毒key
        /// </summary>
        public QueryByString Virus { get; set; }

        /// <summary>
        /// 分页
        /// </summary>
        public QueryByPage Pages { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public QueryByString Remark { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public static class VirusHandleRecordExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="viruses"></param>
        /// <param name="raw"></param>
        /// <returns></returns>
        public static VirusHandleRecord ToModel(this VirusHandleRecordDataModel model, IQueryable<Virus> viruses, VirusHandleRecord raw = null)
        {
            if (raw == null) raw = new VirusHandleRecord() {Create=DateTime.Now };
            raw.Create = model.Create;
            raw.HandleStatus = model.HandleStatus;
            raw.Remark = model.Remark ?? raw.Remark;
            raw.Virus = viruses.FirstOrDefault(i => i.Key == model.Virus) ?? raw.Virus;
            raw.VirusKey = raw.Virus?.Key;// cache client info
            raw.IsRemoved = model.IsRemoved;
            return raw;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static VirusHandleRecordDataModel ToModel(this VirusHandleRecord model)
        {
            return new VirusHandleRecordDataModel()
            {
                Id=model.Id,
                Virus=model.VirusKey,
                HandleStatus=model.HandleStatus,
                Create=model.Create,
                IsRemoved=model.IsRemoved,
                Remark=model.Remark
            };
        }
    }
}
