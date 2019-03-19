using System;
using TrainSchdule.BLL.DTO;
using TrainSchdule.DAL.Entities;

namespace TrainSchdule.BLL.Interfaces
{
    /// <summary>
    /// Interface for getting current user services.
    /// </summary>
    public interface ICurrentUserService : IDisposable
    {
        /// <summary>
        /// Gets current user entity.
        /// </summary>
        User CurrentUser { get; }

        /// <summary>
        /// Gets current user data transfer object.
        /// </summary>
        UserDTO CurrentUserDTO { get; }
    }
}
