using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DAL.Entities.ApplyInfo
{
	public enum ExecuteStatus
	{
		[Description("未设置")]
		NotSet = 0,

		[Description("已设置")]
		BeenSet = 1,

		[Description("已召回")]
		Recall = 2,

		[Description("延误")]
		Delay = 4
	}

	/// <summary>
	/// 休假落实状态
	/// </summary>
	public class ApplyExecuteStatus : HandleModifyReturnStamp
	{
	}
}