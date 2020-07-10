using DAL.Entities.ApplyInfo;
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

		private const string applyStatus = "ApplyStatus";
		private const string applyAuditStatus = "ApplyAuditStatus";
		private const string applyAction = "ApplyAction";
		private const string applyExecuteStatus = "ApplyExecuteStatus";
		private const string Publish = "Publish";
		private const string Save = "Save";
		private const string Delete = "Delete";
		private const string Withdrew = "Withdrew";
		private const string Cancel = "Cancel";
		private int dataId = 1;

		private static string ToHtml(Color c) => string.Format("#{0:x2}{1:x2}{2:x2}{3:x2}", c.R, c.G, c.B, c.A);

		/// <summary>
		/// 数据字典 分组数据
		/// </summary>
		/// <param name="builder"></param>
		private void Configuration_Common_Group(ModelBuilder builder)
		{
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
				},
				new CommonDataGroup(){
				Id=4,
				Name=applyExecuteStatus,
				Description="休假的落实状态。联动系统逻辑，勿修改"
				}
			});
		}

		/// <summary>
		/// 数据字典 申请的操作数据
		/// </summary>
		/// <param name="builder"></param>
		private void Configuration_Actions(ModelBuilder builder)
		{
			#region actions

			var data = builder.Entity<CommonDataDictionary>();

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
		}

		/// <summary>
		/// 数据字典 申请状态数据
		/// </summary>
		/// <param name="builder"></param>
		private void Configuration_Status(ModelBuilder builder)
		{
			var data = builder.Entity<CommonDataDictionary>();
			var status = new List<CommonDataDictionary>()
			{
				new CommonDataDictionary()
				{
					Alias="未保存",
					Key = AuditStatus.NotSave.ToString(),
					Color=ToHtml(Color.Black),
					Description=$"{Save}##{Publish}##{Delete}",
					Value=(int)AuditStatus.NotSave
				},
				new CommonDataDictionary()
				{
					Alias="未发布",
					Key = AuditStatus.NotPublish.ToString(),
					Color=ToHtml(Color.DarkGray),
					Description=$"{Publish}##{Delete}",
					Value=(int)AuditStatus.NotPublish
				},
				new CommonDataDictionary()
				{
					Alias="已撤回",
					Key = AuditStatus.Withdrew.ToString(),
					Color=ToHtml(Color.Gray),
					Description=$"",
					Value=20
				},
				new CommonDataDictionary()
				{
					Alias="审核中",
					Key=AuditStatus.Auditing.ToString(),
					Color =ToHtml(Color.Coral),
					Description=$"{Withdrew}",
					Value= (int)AuditStatus.Auditing
				},
				new CommonDataDictionary()
				{
					Alias="终审中",
					Key=AuditStatus.AcceptAndWaitAdmin.ToString(),
					Color = ToHtml(Color.DeepSkyBlue),
					Description=$"{Withdrew}",
					Value= (int)AuditStatus.AcceptAndWaitAdmin
				},
				new CommonDataDictionary()
				{
					Alias="被驳回",
					Key=AuditStatus.Denied.ToString(),
					Color =ToHtml(Color.Red),
					Description=$"",
					Value= (int)AuditStatus.Denied
				},
				new CommonDataDictionary()
				{
					Alias="已通过",
					Key=AuditStatus.Accept.ToString(),
					Color =ToHtml(Color.LimeGreen),
					Description=$"{Cancel}",
					Value= (int)AuditStatus.Accept
				},
				new CommonDataDictionary()
				{
					Alias="被作废",
					Key=AuditStatus.Cancel.ToString(),
					Color = ToHtml(Color.LightSlateGray),
					Description=$"",
					Value= (int)AuditStatus.Cancel
				},
			};
			foreach (var d in status)
			{
				d.Id = dataId++;
				d.GroupName = applyStatus;
			}
			data.HasData(status);
		}

		/// <summary>
		/// 数据字典 休假的落实状态
		/// </summary>
		/// <param name="builder"></param>
		private void Configuration_ExecuteStatus(ModelBuilder builder)
		{
			var data = builder.Entity<CommonDataDictionary>();
			var status = new List<CommonDataDictionary>()
			{
				new CommonDataDictionary()
				{
					Alias="未确认",
					Key = ExecuteStatus.NotSet.ToString(),
					Color=ToHtml(Color.OrangeRed),
					Description="待确认归队时间",
					Value=(int)ExecuteStatus.NotSet
				},
				new CommonDataDictionary()
				{
					Alias="已确认",
					Key = ExecuteStatus.BeenSet.ToString(),
					Color=ToHtml(Color.Green),
					Description="已确认归队时间",
					Value=(int)ExecuteStatus.BeenSet
				},
				new CommonDataDictionary()
				{
					Alias="推迟归队",
					Key = ExecuteStatus.Delay.ToString(),
					Color=ToHtml(Color.Red),
					Description="因事推迟归队",
					Value=(int)ExecuteStatus.BeenSet|(int)ExecuteStatus.Delay
				},
				new CommonDataDictionary()
				{
					Alias="被召回",
					Key = ExecuteStatus.Recall.ToString(),
					Color=ToHtml(Color.Blue),
					Description="因事提前归队",
					Value=(int)ExecuteStatus.BeenSet|(int)ExecuteStatus.Recall
				},
			};
			foreach (var d in status)
			{
				d.Id = dataId++;
				d.GroupName = applyExecuteStatus;
			}
			data.HasData(status);
		}

		private void Configuration_Common(ModelBuilder builder)
		{
			var data = builder.Entity<CommonDataDictionary>();
			data.HasIndex(d => d.Key);

			Configuration_Common_Group(builder);
			Configuration_Actions(builder);
			Configuration_Status(builder);
			Configuration_ExecuteStatus(builder);
		}
	}
}