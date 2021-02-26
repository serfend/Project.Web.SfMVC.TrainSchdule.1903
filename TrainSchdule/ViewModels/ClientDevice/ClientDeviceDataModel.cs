using BLL.Extensions.Common;
using BLL.Interfaces;
using DAL.Entities.ClientDevice;
using DAL.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.BBS
{
	/// <summary>
	/// 终端
	/// </summary>
	public class ClientDeviceDataModel
	{
		/// <summary>
		/// id
		/// </summary>
		public Guid Id { get; set; }
		/// <summary>
		/// mid
		/// </summary>
		public string MachineId { get; set; }
		/// <summary>
		/// 终端ip
		/// </summary>
		public string Ip { get; set; }
		/// <summary>
		/// 终端ip值
		/// </summary>
		public int IpInt { get; set; }
		/// <summary>
		/// 终端mac
		/// </summary>
		public string Mac { get; set; }
		/// <summary>
		/// 类型型号
		/// </summary>
		public string DeviceType { get; set; }
		/// <summary>
		/// 负责人id
		/// </summary>
		public  string Owner { get; set; }
		/// <summary>
		/// 负责人姓名
		/// </summary>
		public  string OwnerRealName { get; set; }
		/// <summary>
		/// 所属单位
		/// </summary>
		public  string Company { get; set; }
		/// <summary>
		/// 是否删除
		/// </summary>
		public bool IsRemoved { get; set; }
		/// <summary>
		/// 备注信息
		/// </summary>
		public string FutherInfo { get; set; }
	}

	/// <summary>
	/// 设备查询
	/// </summary>
	public class ClientDeviceQueryDataModel
	{

		/// <summary>
		/// 分页
		/// </summary>
		public QueryByPage Pages { get; set; }

		/// <summary>
		/// 设备类型
		/// </summary>
		public QueryByString DeviceType { get; set; }
		/// <summary>
		/// ip
		/// </summary>
		public QueryByString Ip { get; set; }
		/// <summary>
		/// mid
		/// </summary>
		public QueryByString MachineId { get; set; }
		/// <summary>
		/// 备注信息
		/// </summary>
		public QueryByString FutherInfo { get; set; }
		/// <summary>
		/// 使用人
		/// </summary>
		public QueryByString Owner { get; set; }
		/// <summary>
		/// 责任单位
		/// </summary>
		public QueryByString Company { get; set; }
	}
	/// <summary>
	/// 
	/// </summary>
	public static class ClientDeviceExtensions
    {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="client"></param>
		/// <returns></returns>
		public static ClientDeviceDataModel ToModel(this Client client) {
			return new ClientDeviceDataModel()
			{
				Id=client.Id,
				Company = client.Company?.Code,
				DeviceType=client.DeviceType,
				Ip=client.Ip,
				Mac=client.Mac,
				MachineId=client.MachineId,
				Owner=client.Owner?.Id,
				OwnerRealName=client.Owner?.BaseInfo?.RealName, // cache
				IsRemoved =client.IsRemoved,
				FutherInfo=client.FutherInfo
			};
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="usersService"></param>
        /// <param name="companies"></param>
        /// <param name="raw"></param>
        /// <returns></returns>
        public static Client ToModel(this ClientDeviceDataModel client,IUsersService usersService, IQueryable<DAL.Entities.Company> companies,  Client raw = null)
		{
			if (raw == null) raw = new Client();
			raw.Company = companies.FirstOrDefault(i => i.Code == client.Company);
			if (client.Owner == null)
				raw.Owner = usersService.GetUserByRealname(client.OwnerRealName).FirstOrDefault();
			else
				raw.Owner = usersService.GetById(client.Owner);
			raw.OwnerId = raw.Owner?.Id ;
			raw.DeviceType = client.DeviceType ?? raw.DeviceType;
			raw.Ip = client.Ip ?? raw.Ip;
			raw.IpInt = raw.Ip?.Ip2Int() ?? 0;
			raw.Mac = client.Mac ?? raw.Mac;
			raw.MachineId = client.MachineId ?? raw.MachineId;
			raw.IsRemoved = client.IsRemoved;
			raw.FutherInfo = client.FutherInfo ?? raw.FutherInfo;
			return raw;
		}
    }
}