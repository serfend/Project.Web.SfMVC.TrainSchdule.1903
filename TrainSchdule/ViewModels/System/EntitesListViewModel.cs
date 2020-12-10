using BLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.System
{
	/// <summary>
	/// 任意实体列表的一般返回
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class EntitiesListViewModel<T> : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public EntitiesListDataModel<T> Data { get; set; }

		/// <summary>
		///
		/// </summary>
		public EntitiesListViewModel() { }

		/// <summary>
		/// 依据实体生成
		/// </summary>
		/// <param name="model"></param>
		/// <param name="totalCount"></param>
		public EntitiesListViewModel(IEnumerable<T> model, int totalCount = -1)
		{
			this.Data = new EntitiesListDataModel<T>(model, totalCount);
		}
	}

	/// <summary>
	/// 任意实体列表
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class EntitiesListDataModel<T>
	{
		/// <summary>
		/// 列表
		/// </summary>
		public IEnumerable<T> List { get; set; }

		/// <summary>
		/// 数据库总量，配合SplitPage查询
		/// </summary>
		public int TotalCount { get; set; }

		/// <summary>
		///
		/// </summary>
		public EntitiesListDataModel() { }

		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <param name="totalCount"></param>
		public EntitiesListDataModel(IEnumerable<T> model, int totalCount = -1)
		{
			this.List = model;
			TotalCount = totalCount > -1 ? totalCount : model?.Count() ?? -1;
		}
	}
}