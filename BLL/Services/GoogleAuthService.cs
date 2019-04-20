using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using BLL.Interfaces;
using GoogleAuth;
using GoogleAuther;
using TrainSchdule.BLL.Interfaces;
using TrainSchdule.BLL.Services;
using TrainSchdule.DAL.Entities;

namespace BLL.Services
{
	public class GoogleAuthService:IGoogleAuthService
	{
		private Auth auth=new Auth();
		private User _user;
		public string Issuer { get; set; }
		private User currentUser => _user ?? (_user = CurrentUserService.CurrentUser);

		public GoogleAuthService(ICurrentUserService currentUserService)
		{
			CurrentUserService = currentUserService;
			Issuer = "XXTX2U";
		}
		public bool Verify(int code,string username=null,string password=null)
		{
			InitCode(username, password);
			return auth.Verify(code,5);
		}

		public int Code(string userName = null, string password = null)
		{
			InitCode(userName, password);
			return auth.OneTimePassword;
		}


		public void InitCode(string username = null, string password = null)
		{
			username = username ?? currentUser.UserName;
			auth.UserName = username;
			GetAuthKey(password);
		}
		public string GetAuthKey(string password=null)
		{
			password = password ?? currentUser.AuthKey;
			password = password ?? currentUser.UserName;
			auth.Password = Base32.ToString(new HMACSHA1(new byte[]{17,24,33,45}).ComputeHash(Encoding.UTF8.GetBytes(password)));
			return auth.Password;
		}

		public string Url => $"otpauth://totp/{auth.UserName}?secret={auth.Password}&issuer={Issuer}";

		public ICurrentUserService CurrentUserService { get; set; }

		public void Dispose()
		{
			CurrentUserService?.Dispose();
		}
	}
}
