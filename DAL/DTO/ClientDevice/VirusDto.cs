﻿using DAL.Entities.ClientDevice;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.ClientDevice
{
    public class VirusDto
    {
        /// <summary>
        /// id
        /// </summary>
        [Required]
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
        /// 病毒类型id
        /// </summary>
        public string TypeId { get; set; }
        /// <summary>
        /// Sha1
        /// </summary>
        [IgnoreEmptyWithZero]
        public string Sha1 { get; set; }
        /// <summary>
        /// 危害级别
        /// </summary>
        public short WarningLevel { get; set; }
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
        /// <summary>
        /// 病毒类型名称【冗余】
        /// </summary>
        public string TraceAlias { get; set; }
    }

    public class IgnoreEmptyWithZeroAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var result = Convert.ToString(value, CultureInfo.CurrentCulture).Contains(new string('0', 10));
            this.ErrorMessage = result?"值为10位0以上的认定为无需处理项":null;
            return !result;
        }
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
        public static Virus ToModel(this VirusDto model, IQueryable<Client> clients, Virus raw = null)
        {
            if (raw == null) raw = new Virus();
            raw.Client = clients.FirstOrDefault(i => i.MachineId == model.Client);
            raw.ClientIp = model.ClientIp; // 以防出现未录入的终端无信息的情况
            if (raw.Client != null)
            {
                raw.Owner = raw.Client.OwnerId;
                raw.Company = raw.Client?.CompanyCode;
                raw.ClientMachineId = raw.Client.MachineId; // cache client info
            }
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
        public static VirusDto ToModel(this Virus model)
        {
            return new VirusDto()
            {
                Id = model.Id,
                HandleDate = model.HandleDate,
                Client = model.ClientMachineId,
                ClientIp = model.ClientIp,
                Company = model.Company,
                Owner = model.Owner,
                Create = model.Create,
                FileName = model.FileName,
                Key = model.Key,
                Sha1 = model.Sha1,
                Status = model.Status,
                Type = model.Type,
                TypeId = model.TraceTypeId.ToString(),
                TraceAlias=model.TraceAlias
            };
        }
    }
}
