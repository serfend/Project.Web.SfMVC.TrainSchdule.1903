using System;
using System.Collections.Generic;
using System.Text;
using BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using TrainSchdule.DAL.Interfaces;

namespace BLL.Services
{
	public class VerifyService:IVerifyService
	{
		#region Fileds

		private static Dictionary<byte[], int> verifyBase = new Dictionary<byte[], int>();
		private readonly IUnitOfWork _unitOfWork;

		private readonly IHttpContextAccessor _httpContextAccessor;

		#endregion
		#region Disposing

		private bool _isDisposed;

		public VerifyService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
		{
			_unitOfWork = unitOfWork;
			_httpContextAccessor = httpContextAccessor;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public virtual void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				if (disposing)
				{
					_unitOfWork.Dispose();
				}

				_isDisposed = true;
			}
		}

		~VerifyService()
		{
			Dispose(false);
		}

		#endregion

		private const string KEY_VerifyCode = "verify-code";
		private int Generate()
		{
			return new Random().Next(0, 100000);
		}
		public void Get()
		{
			var newCode=Guid.NewGuid().ToByteArray();
			var newCodeValue = Generate();
			verifyBase.Add(newCode, newCodeValue);
			_httpContextAccessor.HttpContext.Session.Set(KEY_VerifyCode, newCode);
		}

		public bool Verify(int code)
		{
			_httpContextAccessor.HttpContext.Session.TryGetValue(KEY_VerifyCode,out var codeIndex);
			if (codeIndex == null) return false;
			verifyBase.TryGetValue(codeIndex, out var value);
			return code == value;
		}
	}
}
