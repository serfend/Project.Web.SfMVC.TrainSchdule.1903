using DAL.Entities;
using TrainSchdule.ViewModels.Company;

namespace TrainSchdule.Extensions
{
	/// <summary>
	/// 
	/// </summary>
	public static class CompanyExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static CompanyChildDataModel ToCompanyModel(this Company model)
		{
			var b=new CompanyChildDataModel()
			{
				Code = model.Code,
				Name = model.Name
			};
			return b;
		}
	}
}
