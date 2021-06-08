using BLL.Helpers;
using System.Collections.Generic;

namespace TrainSchdule.ViewModels.System
{
    /// <summary>
    /// 常规数据更新格式
    /// </summary>
    public interface ICommonDataUpdate
    {
        /// <summary>
        /// 是否允许覆盖
        /// </summary>
        bool AllowOverwrite { get; set; }
    }
    /// <summary>
    /// 批量请求情况回复
    /// </summary>
    public class ResponseStatusOrModelExceptionViweModel : ApiResult
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="status"></param>
        public ResponseStatusOrModelExceptionViweModel(ApiResult status) : base(status.Status, status.Message) { }

        /// <summary>
        /// 返回键对应的错误
        /// </summary>
        public Dictionary<string, ApiResult> StatusException { get; set; }

        /// <summary>
        /// 键对应的格式错误
        /// </summary>
        public Dictionary<string, ModelStateExceptionDataModel> ModelStateException { get; set; }
    }
}