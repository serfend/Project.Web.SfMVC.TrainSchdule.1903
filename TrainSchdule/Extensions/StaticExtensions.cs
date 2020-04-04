using DAL.Entities;
using TrainSchdule.ViewModels.Static;

namespace TrainSchdule.Extensions
{
	/// <summary>
	///
	/// </summary>
	public static class StaticExtensions
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static LocationChildNodeDataModel ToDataModel(this AdminDivision model)
		{
			var b = new LocationChildNodeDataModel()
			{
				Code = model.Code,
				Name = model.ShortName
			};
			return b;
		}
	}
}