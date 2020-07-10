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
		public DbSet<CommonDataGroup> CommonDataGroups { get; set; }
		public DbSet<CommonDataDictionary> CommonDataDictionaries { get; set; }

		private void Configuration_Common(ModelBuilder builder)
		{
			#region 数据字典组

			var applyStatus = "ApplyStatus";
			var applyAuditStatus = "ApplyAuditStatus";
			var applyAction = "ApplyAction";
			var group = builder.Entity<CommonDataGroup>();
			group.HasData(new List<CommonDataGroup>() {
				new CommonDataGroup()
				{
					Id=1,
					Name=applyStatus,
					Description="休假申请的审批状态，其描述为可进行的操作名称。联动系统逻辑，勿修改"
				},
				new CommonDataGroup()
				{
					Id=2,
					Name=applyAuditStatus,
					Description="休假申请审批步骤的状态。联动系统逻辑，勿修改"
				},
				new CommonDataGroup()
				{
					Id=3,
					Name=applyAction,
					Description="对应审批状态下可操作的行为。联动系统逻辑，勿修改"
				}
			});

			#endregion 数据字典组

			#region 数组字典数据

			var data = builder.Entity<CommonDataDictionary>();
			int dataId = 1;

			#region actions

			var Publish = "Publish";
			var Save = "Save";
			var Delete = "Delete";
			var Withdrew = "Withdrew";
			var Cancel = "Cancel";
			var actions = new List<CommonDataDictionary>(){
				new CommonDataDictionary()
				{
					Alias="保存",
					Description="保存到系统，使得记录不被删除",
					Color="success",
					Key=Save,
					Value=2
				},
				new CommonDataDictionary()
				{
					Alias="发布",
					Description="开始审批流程。可以撤回申请，但不再可删除",
					Color="success",
					Key=Publish,
					Value=4
				},
				new CommonDataDictionary()
				{
					Alias="删除",
					Description="不再可见，且不可恢复",
					Color="danger",
					Key=Delete,
					Value=8
				},
				new CommonDataDictionary()
				{
					Alias="撤回",
					Description="取消审批流程，且不可恢复",
					Color="info",
					Key=Withdrew,
					Value=16
				},
				new CommonDataDictionary()
				{
					Alias="作废",
					Description="认定此次休假数据无效，且不可恢复",
					Color="danger",
					Key=Cancel,
					Value=32
				},
				};
			foreach (var d in actions)
			{
				d.Id = dataId++;
				d.GroupName = applyAction;
			}
			data.HasData(actions);

			#endregion actions

			#region Status

			var toHtml = new Func<Color, string>((c) => string.Format("#{0:x2}{1:x2}{2:x2}{3:x3}", c.A, c.R, c.G, c.B));
			var Status_NotPublish = "NotPublish";
			var Status_Auditing = "Auditing";
			var Status_Withdrew = "Withdrew";
			var Status_AcceptAndWaitAdmin = "AcceptAndWaitAdmin";
			var Status_Accept = "Accept";
			var Status_Denied = "Denied";
			var Status_NotSave = "NotSave";
			var Status_Cancel = "Cancel";
			data.HasIndex(d => d.Key);
			var status = new List<CommonDataDictionary>()
			{
				new CommonDataDictionary()
				{
					Alias="未保存",
					Key = Status_NotSave,
					Color=toHtml(Color.Black),
					Description=$"{Save}##{Publish}##{Delete}",
					Value=0
				},
				new CommonDataDictionary()
				{
					Alias="未发布",
					Key = Status_NotPublish,
					Color=toHtml(Color.DarkGray),
					Description=$"{Publish}##{Delete}",
					Value=10
				},
				new CommonDataDictionary()
				{
					Alias="已撤回",
					Key = Status_Withdrew,
					Color=toHtml(Color.Gray),
					Description=$"",
					Value=20
				},
				new CommonDataDictionary()
				{
					Alias="审核中",
					Key=Status_Auditing,
					Color =toHtml(Color.Coral),
					Description=$"{Withdrew}",
					Value= 40
				},
				new CommonDataDictionary()
				{
					Alias="终审中",
					Key=Status_AcceptAndWaitAdmin,
					Color = toHtml(Color.DeepSkyBlue),
					Description=$"{Withdrew}",
					Value= 50
				},
				new CommonDataDictionary()
				{
					Alias="被驳回",
					Key=Status_Denied,
					Color =toHtml(Color.Red),
					Description=$"",
					Value= 75
				},
				new CommonDataDictionary()
				{
					Alias="审核中",
					Key=Status_Accept,
					Color =toHtml(Color.LimeGreen),
					Description=$"{Cancel}",
					Value= 100
				},
				new CommonDataDictionary()
				{
					Alias="被作废",
					Key=Status_Cancel,
					Color = toHtml(Color.LightSlateGray),
					Description=$"",
					Value= 125
				},
			};
			foreach (var d in status)
			{
				d.Id = dataId++;
				d.GroupName = applyStatus;
			}
			data.HasData(status);

			#endregion Status

			#endregion 数组字典数据
		}
	}
}