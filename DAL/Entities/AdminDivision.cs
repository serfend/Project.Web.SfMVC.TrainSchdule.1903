using Castle.Core.Internal;
using DAL.DTO.User.Social;
using DAL.Entities.UserInfo.Settle;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
	public class AdminDivision
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Code { get; set; }

		public int ParentCode { get; set; }

		/// <summary>
		/// 北京北京市海淀区
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 海淀区
		/// </summary>
		public string ShortName { get; set; }

		public double Longtitude { get; set; }
		public double Latitude { get; set; }
	}

	/// <summary>
	/// 判断是否精确到了省一级
	/// </summary>
	public class AddressCodeOnProvinceAttribute : DataTypeAttribute
	{
		public bool Required { get; }
		private string NowErrorType { get; set; }

		public override string FormatErrorMessage(string name)
		{
			return string.Format(NowErrorType, ErrorMessage);
		}

		public AddressCodeOnProvinceAttribute(bool Required) : base(DataType.Text)
		{
			this.Required = Required;
		}

		public override bool IsValid(object value)
		{
			var vmodel = value as MomentDto;
			if (vmodel == null) return true;

			if (!vmodel.Valid)
			{
				NowErrorType = "{0}地址的有效性必须为启用";
				if (Required) return false;
				return true;
			}
			var v = vmodel.Address?.Code;
			NowErrorType = "当{0}地址填写时，需要选中具体地址";
			return v / 100 >= 1000;
		}
	}
}