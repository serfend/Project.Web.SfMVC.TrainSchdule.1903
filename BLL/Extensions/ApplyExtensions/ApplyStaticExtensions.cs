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
	}
}