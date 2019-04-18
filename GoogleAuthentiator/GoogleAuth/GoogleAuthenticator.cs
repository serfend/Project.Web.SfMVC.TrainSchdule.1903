using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace GoogleAuther
{
	class GoogleAuthenticator
	{
		public GoogleAuthenticator()
		{
			Secret = new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x21, 0xDE, 0xAD, 0xBE, 0xEF };
			Identity = "user@host.com";

		}

		private int _secondsToGo;

		public int SecondsToGo
		{
			get { return _secondsToGo; }
			private set { _secondsToGo = value; if (SecondsToGo == 30) CalculateOneTimePassword(); }
		}


		private string _identity;

		public string Identity
		{
			get { return _identity; }
			set { _identity = value;  CalculateOneTimePassword(); }
		}

		public string SecretBase32
		{
			get { return Base32.ToString(Secret); }
			set { try { Secret = Base32.ToBytes(value); } catch { }; }
		}

		private byte[] _secret;

		public byte[] Secret
		{
			get { return _secret; }
			set { _secret = value; CalculateOneTimePassword(); }
		}

		public string QRCodeUrl
		{
			get { return GetQRCodeUrl(); }
		}

		private Int64 _timestamp;

		private byte[] _hmac;

		public byte[] HmacPart1
		{
			get { return Hmac.Take(Offset).ToArray(); }
		}

		public byte[] HmacPart2
		{
			get { return Hmac.Skip(Offset).Take(4).ToArray(); }
		}

		public byte[] HmacPart3
		{
			get { return Hmac.Skip(Offset + 4).ToArray(); }
		}

		public int Offset {
			get
			{
				return _offset;
			}
			set
			{
				_offset = value;
			}
		}
		public int OneTimePassword {
			get
			{
				return _oneTimePassword;
			}
			set
			{
				 _oneTimePassword=value;
			}
		}
		public byte[] Hmac {
			set
			{
				_hmac = value;
			}
			get
			{
				return _hmac;
			}
		}
		public long Timestamp {
			get
			{
				return _timestamp;
			}
			set
			{
				_timestamp = value;
			}
		}

		private int _offset;
		private int _oneTimePassword;


		private string GetQRCodeUrl()
		{
			// https://code.google.com/p/google-authenticator/wiki/KeyUriFormat
			return String.Format("https://www.google.com/chart?chs=200x200&chld=M|0&cht=qr&chl=otpauth://totp/{0}%3Fsecret%3D{1}", Identity, SecretBase32);
		}

		private void CalculateOneTimePassword()
		{
			// https://tools.ietf.org/html/rfc4226
			Timestamp = Convert.ToInt64(GetUnixTimestamp() / 30);
			var data = BitConverter.GetBytes(Timestamp).Reverse().ToArray();
			Hmac = new HMACSHA1(Secret).ComputeHash(data);
			Offset = Hmac.Last() & 0x0F;
			OneTimePassword = (
				((Hmac[Offset + 0] & 0x7f) << 24) |
				((Hmac[Offset + 1] & 0xff) << 16) |
				((Hmac[Offset + 2] & 0xff) << 8) |
				(Hmac[Offset + 3] & 0xff)
					) % 1000000;
		}

		private static Int64 GetUnixTimestamp()
		{
			return Convert.ToInt64(Math.Round((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds));
		}
	}
}
