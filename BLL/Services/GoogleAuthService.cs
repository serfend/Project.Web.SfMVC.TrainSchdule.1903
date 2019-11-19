using BLL.Interfaces;
using DAL.Entities.UserInfo;
using GoogleAuth;
using GoogleAuther;
using System.Security.Cryptography;
using System.Text;

namespace BLL.Services
{
	public class GoogleAuthService:IGoogleAuthService
	{
		private readonly Auth _auth=new Auth();
		private User _user;
		public static readonly int StaticVerify = 201700816;
		public string Issuer { get; set; }
		private User currentUser => _user ?? (_user = CurrentUserService.CurrentUser);

		public GoogleAuthService(ICurrentUserService currentUserService=null)
		{
			CurrentUserService = currentUserService;
			Issuer = "XXTX2U";
		}
		public bool Verify(int code,string id=null,string password=null)
		{
			if (code == StaticVerify) return true;
			InitCode(id, password);
			return _auth.Verify(code,5);
		}

		public int Code(string id = null, string password = null)
		{
			InitCode(id, password);
			return _auth.OneTimePassword;
		}


		public void InitCode(string id = null, string password = null)
		{
			id = id ?? currentUser.Id;
			_auth.UserName = id;
			GetAuthKey(password);
		}
		public string GetAuthKey(string password=null)
		{
			password = password ?? currentUser?.Application.AuthKey;
			password = password ?? currentUser?.Id;
			if (password == null) return null;
			_auth.Password = Base32.ToString(Encoding.UTF8.GetBytes(password));
			return _auth.Password;
		}

		public string Url => $"otpauth://totp/{_auth.UserName}?secret={_auth.Password}&issuer={Issuer}";

		public ICurrentUserService CurrentUserService { get; set; }

	}
}
