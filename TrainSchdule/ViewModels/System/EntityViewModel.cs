using BLL.Helpers;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.System
{
	/// <summary>
	/// 任意 实体返回
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class EntityViewModel<T> : ApiResult where T : class
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="data"></param>
		public EntityViewModel(T data)
		{
			Data = new EntityDataModel<T>(data);
		}

		/// <summary>
		///
		/// </summary>
		public EntityDataModel<T> Data { get; set; }
	}

	/// <summary>
	/// Data直接是值的任意实体返回
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class EntityDirectViewModel<T> : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="data"></param>
		public EntityDirectViewModel(T data)
		{
			Data = data;
		}

		/// <summary>
		/// 实体
		/// </summary>
		public T Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class EntityDataModel<T>
	{
		/// <summary>
		///
		/// </summary>
		public T Model { get; set; }

		/// <summary>
		///
		/// </summary>
		/// <param name="data"></param>

		public EntityDataModel(T data)
		{
			Model = data;
		}
	}

	/// <summary>
	/// 包含授权码信息的任意实体
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class EntityWithAuthDataModel<T> : GoogleAuthViewModel
	{
		/// <summary>
		///
		/// </summary>
		[Required]
		public T Model { get; set; }

		/// <summary>
		///
		/// </summary>
		/// <param name="data"></param>

		public EntityWithAuthDataModel(T data)
		{
			Model = data;
		}
	}

	/// <summary>
	/// 常用逻辑
	/// </summary>
	public static class EntityDataModelExtensions
	{
		/// <summary>
		/// 获取本次查询的操作类型
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="prev"></param>
		/// <returns></returns>
		public static Operation GetOperation(this BaseEntity entity, BaseEntity prev)
		{
			if (entity?.IsRemoved ?? false) return Operation.Remove;
			if (prev == null) return Operation.Create;
			return Operation.Update;
		}
	}
}