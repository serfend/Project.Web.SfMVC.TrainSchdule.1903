using DAL.Entities.ApplyInfo;
using System.Collections.Generic;
using System.Drawing;

namespace BLL.Extensions.ApplyExtensions
{
	public static class ApplyStaticExtensions
	{
		public static string ToStr(this Auditing model)
		{
			switch (model)
			{
				case Auditing.Received: return "审核中";
				case Auditing.Denied: return "驳回";
				case Auditing.UnReceive: return "未收到审核";
				case Auditing.Accept: return "同意";
				default: return "null";
			}
		}

		private static Dictionary<int, AuditStatusMessage> statusDic;
		public static Dictionary<int, AuditStatusMessage> StatusDic { get { if (statusDic == null) statusDic = InitStatusDic(); return statusDic; } }
		private static Dictionary<string, ActionByUserItem> actionDic;
		public static Dictionary<string, ActionByUserItem> ActionDic { get { if (actionDic == null) actionDic = InitActionDic(); return actionDic; } }

		public static readonly Dictionary<int, Color> StatusColors = new Dictionary<int, Color>
			{
				{(int)AuditStatus.NotPublish, Color.DarkGray},
				{(int)AuditStatus.Auditing, Color.Coral},
				{(int)AuditStatus.Withdrew, Color.Gray},
				{(int)AuditStatus.AcceptAndWaitAdmin, Color.DeepSkyBlue},
				{(int)AuditStatus.Accept,Color.LimeGreen},
				{(int)AuditStatus.Denied, Color.Red},
				{(int)AuditStatus.NotSave,Color.Black },
				{(int)AuditStatus.Cancel,Color.LightSlateGray }
			};

		public readonly static Dictionary<int, string> StatusDesc = new Dictionary<int, string>()
			{
				{(int) AuditStatus.NotPublish, "未发布"},
				{(int) AuditStatus.Auditing, "审核中"},
				{(int) AuditStatus.Withdrew, "已撤回"},
				{(int) AuditStatus.AcceptAndWaitAdmin, "终审中"},
				{(int) AuditStatus.Accept, "已通过"},
				{(int) AuditStatus.Denied, "已驳回"},
				{(int) AuditStatus.NotSave,"未保存" },
			{(int)AuditStatus.Cancel,"假期作废" }
			};

		public static readonly Dictionary<int, IEnumerable<string>> StatusAcessable = new Dictionary<int, IEnumerable<string>>()
			{
				{(int)AuditStatus.NotSave,new List<string>(){"Save","Publish", "Delete"} },
				{(int)AuditStatus.NotPublish,new List<string>(){"Publish","Delete"}},
				{(int)AuditStatus.Withdrew, new List<string>(){}},
				{(int)AuditStatus.Auditing, new List<string>(){"Withdrew"}},
				{(int)AuditStatus.AcceptAndWaitAdmin, new List<string>(){"Withdrew"}},
				{(int)AuditStatus.Accept, new List<string>(){"Cancel"}},
				{(int)AuditStatus.Denied, new List<string>(){}},
				{(int)AuditStatus.Cancel,new List<string>(){} },
			};

		private static Dictionary<string, ActionByUserItem> InitActionDic()
		{
			var l = new List<ActionByUserItem>() {
				new ActionByUserItem("Save","保存","success",""),
				new ActionByUserItem("Publish","发布","success","将不可撤回"),
				new ActionByUserItem("Delete","删除","danger","将不再可见，且不可恢复"),
				new ActionByUserItem("Withdrew","撤回","info","将取消审批流程，且不可恢复"),
				new ActionByUserItem("Cancel","作废","danger","认定此次休假数据无效")
			};
			var dic = new Dictionary<string, ActionByUserItem>();
			foreach (var i in l) dic[i.Name] = i;
			return dic;
		}

		private static Dictionary<int, AuditStatusMessage> InitStatusDic()
		{
			var statusMessages = new Dictionary<int, AuditStatusMessage>();
			var type = typeof(AuditStatus).GetFields();
			for (var i = 1; i < type.Length; i++)
			{
				var fieldInfo = type[i];
				var key = (int)fieldInfo.GetRawConstantValue();
				statusMessages.Add(key, new AuditStatusMessage()
				{
					Code = key,
					Color = ColorTranslator.ToHtml(Color.FromArgb(StatusColors[key].ToArgb())),
					Message = fieldInfo.Name,
					Desc = StatusDesc[key],
					Acessable = StatusAcessable[key]
				});
			}

			return statusMessages;
		}
	}
}