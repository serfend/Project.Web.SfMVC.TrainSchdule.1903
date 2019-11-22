using DAL.DTO.Apply;
using DAL.Entities.ApplyInfo;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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
	
		public static Dictionary<int, AuditStatusMessage> StatusDic { get; } = InitStatusDic();
		public static Dictionary<int,Color> StatusColors { get; set; }
		public static Dictionary<int,string> StatusDesc { get; set; }
		public static Dictionary<int, IEnumerable<string>> StatusAcessable { get; set; }


		private static Dictionary<int, AuditStatusMessage> InitStatusDic()
		{
			StatusColors = new Dictionary<int, Color>
			{
				{(int)AuditStatus.NotPublish, Color.DarkGray},
				{(int)AuditStatus.Auditing, Color.Coral},
				{(int)AuditStatus.Withdrew, Color.Gray},
				{(int)AuditStatus.AcceptAndWaitAdmin, Color.DeepSkyBlue},
				{(int)AuditStatus.Accept, Color.LawnGreen},
				{(int)AuditStatus.Denied, Color.Red},
				{(int)AuditStatus.NotSave,Color.Black }
			};
			StatusDesc=new Dictionary<int, string>()
			{
				{(int)AuditStatus.NotPublish, "未发布"},
				{(int)AuditStatus.Auditing, "审核中"},
				{(int)AuditStatus.Withdrew, "已撤回"},
				{(int)AuditStatus.AcceptAndWaitAdmin, "人力审核"},
				{(int)AuditStatus.Accept, "已通过"},
				{(int)AuditStatus.Denied, "已驳回"},
				{(int)AuditStatus.NotSave,"未保存" }
			};
			StatusAcessable=new Dictionary<int, IEnumerable<string>>()
			{
				{(int)AuditStatus.NotPublish,new List<string>(){"Publish","Delete"}},
				{(int)AuditStatus.Auditing, new List<string>(){"Withdrew","Delete"}},
				{(int)AuditStatus.Withdrew, new List<string>(){"Delete"}},
				{(int)AuditStatus.AcceptAndWaitAdmin, new List<string>(){"Withdrew","Delete"}},
				{(int)AuditStatus.Accept, new List<string>(){""}},
				{(int)AuditStatus.Denied, new List<string>(){"Delete"}},
				{(int)AuditStatus.NotSave,new List<string>(){"Delete","Save","Publish"} }
			};
			var statusMessages = new Dictionary<int, AuditStatusMessage>();
			var type = typeof(AuditStatus).GetFields();
			for (var i=1;i<type.Length ;i++)
			{
				var fieldInfo = type[i];
				var key =(int)fieldInfo.GetRawConstantValue();
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
