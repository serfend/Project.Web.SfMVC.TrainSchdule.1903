using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TrainSchdule.DAL.Interfaces;
using TrainSchdule.DAL.Entities.UserInfo;
using TrainSchdule.BLL.Interfaces;

namespace TrainSchdule.BLL.Services
{
    /// <summary>
    /// Contains methods with likes processing logic.
    /// Realization of <see cref="ILikesService"/>.
    /// </summary>
    public class LikesService : ILikesService
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICurrentUserService _currentUserService;

        private bool _isDisposed;

        #endregion

        #region .ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="LikesService"/>.
        /// </summary>
        public LikesService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _currentUserService = new CurrentUserService(unitOfWork, httpContextAccessor);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Adds like by liked photo id.
        /// </summary>
        public void Add(Guid photoId)
        {
            var photo = _unitOfWork.Photos.Get(photoId);
            var user = _currentUserService.CurrentUser;
            var like = _unitOfWork.Likes.Find(l => l.Owner.Id == user.Id && l.Photo.Id == photo.Id).FirstOrDefault();

            if (photo != null && user != null && like == null)
            {
                _unitOfWork.Likes.Create(
                    new Like
                    {
                        Photo=new Photo(){Id = photo.Id},
                        Date = DateTime.Now,
                        Owner=new User(){Id = user.Id},
                    }
                );

                _unitOfWork.Save();
            }
        }

        /// <summary>
        /// Async adds like by liked photo id.
        /// </summary>
        public async Task AddAsync(Guid photoId)
        {
            var photo = await _unitOfWork.Photos.GetAsync(photoId);
            var user = _currentUserService.CurrentUser;
            var like = _unitOfWork.Likes.Find(l => l.Owner.Id == user.Id && l.Photo.Id == photo.Id).FirstOrDefault();

            if (photo != null && user != null && like == null)
            {
                await _unitOfWork.Likes.CreateAsync(
                    new Like
                    {
                        Photo=new Photo(){Id = photo.Id},
                        Date = DateTime.Now,
                        Owner=new User(){Id = user.Id},
                    }
                );

                await _unitOfWork.SaveAsync();
            }
        }

        /// <summary>
        /// Deletes like by photo id.
        /// </summary>
        public void Delete(Guid photoId)
        {
            var photo = _unitOfWork.Photos.Get(photoId);
            var user = _currentUserService.CurrentUser;
            var like = _unitOfWork.Likes.Find(l => l.Owner.Id == user.Id && l.Photo.Id == photo.Id).FirstOrDefault();

            if (photo != null && user != null && like != null)
            {
                _unitOfWork.Likes.Delete(like.Id);
                _unitOfWork.Save();
            }
        }

        /// <summary>
        /// Async deletes like by photo id.
        /// </summary>
        public async Task DeleteAsync(Guid photoId)
        {
            var photo = await _unitOfWork.Photos.GetAsync(photoId);
            var user = _currentUserService.CurrentUser;
            var like = _unitOfWork.Likes.Find(l => l.Owner.Id == user.Id && l.Photo.Id == photo.Id).FirstOrDefault();

            if (photo != null && user != null && like != null)
            {
                await _unitOfWork.Likes.DeleteAsync(like.Id);
                await _unitOfWork.SaveAsync();
            }
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
                    _currentUserService.Dispose();
                }

                _isDisposed = true;
            }
        }

        ~LikesService()
        {
            Dispose(false);
        }

        #endregion
    }
}