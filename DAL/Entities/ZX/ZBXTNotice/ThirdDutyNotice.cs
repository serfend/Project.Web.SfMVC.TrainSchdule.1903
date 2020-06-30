using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.ZX.ZBXTNotice
{
	public class ThirdDutyNotice : BaseEntityInt
	{
		/// <summary>
		/// zbxt company code , split by ##
		/// </summary>
		public string ToHandleCompaniesCodes { get; set; }

		/// <summary>
		/// zbxt company name ,split by ##
		/// </summary>

		public string ToHandleCompaniesNames { get; set; }

		/// <summary>
		/// user code send notice
		/// </summary>
		public string NoticeByCode { get; set; }

		/// <summary>
		/// user name send notice
		/// </summary>
		public string NoticeByName { get; set; }

		public string NoticeByCompanyCode { get; set; }

		/// <summary>
		/// real name
		/// </summary>
		public string NoticeByUserName { get; set; }

		public string NoticeByRuleCode { get; set; }

		public string Content { get; set; }
		public string Title { get; set; }
		public DateTime Create { get; set; }

		/// <summary>
		/// raw id
		/// </summary>
		public string Code { get; set; }

		public ThirdDutyNotice(RawNotice model)
		{
			DateTime.TryParse(model.FSSJ, out var create);
			Code = model.NM;
			Content = model.NR;
			Title = model.ZT;

			ToHandleCompaniesCodes = model.YJSBDNM;
			ToHandleCompaniesNames = model.YJSBDMC;
			NoticeByCode = model.FSYHNM;
			Create = create;
			NoticeByCompanyCode = model.FSYHBDNM;
			NoticeByName = model.FSDW;
			NoticeByRuleCode = model.ZBLBNM;
			NoticeByUserName = model.FSR;
		}
	}

	public class RawNotice
	{
		public string YJSBDNM { get; set; }
		public string FSYHNM { get; set; }
		public string FSSJ { get; set; }
		public string NM { get; set; }
		public string YJSBDMC { get; set; }
		public string FSDW { get; set; }
		public string ZBLBNM { get; set; }
		public string NR { get; set; }
		public string FSR { get; set; }
		public string FSYHBDNM { get; set; }
		public string ZT { get; set; }
	}
}