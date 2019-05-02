﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using TrainSchdule.DAL.Interfaces;
using TrainSchdule.DAL.Entities.UserInfo;
using TrainSchdule.BLL.Interfaces;
using TrainSchdule.BLL.DTO.UserInfo;
using TrainSchdule.BLL.Extensions;

namespace TrainSchdule.BLL.Services
{
    /// <summary>
    /// Contains properties that returns current user.
    /// Realization of <see cref="ICurrentUserService"/>.
    /// </summary>
    public class CurrentUserService : ICurrentUserService
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private bool _isDisposed;

        #endregion

        #region Properties

        /// <returns>
        /// Returns current user entity.
        /// </returns>
        public User CurrentUser => 
	        _unitOfWork.Users
		        .Find(u => u.UserName == _httpContextAccessor.HttpContext.User.Identity.Name)
		        .FirstOrDefault();

        /// <returns>
        /// Returns current user data transfer object.
        /// </returns>
        public UserDTO CurrentUserDTO
        {
            get
            {
                var user = CurrentUser;

                return user.ToDTO();
            }
        }

        #endregion

        #region .ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentUserService"/>.
        /// </summary>
        public CurrentUserService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Disposing

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

        ~CurrentUserService()
        {
            Dispose(false);
        }

        #endregion
    }
}