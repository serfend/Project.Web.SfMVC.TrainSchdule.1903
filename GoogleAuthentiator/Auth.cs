using GoogleAuther;

namespace GoogleAuth
{
	public interface IAuth
	{
		string UserName { get; set; }
		string Password { get; set; }
		int OneTimePassword { get; }

		bool Verify(int code, int tryTime = 0);
	}

	public class Auth : IAuth
	{
		private readonly GoogleAuthMain _auth = new GoogleAuthMain();

		public string UserName
		{
			get => _auth.UserName;
			set => _auth.UserName = value;
		}

		public string Password
		{
			get => _auth.Password;
			set => _auth.Password = value;
		}

		public int OneTimePassword => _auth.Code;

		public bool Verify(int code, int tryTime = 0)
		{
			var timestamp = GoogleAuthMain.GetUnixTimestamp();
			for (var i = 0; i <= tryTime; i++)
			{
				if (_auth.CalculateOneTimePassword(timestamp + 30 * i) == code) return true;
				if (_auth.CalculateOneTimePassword(timestamp - 30 * i) == code) return true;
			}

			return false;
		}
	}
}