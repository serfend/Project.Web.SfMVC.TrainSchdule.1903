
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
namespace GoogleAuther
{
	public class GoogleAuthMain
	{
		public GoogleAuthMain()
		{

			Secret = new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x21, 0xDE, 0xAD, 0xBE, 0xEF };
			UserName = "user@host.com";
		}

		public string UserName { get; set; }
		public string Password
		{
			get => Base32.ToString(Secret);
			set => Secret = Base32.ToBytes(value);
		}

		public byte[] Secret { get; set; }

		public int Code => CalculateOneTimePassword();


		private byte[] Hmac { get; set; }
		private byte[] HmacPart1=> Hmac.Take(Offset).ToArray();

		private byte[] HmacPart2=> Hmac.Skip(Offset).Take(4).ToArray();

		private byte[] HmacPart3=> Hmac.Skip(Offset + 4).ToArray();

		private int Offset { get; set; }


		public int CalculateOneTimePassword(long timestamp=0)
		{
			if(timestamp==0)  timestamp =GetUnixTimestamp();
			timestamp /= 30;
			var data = BitConverter.GetBytes(timestamp).Reverse().ToArray();
			Hmac = new HMACSHA1(Secret).ComputeHash(data);
			Offset = Hmac.Last() & 0x0F;
			var oneTimePassword = (
				((Hmac[Offset + 0] & 0x7f) << 24) |
				((Hmac[Offset + 1] & 0xff) << 16) |
				((Hmac[Offset + 2] & 0xff) << 8) |
				(Hmac[Offset + 3] & 0xff)
					) % 1000000;
			return oneTimePassword;
		}

		public static long GetUnixTimestamp()
		{
			return Convert.ToInt64(Math.Round((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds));
		}

	}
}
