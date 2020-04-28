using BLL.Interfaces;
using DAL.Entities.UserInfo;
using GoogleAuth;
using GoogleAuther;
using System;
using System.Security.Cryptography;
using System.Text;

namespace BLL.Services
{
	public class GoogleAuthService : IGoogleAuthService
	{
		private readonly int StaticVerify = 199500616;
		private readonly IUsersService usersService;

		public GoogleAuthService(IUsersService usersService = null)
		{
			this.usersService = usersService;
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
			if (password == null) password = "invalid@user";

			var normalUserName = $"{username}@{DateTime.Now.ToString("yyyyMMdd")}";
			using (var md5 = MD5.Create())
			{
				var result = md5.ComputeHash(Encoding.UTF8.GetBytes(normalUserName));
				var rawMd5 = BitConverter.ToString(result).Replace("-", "").ToLower();
				var a = new AuthServiceModel(username, password, rawMd5);
				return a;
			}
		}

		public bool Verify(int code, string username)
		{
			var m = Init(username);
			var permit = m.Main.Verify(code, 5) || m.Second.Verify(code, 5);
			// 若认证失败，则判断当前密码是否和root相同
			if (!permit && username != "root")
			{
				var root = Init("root");
				permit = root.Main.Verify(code, 5) || root.Second.Verify(5);
			}
			return permit;
		}
	}

	public class AuthServiceModel
	{
		public Auth Main { get; set; }
		public Auth Second { get; set; }

		public AuthServiceModel(string username, string password, string secondPassword = null)
		{
			Main = new Auth()
			{
				UserName = username,
				Password = Base32.ToString(Encoding.UTF8.GetBytes(password))
			};
			Second = new Auth()
			{
				UserName = username,
				Password = Base32.ToString(Encoding.UTF8.GetBytes(secondPassword ?? password))
			};
		}
	}
}