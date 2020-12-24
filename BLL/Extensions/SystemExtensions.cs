using BLL.Helpers;
using BLL.Interfaces.Common;
using DAL.DTO.System;
using DAL.Entities.FileEngine;
using System;
using System.Security.Cryptography;
using System.Text;

namespace BLL.Extensions
{
	public static class SystemExtensions
	{
		public static DateTime XjxtNow(this DateTime date) => date;

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

		/// <summary>
		/// 通过加密密文获取原文
		/// </summary>
		/// <param name="cipper">格式为md5(username){密文}{今天日期}</param>
		/// <param name="username"></param>
		/// <param name="cipperServices">获取密钥</param>
		/// <returns></returns>
		public static string FromCipperToString(this string cipper, string username, ICipperServices cipperServices)
		{
			try
			{
				string md5Str = username.ToMd5();
				var rsa = new RsaHelper(RSAType.RSA2, Encoding.UTF8, cipperServices.PrivateKey, cipperServices.PublicKey);
				var decryptStr = rsa.Decrypt(cipper);
				if (decryptStr == null || decryptStr.Length <= md5Str.Length ||
					DateTime.Now.ToString("yyyyMMdd") != decryptStr.Substring(0, 8) ||
					decryptStr.Substring(decryptStr.Length - md5Str.Length, md5Str.Length) != md5Str) return null;
				return decryptStr.Substring(8, decryptStr.Length - 8 - md5Str.Length);
			}
			catch (Exception)
			{
				throw new ActionStatusMessageException(ActionStatusMessage.Account.Auth.Invalid.CipperInvalid);
			}
		}

		public static string ToMd5(this string raw)
		{
			using (var md5 = MD5.Create())
			{
				return BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(raw))).Replace("-", "").ToLower();
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
			raw.LastModify = model.LastModify;
			raw.Length = model.Length;
			raw.Name = model.Name;
			raw.Path = model.Path;
			return raw;
		}
	}
}