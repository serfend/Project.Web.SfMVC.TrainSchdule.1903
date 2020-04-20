namespace DAL.QueryModel
{
	public class QueryVacationStatisticsDataModel
	{
		public QueryByString CompanyId { get; set; }

		/// <summary>
		/// 通过id确定时限
		/// </summary>
		public QueryByString Id { get; set; }
	}
}