namespace DAL.QueryModel
{
	public class QueryVacationStatisticsViewModel
	{
		public QueryByString CompanyId { get; set; }

		/// <summary>
		/// 通过id确定时限
		/// </summary>
		public QueryByString Id { get; set; }

		/// <summary>
		///
		/// </summary>
		public QueryByPage Pages { get; set; }
	}
}