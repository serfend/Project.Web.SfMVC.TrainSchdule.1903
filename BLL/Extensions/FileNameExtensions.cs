using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Extensions
{
	public static class FileNameExtensions
	{
		/// <summary>
		/// 为字符串中的非英文字符编码
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string ToHexString(this string s)
		{
			char[] chars = s.ToCharArray();
			StringBuilder builder = new StringBuilder();
			for (int index = 0; index < chars.Length; index++)
			{
				bool needToEncode = NeedToEncode(chars[index]);
				if (needToEncode)
				{
					string encodedString = ToHexString(chars[index]);
					builder.Append(encodedString);
				}
				else
				{
					builder.Append(chars[index]);
				}
			}

			return builder.ToString();
		}

		/// <summary>
		///指定 一个字符是否应该被编码
		/// </summary>
		/// <param name="chr"></param>
		/// <returns></returns>
		private static bool NeedToEncode(this char chr)
		{
			string reservedChars = "$-_.+!*'(),@=&";

			if (chr > 127)
				return true;
			if (char.IsLetterOrDigit(chr) || reservedChars.IndexOf(chr) >= 0)
				return false;

			return true;
		}

		/// <summary>
		/// 为非英文字符串编码
		/// </summary>
		/// <param name="chr"></param>
		/// <returns></returns>
		private static string ToHexString(this char chr)
		{
			UTF8Encoding utf8 = new UTF8Encoding();
			byte[] encodedBytes = utf8.GetBytes(chr.ToString());
			StringBuilder builder = new StringBuilder();
			for (int index = 0; index < encodedBytes.Length; index++)
			{
				builder.AppendFormat("%{0}", Convert.ToString(encodedBytes[index], 16));
			}
			return builder.ToString();
		}
	}
}
