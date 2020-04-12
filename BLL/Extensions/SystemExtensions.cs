using BLL.Helpers;
using DAL.DTO.System;
using DAL.Entities.FileEngine;
using System;
using System.Security.Cryptography;
using System.Text;

namespace BLL.Extensions
{
	public static class SystemExtensions
	{
		public static string ToBase64(this byte[] data)
		{
			if (data == null) return null;
			return Convert.ToBase64String(data);
		}

		public static byte[] FromBase64ToBytes(this string base64)
		{
			var checkIfBase64CharContain = base64.IndexOf("base64");
			if (checkIfBase64CharContain >= 0) base64 = base64.Substring(checkIfBase64CharContain + 7);
			return Convert.FromBase64String(base64);
		}

		private const string privateKey = "MIICXgIBAAKBgQCz3lBSqu05sK6r3SDCr2z0lT19j4LBbWbapEvv37paxbwmkvA5E/nr/VD9Hw2jueBt9NyEdnzWEgN+WmRF1GUYBQFL6YWneFkovgpLA8tgXHEojePAMfgMb+hoYHoV90MUQwANDbt0gg4nnlRxZB+WtZc5CUQT5x7ckCs5+iQNTwIDAQABAoGBAKSOgcH/6uTaxhMqTWyP/giN+SHEiAXaxzzFD0w3zVB6kzZfPDOcGQxURyIspNfjmHZAjPcLSA65kESrAg340Trs00k9i1JfzYp4hc/r85yBVTp5ljWp8kPWRpfJBK3yzBok4qvGbIpJHlLrENFnVUd0dkPXKaOXZs3+mZ1GWTIRAkEA5gFj/QkpGWa/PRLSJ55ptdiIVjxXDhdNVJsozs4UcbYr/CIEUiQA6OqYNOWr8shAdQM1g65PvDYWGFJQq42qvQJBAMgyVr5P1Vj2EwahnbDtD9Zzngchcv5sv9sVlI3NNhD4tkzxntc01ikOzzy9M+x3cP1tHavv8lxgNWnWAi6hnvsCQBprfnjKXJY2XzE8wDcc0ze4L7D4LWfI9XEKgZ1/volxS4wivCxTRmd6yxEIcL/qkLzgKX1+wFn2PIN+sRWDqGECQQCazbofvXHXMajypsReTGHDzXF0SBw4uvT8P0q4/+b/5qJpCyltdjDoXMhJSnC9OHsJrHeWPZvmbIrBBTh4wIdDAkEAirO5n1K88opa8chywQxfdnKqt0BJq/x+Xp2W9V4p61PucMKSDQQ3Ytf47JUfi/17WeQeTc5L6RBDxVF2Hqjk4Q==";
		private const string publicKey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCz3lBSqu05sK6r3SDCr2z0lT19j4LBbWbapEvv37paxbwmkvA5E/nr/VD9Hw2jueBt9NyEdnzWEgN+WmRF1GUYBQFL6YWneFkovgpLA8tgXHEojePAMfgMb+hoYHoV90MUQwANDbt0gg4nnlRxZB+WtZc5CUQT5x7ckCs5+iQNTwIDAQAB";

		/// <summary>
		/// 通过加密密文获取原文
		/// </summary>
		/// <param name="cipper">格式为md5(username){密文}{今天日期}</param>
		/// <param name="username"></param>
		/// <returns></returns>
		public static string FromCipperToString(this string cipper, string username)
		{
			using (var md5 = MD5.Create())
			{
				var result = md5.ComputeHash(Encoding.UTF8.GetBytes(username));
				var rawMd5 = BitConverter.ToString(result);
				string md5Str = rawMd5.Replace("-", "");
				var rsa = new RsaHelper(RSAType.RSA2, Encoding.UTF8, privateKey, publicKey);
				var decryptStr = rsa.Decrypt(cipper);
				if (decryptStr == null || decryptStr.Length <= md5Str.Length ||
					DateTime.Now.ToString("yyyyMMdd") != decryptStr.Substring(0, 8) ||
					decryptStr.Substring(decryptStr.Length - md5Str.Length, md5Str.Length) != md5Str.ToLower()) return null;
				return decryptStr.Substring(8, decryptStr.Length - 8 - md5Str.Length);
			}
		}

		public static FileInfoVDto ToVdto(this UserFileInfo model, FileInfoVDto raw = null)
		{
			if (model == null) return null;
			if (raw == null) raw = new FileInfoVDto();
			raw.Id = model.Id;
			raw.Create = model.Create;
			raw.IsRemoved = model.IsRemoved;
			raw.IsRemovedDate = model.IsRemovedDate;
			raw.FromClient = model.FromClient;
			raw.LastModefy = model.LastModefy;
			raw.Length = model.Length;
			raw.Name = model.Name;
			raw.Path = model.Path;
			return raw;
		}
	}
}