using BLL.Interfaces;
using DAL.Entities.UserInfo;
using GoogleAuth;
using GoogleAuther;
using Microsoft.Extensions.Configuration;
using System;
using System.Security.Cryptography;
using System.Text;

namespace BLL.Services
{
	public class GoogleAuthService : IGoogleAuthService
	{
		private readonly IUsersService usersService;
		private readonly IConfiguration configuration;

		public GoogleAuthService(IUsersService usersService = null, IConfiguration configuration = null)
		{
			this.usersService = usersService;
			this.configuration = configuration;
		}

		public int Code(string username)
		{
			var m = Init(username);
			return m.Main.OneTimePassword;
		}

		public string GetAuthKey(string username)
		{
			var m = Init(username);
			return m.Main.Password;
		}

		public AuthServiceModel Init(string username)
		{
			if (username == null) username = "root";
			var u = usersService.Get(username);
			var password = u?.Application?.AuthKey;
			if (password == null) password = configuration.GetSection("Configuration").GetSection("Permission")["DefaultPassword"] ?? "invalid@user";

			using (var md5 = SHA256.Create())
			{
				var result = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
				var rawMd5 = BitConverter.ToString(result).Replace("-", "").ToLower();
				rawMd5 = rawMd5.Substring(15, 20);
				var a = new AuthServiceModel(username, rawMd5);
				return a;
			}
		}

		public bool Verify(int code, string username)
		{
			var m = Init(username);
			var permit = m.Main.Verify(code, 5) || m.Main.Verify(code, 5);
			return permit;
		}
	}

	public class AuthServiceModel
	{
		public Auth Main { get; set; }

		public AuthServiceModel(string username, string password)
		{
			Main = new Auth()
			{
				UserName = username,
				Password = Base32.ToString(Encoding.UTF8.GetBytes(password))
			};
		}
	}
}