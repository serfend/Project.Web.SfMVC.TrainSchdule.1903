﻿namespace BLL.Helpers
{
	public static partial class ActionStatusMessage
	{
		public static class Static
		{
			public static readonly ApiResult TempXlsNotExist=new ApiResult(50000,"未找到Excel模板");
			public static readonly ApiResult XlsNoData = new ApiResult(51000,"生成的Excel文件无数据");
			public static class AdminDivision
			{
				public static readonly ApiResult NoSuchArea = new ApiResult(52100, "无此地址");
				public static readonly ApiResult NoChildArea = new ApiResult(52200, "此地址无下属行政区");
			}
			
		}
	}
}