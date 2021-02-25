using DAL.Entities.ApplyInfo;
using DAL.Entities.ClientDevice;
using DAL.Entities.Common.DataDictionary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace DAL.Data
{
	public partial class ApplicationDbContext
	{

		/// <summary>
		/// 数据字典 分组数据
		/// </summary>
		/// <param name="builder"></param>
		private void Configuration_Common_Group_Client(ModelBuilder builder)
		{
			var group = builder.Entity<CommonDataGroup>();
			group.HasData(new List<CommonDataGroup>() {
				new CommonDataGroup()
				{
					Name=clientVirusHandleStatus,
					Description="病毒处置状态，不应修改"
				},
				new CommonDataGroup()
				{
					Name=clientVirusStatus,
					Description="病毒状态，不应修改"
				}
			});
		}
		public const string clientVirusHandleStatus = "clientVirusHandleStatus";
		public const string clientVirusStatus = "clientVirusStatus";
		/// <summary>
		/// 数据字典 病毒处置状态
		/// </summary>
		/// <param name="builder"></param>
		private void Configuration_ClientStatus(ModelBuilder builder)
		{
			Configuration_ClientVirusStatus(builder);
			var data = builder.Entity<CommonDataDictionary>();
			var status = new List<CommonDataDictionary>()
			{
				new CommonDataDictionary()
				{
					Alias="无",
					Key = VirusHandleStatus.None.ToString(),
					Color=ToHtml(Color.LightGray),
					Description="暂无状态",
					Value=(int)VirusHandleStatus.None
				},
				new CommonDataDictionary()
				{
					Alias="终端设备",
					Key = VirusHandleStatus.ClientDevice.ToString(),
					Color=ToHtml(Color.LightGray),
					Description="终端设备",
					Value=(int)VirusHandleStatus.ClientDevice
				},
				new CommonDataDictionary()
				{
					Alias="终端病毒",
					Key = VirusHandleStatus.ClientDeviceVirus.ToString(),
					Color=ToHtml(Color.LightGray),
					Description="终端设备病毒",
					Value=(int)VirusHandleStatus.ClientDeviceVirus
				},
				new CommonDataDictionary()
				{
					Alias="染毒通告",
					Key = VirusHandleStatus.ClientDeviceVirusNotify.ToString(),
					Color=ToHtml(Color.PeachPuff),
					Description="通过公告系统发出染毒通告",
					Value=(int)VirusHandleStatus.ClientDeviceVirusNotify
				},
				new CommonDataDictionary()
				{
					Alias="染毒即时消息",
					Key = VirusHandleStatus.ClientDeviceVirusMessage.ToString(),
					Color=ToHtml(Color.MediumPurple),
					Description="通过第三方发出染毒即时消息",
					Value=(int)VirusHandleStatus.ClientDeviceVirusMessage
				},
				new CommonDataDictionary()
				{
					Alias="新增待处置",
					Key = VirusHandleStatus.ClientDeviceVirusNew.ToString(),
					Color=ToHtml(Color.Red),
					Description="终端设备新增待处置",
					Value=(int)VirusHandleStatus.ClientDeviceVirusNew
				},
				new CommonDataDictionary()
				{
					Alias="处置成功",
					Key = VirusHandleStatus.ClientDeviceVirusNewSuccess.ToString(),
					Color=ToHtml(Color.ForestGreen),
					Description="终端设备新增处置成功",
					Value=(int)VirusHandleStatus.ClientDeviceVirusNewSuccess
				},
				new CommonDataDictionary()
				{
					Alias="新增未处置",
					Key = VirusHandleStatus.ClientDeviceVirusNewUnhandle.ToString(),
					Color=ToHtml(Color.DarkRed),
					Description="终端设备新增未处置",
					Value=(int)VirusHandleStatus.ClientDeviceVirusNewUnhandle
				},
				new CommonDataDictionary()
				{
					Alias="新增处置失败",
					Key = VirusHandleStatus.ClientDeviceVirusNewFail.ToString(),
					Color=ToHtml(Color.IndianRed),
					Description="终端设备新增处置失败",
					Value=(int)VirusHandleStatus.ClientDeviceVirusNewFail
				},
				new CommonDataDictionary()
				{
					Alias="新增已处置",
					Key = VirusHandleStatus.ClientDeviceVirusHandle.ToString(),
					Color=ToHtml(Color.AliceBlue),
					Description="终端设备新增已处置",
					Value=(int)VirusHandleStatus.ClientDeviceVirusHandle
				},
				new CommonDataDictionary()
				{
					Alias="自主处置",
					Key = VirusHandleStatus.ClientDeviceVirusHandleByUser.ToString(),
					Color=ToHtml(Color.AliceBlue),
					Description="自主处置",
					Value=(int)VirusHandleStatus.ClientDeviceVirusHandleByUser
				},
				new CommonDataDictionary()
				{
					Alias="第三方处置",
					Key = VirusHandleStatus.ClientDeviceVirusHandleByIgnore.ToString(),
					Color=ToHtml(Color.DodgerBlue),
					Description="通过第三方处置",
					Value=(int)VirusHandleStatus.ClientDeviceVirusHandleByIgnore
				},
				new CommonDataDictionary()
				{
					Alias="提交处置",
					Key = VirusHandleStatus.ClientDeviceVirusHandleBySubmit.ToString(),
					Color=ToHtml(Color.MediumBlue),
					Description="通过提交方式处置",
					Value=(int)VirusHandleStatus.ClientDeviceVirusHandleBySubmit
				},
			};
			foreach (var d in status)
			{
				d.Id = dataId++;
				d.GroupName = clientVirusHandleStatus;
			}
			data.HasData(status);
		}
		private void Configuration_ClientVirusStatus(ModelBuilder builder)
		{
			var data = builder.Entity<CommonDataDictionary>();
			var status = new List<CommonDataDictionary>()
			{
				new CommonDataDictionary()
				{
					Alias="无状态",
					Key = VirusStatus.None.ToString(),
					Color=ToHtml(Color.Red),
					Description="无状态可用",
					Value=(int)VirusStatus.None
				},
				new CommonDataDictionary()
				{
					Alias="待处理",
					Key = VirusStatus.Unhandle.ToString(),
					Color=ToHtml(Color.Red),
					Description="处于待处理状态",
					Value=(int)VirusStatus.Unhandle
				},
				new CommonDataDictionary()
				{
					Alias="处置成功",
					Key = VirusStatus.Success.ToString(),
					Color=ToHtml(Color.ForestGreen),
					Description="此项已处置成功",
					Value=(int)VirusStatus.Success
				},
				new CommonDataDictionary()
				{
					Alias="终端推送已发出",
					Key = VirusStatus.ClientNotify.ToString(),
					Color=ToHtml(Color.LightGray),
					Description="已通过推送系统向终端发送染毒通告待处理中",
					Value=(int)VirusStatus.ClientNotify
				},
				new CommonDataDictionary()
				{
					Alias="第三方消息已发出",
					Key = VirusStatus.MessageSend.ToString(),
					Color=ToHtml(Color.LightGray),
					Description="已通过第三方系统发布消息",
					Value=(int)VirusStatus.MessageSend
				},
			};
			foreach (var d in status)
			{
				d.Id = dataId++;
				d.GroupName = clientVirusStatus;
			}
			data.HasData(status);
		}
	}
}