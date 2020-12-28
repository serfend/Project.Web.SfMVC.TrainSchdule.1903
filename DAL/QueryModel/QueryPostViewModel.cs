namespace DAL.QueryModel
{
	public class QueryPostViewModel
	{
		public QueryByPage Page { get; set; }
		public QueryByDate Create { get; set; }
		public QueryByString CreateBy { get; set; }
	}

	public class QueryContentViewModel
	{
		public QueryByPage Page { get; set; }
		public QueryByDate Create { get; set; }
		public QueryByString CreateBy { get; set; }
		public QueryByString ReplyTo { get; set; }
		public QueryByString ReplySubject { get; set; }
	}
}