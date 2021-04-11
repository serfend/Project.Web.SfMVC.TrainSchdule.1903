using Microsoft.International.Converters.PinYinConverter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;

namespace DAL.Entities.UserInfo
{

	public static class RealnameExtensions
    {
		public static List<string> Descartes(this List<List<string>> list,int nowIndex = 0)
        {
			var result = new List<string>() { "" };
			if (list.Count <= nowIndex) return result;
			var nowList = list[nowIndex];
			var subList = list.Descartes(nowIndex + 1);
			nowList.ForEach(now =>
			{
				subList.ForEach(sub =>
				{
					result.Add($"{now}{sub}");
				});
			});
			return result.Distinct().ToList();
        }
	}
	public class UserBaseInfo : BaseEntityGuid
	{
		[Required(ErrorMessage = "未输入身份证")]

		/// <summary>
		/// 居民身份证
		/// </summary>
		[IDCard]
		public string Cid { get; set; }
		private string realName;
		[Required(ErrorMessage = "未输入真实姓名")]
		public string RealName { get=> realName; set {
				realName = value;
				var chars = value
						.ToCharArray()
						.Select(c => {
							var result = new List<string>() { "" };
							if (c < 256) return result;
							var py = new ChineseChar(c);
							var list =py.Pinyins.ToList();
							list.ForEach(v => {
								if (v == null) return;
								v = v.Substring(0, v.Length - 1);
								result.Add(v);
								result.Add(v.Substring(0, 1));
							});
							result = result.Select(i => i.ToLower()).Distinct().ToList();
							return result;
						})
						.ToList();
				PinYin = string.Join('#', chars.Descartes());
			} }
		/// <summary>
		/// 【冗余】真实姓名的拼音组合
		/// </summary>
		public string PinYin { get; set; }
		[Required(ErrorMessage = "未输入性别")]
		public GenderEnum Gender { get; set; }

		[Required(ErrorMessage = "未输入籍贯")]
		public string Hometown { get; set; }

		[Required(ErrorMessage = "未输入工作时间")]
		/// <summary>
		/// 工作/入伍时间
		/// </summary>
		public DateTime Time_Work { get; set; }

		[Required(ErrorMessage = "未输入生日")]

		/// <summary>
		/// 出生日期
		/// </summary>
		public DateTime Time_BirthDay { get; set; }

		[Required(ErrorMessage = "未输入党团时间")]

		/// <summary>
		/// 党团时间
		/// </summary>
		public DateTime Time_Party { get; set; }

		/// <summary>
		/// 是否修改过密码
		/// </summary>
		[Column("PasswodModify")]
		public bool PasswordModify { get; set; }
	}

	public enum GenderEnum
	{
		[Description("未知")]
		Unknown = 0,

		[Description("男")]
		Male = 1,

		[Description("女")]
		Female = 2,
	}


	public class IDCardAttribute : ValidationAttribute
	{
		public override bool IsValid(object value)
        {
			var result = Convert.ToString(value, CultureInfo.CurrentCulture).CheckIDCard(out var reason);
			this.ErrorMessage = reason;
			return result;
		}
    }

	/// <summary>
	/// 验证身份证号码类
	/// </summary>
	public static class IDCardValidationExtension
	{
		/// <summary>
		/// 验证身份证合理性
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public static bool CheckIDCard(this string idNumber,out string reason)
		{
			reason = null;
			if (idNumber == null)
            {
				reason = "未识别到身份证号";
				return false;
			}
			if (idNumber.Length == 18)
			{
				reason = CheckIDCard18(idNumber);
				return reason == null;
			}
			else if (idNumber.Length == 15)
			{
				reason = CheckIDCard15(idNumber);
				return reason == null;
			}
			else {
				reason = "身份证位数无效";
				return false;
			}
		}

		/// <summary>
		/// 18位身份证号码验证
		/// </summary>
		private static string CheckIDCard18(this string idNumber)
		{
            if (long.TryParse(idNumber.Remove(17), out long n) == false
                || n < Math.Pow(10, 16) || long.TryParse(idNumber.Replace('x', '0').Replace('X', '0'), out _) == false)
                return "位数验证无效";//数字验证
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
			if (!address.Contains(idNumber.Remove(2), StringComparison.CurrentCulture))
				return "省份代码无效";//省份验证
			string birth = idNumber.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            if (DateTime.TryParse(birth, out _) == false)
				return "出生日期无效";//生日验证
			string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
			string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
			char[] Ai = idNumber.Remove(17).ToCharArray();
			int sum = 0;
			for (int i = 0; i < 17; i++)
			{
				sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
			}
            Math.DivRem(sum, 11, out int y);
            if (arrVarifyCode[y] != idNumber.Substring(17, 1).ToLower())
				return $"校验码错误,应为{arrVarifyCode[y]}";//校验码验证
			return null;//符合GB11643-1999标准
		}

		/// <summary>
		/// 16位身份证号码验证
		/// </summary>
		private static string CheckIDCard15(string idNumber)
		{
            if (long.TryParse(idNumber, out long n) == false || n < Math.Pow(10, 14))
                return "位数验证无效";//数字验证
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
			if (address.IndexOf(idNumber.Remove(2)) == -1)
				return "省份代码无效";//省份验证
			string birth = idNumber.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            if (DateTime.TryParse(birth, out _) == false)
                return "出生日期无效";//生日验证
            return null;
		}
	}
}