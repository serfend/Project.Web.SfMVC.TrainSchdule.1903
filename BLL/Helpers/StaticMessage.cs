namespace BLL.Helpers
{
	public static partial class ActionStatusMessage
	{
		public static class StaticMessage
		{
			public static readonly ApiResult TempXlsNotExist = new ApiResult(51000, "未找到Excel模板");
			public static readonly ApiResult XlsNoData = new ApiResult(51000, "生成的Excel文件无数据");
			public static readonly ApiResult FileNotExist = new ApiResult(51001, "文件不存在");
			public static readonly ApiResult CacheIsInvalid = new ApiResult(51002, "断点续传失败，因为缓存已失效");
			public static readonly ApiResult FileSizeInvalid = new ApiResult(51003, "文件尺寸与要求不符");
			public static readonly ApiResult ResourceAllReadyExist = new ApiResult(51004, "资源已经存在");
			public static readonly ApiResult ResourceNotExist = new ApiResult(51005, "资源不存在");
			public static readonly ApiResult FileAllReadyExist = new ApiResult(51006, "文件已存在");
			public static readonly ApiResult ResourceOutofSize = new ApiResult(51007, "资源尺寸不符");
			public static readonly ApiResult IdIsNull = new ApiResult(51008, "目标id不合法");
			
			public static class AdminDivision
			{
				public static readonly ApiResult NoSuchArea = new ApiResult(52100, "无此地址");
				public static readonly ApiResult NoChildArea = new ApiResult(52200, "此地址无下属行政区");
			}

			public static class QrCode
			{
				public static readonly ApiResult NoData = new ApiResult(53100, "二维码无数据");
			}

			public static class System
			{
				public static readonly ApiResult SystemBusy = new ApiResult(50000, "系统繁忙导致操作失败，请重试");
			}
		}
	}
}