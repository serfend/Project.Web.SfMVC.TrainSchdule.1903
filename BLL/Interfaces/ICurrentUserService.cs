using System;
using DAL.Entities.UserInfo;

namespace BLL.Interfaces
{
    /// <summary>
    /// Interface for getting current user services.
    /// </summary>
    public interface ICurrentUserService : IDisposable
    {
        User CurrentUser { get; }

    }
}
