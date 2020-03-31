using BLL.Helpers;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.ViewModels.Account
{
	/// <summary>
	///
	/// </summary>
	public class UserActionDictionaryViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public UserActionDictionaryDataModel Data { get; set; }
	}

	/// <summary>
	/// 所有可能的状态
	/// </summary>
	public class UserActionDictionaryDataModel
	{
		/// <summary>
		/// 状态值-状态实体
		/// </summary>
		public Dictionary<int, UserActionDictionaryItem> List { get; set; }
	}

	/// <summary>
	/// 单个UserActionRank项
	/// </summary>
	public class UserActionDictionaryItem
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="foreColor"></param>
		public UserActionDictionaryItem(string name, int value, string foreColor)
		{
			Name = name;
			Value = value;
			ForeColor = foreColor;
		}

		/// <summary>
		///
		/// </summary>
		public UserActionDictionaryItem()
		{
		}

		/// <summary>
		/// 状态名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 状态值
		/// </summary>
		public int Value { get; set; }

		/// <summary>
		/// 前端显示颜色
		/// </summary>
		public string ForeColor { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public static class UADI
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="color"></param>
		/// <returns></returns>
		public static UserActionDictionaryItem Build(string name, int value, Color color)
		{
			var colorStr = Color.FromArgb(color.ToArgb());
			return new UserActionDictionaryItem()
			{
				Name = name,
				ForeColor = ColorTranslator.ToHtml(colorStr),
				Value = value
			};
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="rank"></param>
		/// <param name="color"></param>
		/// <returns></returns>
		public static UserActionDictionaryItem Build(this ActionRank rank, Color color) => Build(rank.ToString(), (int)rank, color);

		/// <summary>
		///
		/// </summary>
		/// <param name="rank"></param>
		/// <param name="color"></param>
		/// <returns></returns>
		public static UserActionDictionaryItem Build(this UserOperation rank, Color color) => Build(rank.ToString(), (int)rank, color);
	}
}