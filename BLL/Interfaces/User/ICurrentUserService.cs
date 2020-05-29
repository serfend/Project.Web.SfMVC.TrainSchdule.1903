using System;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Http;

namespace BLL.Interfaces
{
    /// <summary>
    /// Interface for getting current user services.
    /// </summary>
    public interface ICurrentUserService 
    {
        User CurrentUser { get; }
		IHttpContextAccessor HttpContextAccessor { get; }
	}
}
