using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TrainSchdule.DAL.Interfaces;
using TrainSchdule.DAL.Entities.UserInfo;
using TrainSchdule.BLL.Interfaces;
using TrainSchdule.BLL.DTO;
using TrainSchdule.BLL.Extensions;
using TrainSchdule.BLL.Helpers;

namespace TrainSchdule.BLL.Services
{
    /// <summary>
    /// Contains methods with photos processing logic.
    /// Realization of <see cref="IPhotosService"/>.
    /// </summary>
    public class PhotosService : IPhotosService
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICurrentUserService _currentUserService;

        private bool _isDisposed;

        #endregion

        #region Properties

        /// <summary>
        /// Returns filter DTOs for the photo.
        /// </summary>
        public List<FilterDTO> Filters => _unitOfWork.Filters.GetAll().ToDTOs();

        /// <summary>
        /// Returns tag DTOs for the photo.
        /// </summary>
        public List<TagDTO> Tags => _unitOfWork.Tags.GetAll().ToDTOs();

        #endregion

        #region .ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsService"/>.
        /// </summary>
        public PhotosService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _currentUserService = new CurrentUserService(unitOfWork, httpContextAccessor);
        }

        #endregion

        #region Logic

        /// <summary>
        /// Loads all photos with paggination, returns collection of photo DTOs.
        /// </summary>
        public IEnumerable<PhotoDTO> GetAll(int page, int pageSize)
        {
            var photos = _unitOfWork.Photos.GetAll().OrderByDescending(p => p.Likes.Count).Skip(page * pageSize).Take(pageSize);

            var photoDTOs = new List<PhotoDTO>(pageSize);

            foreach (var photo in photos)
            {
                photoDTOs.Add(MapPhoto(photo));
            }

            return photoDTOs;
        }

        /// <summary>
        /// Loads photo, returns photo DTO.
        /// </summary>
        public PhotoDTO Get(Guid id)
        {
            var photo = _unitOfWork.Photos.Get(id);

            photo.CountViews++;

            _unitOfWork.Photos.Update(photo);
            _unitOfWork.Save();

            return MapPhoto(photo);
        }

        /// <summary>
        /// Async loads photo, returns photo DTO.
        /// </summary>
        public async Task<PhotoDTO> GetAsync(Guid id)
        {
            var photo =  await _unitOfWork.Photos.GetAsync(id);

            photo.CountViews++;

            _unitOfWork.Photos.Update(photo);
            await _unitOfWork.SaveAsync();

            return MapPhoto(photo);
        }

        /// <summary>
        /// Loads photos for home page with paggination, returns collection of photo DTOs.
        /// </summary>
        public IEnumerable<PhotoDTO> GetPhotosHome(int page, int pageSize)
        {
            var currentUser = _currentUserService.CurrentUser;
            var followings = _unitOfWork.Followings.Find(f => f.User.Id == currentUser.Id);
            var photos = new List<Photo>();

            foreach (var follow in followings)
            {
                photos.AddRange(_unitOfWork.Photos.Find(p => p.Owner.Id == follow.FollowedUser.Id));
            }

            photos.Sort((p, p2) => p2.Date.CompareTo(p.Date));
            photos = photos.Skip(page * pageSize).Take(pageSize).ToList();

            var photoDTOs = new List<PhotoDTO>(pageSize);

            foreach (var photo in photos)
            {
                photoDTOs.Add(MapPhoto(photo));
            }

            return photoDTOs;
        }

        /// <summary>
        /// Loads photos for user page with paggination, returns collection of photo DTOs.
        /// </summary>
        public IEnumerable<PhotoDTO> GetForUser(int page, string userName, int pageSize)
        {
            var currentUser = _currentUserService.CurrentUser;
            var user = _unitOfWork.Users.Find(u => u.UserName == userName).FirstOrDefault();

            var photos = _unitOfWork.Photos.Find(p => p.Owner.Id == user.Id).OrderByDescending(p => p.Date).Skip(page * pageSize).Take(pageSize);

            var photoDTOs = new List<PhotoDTO>(pageSize);

            foreach (var photo in photos)
            {
                photoDTOs.Add(MapPhoto(photo));
            }

            return photoDTOs;
        }

        /// <summary>
        /// Loads photos for tag page with paggination, returns collection of photo DTOs.
        /// </summary>
        public IEnumerable<PhotoDTO> GetForTag(string tagName, int pageSize)
        {
            if (string.IsNullOrEmpty(tagName))
            {
                return null;
            }

            var currentUser = _currentUserService.CurrentUser;
            var tag = _unitOfWork.Tags.Find(t => t.Name.ToLower() == tagName.ToLower()).FirstOrDefault();

            if (tag == null)
            {
                return null;
            }

            var tagings = _unitOfWork.Tagings.Find(t => t.Tag.Id == tag.Id).OrderByDescending(t => t.Photo.Date);

            var photos = tagings.Select(t => t.Photo).Skip(0).Take(pageSize);

            var photoDTOs = new List<PhotoDTO>(pageSize);

            foreach (var photo in photos)
            {
                photoDTOs.Add(MapPhoto(photo));
            }

            return photoDTOs;
        }

        /// <summary>
        /// Loads photos for bookmarks page with paggination, returns collection of photo DTOs.
        /// </summary>
        public IEnumerable<PhotoDTO> GetBookmarks(int page, int pageSize)
        {
            var currentUser = _currentUserService.CurrentUser;
            var photos = _unitOfWork.Bookmarks.Find(b => b.User.Id == currentUser.Id).OrderByDescending(b => b.Date).Select(b => b.Photo).Skip(page * pageSize).Take(pageSize);

            var photoDTOs = new List<PhotoDTO>(pageSize);

            foreach (var photo in photos)
            {
                photoDTOs.Add(MapPhoto(photo));
            }

            return photoDTOs;
        }

        /// <summary>
        /// Loads photos for tag page with paggination, returns collection of photo DTOs.
        /// </summary>
        public IEnumerable<PhotoDTO> GetTags(Guid tagId, int page, int pageSize)
        {
            var currentUser = _currentUserService.CurrentUser;

            if (_unitOfWork.Tags.Find(t => t.Id == tagId).FirstOrDefault() == null)
            {
                return null;
            }

            var photos = _unitOfWork.Tagings.Find(t => t.Tag.Id == tagId).Select(t => t.Photo).OrderByDescending(p => p.Date).Skip(page * pageSize).Take(pageSize);

            var photoDTOs = new List<PhotoDTO>(pageSize);

            foreach (var photo in photos)
            {
                photoDTOs.Add(MapPhoto(photo));
            }

            return photoDTOs;
        }

        /// <summary>
        /// Loads photos by search parameters with paggination, returns collection of photo DTOs.
        /// </summary>
        public IEnumerable<PhotoDTO> Search(int page, string search, int pageSize, int? iso, double? exposure, double? aperture, double? focalLength)
        {
            var currentUser = _currentUserService.CurrentUser;
            IEnumerable<Photo> photos;

            if (string.IsNullOrEmpty(search))
            {
                photos = _unitOfWork.Photos.GetAll().OrderByDescending(p => p.Likes.Count).Skip(page * pageSize).Take(pageSize);
            }
            else
            {
                photos = _unitOfWork.Photos.Find(p =>
                    (!string.IsNullOrEmpty(p.Model) ? p.Model.ToLower().Contains(search.ToLower()) : false) ||
                    (!string.IsNullOrEmpty(p.Manufacturer) ? p.Manufacturer.ToLower().Contains(search.ToLower()) : false) ||
                    ((!string.IsNullOrEmpty(p.Manufacturer) && !string.IsNullOrEmpty(p.Model)) ? $"{p.Manufacturer.ToLower()} {p.Model.ToLower()}".Contains(search.ToLower()) : false)
                ).OrderByDescending(p => p.Likes.Count).Skip(page * pageSize).Take(pageSize);
            }

            if(iso != null)
            {
                photos = photos.Where(p => p.Iso == iso);
            }

            if (exposure != null)
            {
                photos = photos.Where(p => p.Exposure == exposure);
            }

            if (aperture != null)
            {
                photos = photos.Where(p => p.Aperture == aperture);
            }

            if (focalLength != null)
            {
                photos = photos.Where(p => p.FocalLength == focalLength);
            }

            var photoDTOs = new List<PhotoDTO>(pageSize);

            foreach (var photo in photos)
            {
                photoDTOs.Add(MapPhoto(photo));
            }

            return photoDTOs;
        }

        /// <summary>
        /// Bookmarks photo by photo id.
        /// </summary>
        public void Bookmark(Guid id)
        {
            var currentUser = _currentUserService.CurrentUser;
            var bookmarkedPhoto = _unitOfWork.Photos.Find(p => p.Id == id).FirstOrDefault();

            if (currentUser != null && bookmarkedPhoto != null && _unitOfWork.Bookmarks.Find(b => b.User.Id == currentUser.Id && b.Photo.Id == id).FirstOrDefault() == null)
            {
                _unitOfWork.Bookmarks.Create(new Bookmark
                {
                    User =new User(){Id= currentUser.Id},
                     Photo=new Photo(){Id =  bookmarkedPhoto.Id},
                });

                _unitOfWork.Save();
            }
        }

        /// <summary>
        /// Async bookmarks photo by photo id.
        /// </summary>
        public async Task BookmarkAsync(Guid id)
        {
            var currentUser = _currentUserService.CurrentUser;
            var bookmarkedPhoto = _unitOfWork.Photos.Find(p => p.Id == id).FirstOrDefault();

            if (currentUser != null && bookmarkedPhoto != null && _unitOfWork.Bookmarks.Find(b => b.User.Id == currentUser.Id && b.Photo.Id == id).FirstOrDefault() == null)
            {
                _unitOfWork.Bookmarks.Create(new Bookmark
                {
                    User =new User(){Id= currentUser.Id},
                     Photo=new Photo(){Id =  bookmarkedPhoto.Id},
                });

                await _unitOfWork.SaveAsync();
            }
        }

        /// <summary>
        /// Dismiss photo bookmark by photo id.
        /// </summary>
        public void DismissBookmark(Guid id)
        {
            var currentUser = _currentUserService.CurrentUser;
            var bookmarkedPhoto = _unitOfWork.Photos.Find(p => p.Id == id).FirstOrDefault();
            var bookmark = _unitOfWork.Bookmarks.Find(b => b.User.Id == currentUser.Id && b.Photo.Id == id).FirstOrDefault();

            if (currentUser != null && bookmarkedPhoto != null && bookmark != null)
            {
                _unitOfWork.Bookmarks.Delete(bookmark.Id);
                _unitOfWork.Save();
            }
        }

        /// <summary>
        /// Async dismiss photo bookmark by photo id.
        /// </summary>
        public async Task DismissBookmarkAsync(Guid id)
        {
            var currentUser = _currentUserService.CurrentUser;
            var bookmarkedPhoto = _unitOfWork.Photos.Find(p => p.Id == id).FirstOrDefault();
            var bookmark = _unitOfWork.Bookmarks.Find(b => b.User.Id == currentUser.Id && b.Photo.Id == id).FirstOrDefault();

            if (currentUser != null && bookmarkedPhoto != null && bookmark != null)
            {
                await _unitOfWork.Bookmarks.DeleteAsync(bookmark.Id);
                await _unitOfWork.SaveAsync();
            }
        }

        /// <summary>
        /// Reports photo by photo id and report text.
        /// </summary>
        public void Report(Guid id, string text)
        {
            var currentUser = _currentUserService.CurrentUser;
            var reportedPhoto = _unitOfWork.Photos.Find(p => p.Id == id).FirstOrDefault();

            if (currentUser != null && reportedPhoto != null && _unitOfWork.PhotoReports.Find(pr => pr.User.Id == currentUser.Id && pr.Photo.Id == id).FirstOrDefault() == null)
            {
                _unitOfWork.PhotoReports.Create(new PhotoReport
                {
                    User =new User(){Id= currentUser.Id},
                    Photo=new Photo(){Id = reportedPhoto.Id},
                    Text = text
                });

                _unitOfWork.Save();
            }
        }

        /// <summary>
        /// Async reports photo by photo id and report text.
        /// </summary>
        public async Task ReportAsync(Guid id, string text)
        {
            var currentUser = _currentUserService.CurrentUser;
            var reportedPhoto = _unitOfWork.Photos.Find(p => p.Id == id).FirstOrDefault();

            if (currentUser != null && reportedPhoto != null && _unitOfWork.PhotoReports.Find(pr => pr.User.Id == currentUser.Id && pr.Photo.Id == id).FirstOrDefault() == null)
            {
                _unitOfWork.PhotoReports.Create(new PhotoReport
                {
                    User =new User(){Id= currentUser.Id},
                    Photo=new Photo(){Id = reportedPhoto.Id},
                    Text = text
                });

                await _unitOfWork.SaveAsync();
            }
        }

        /// <summary>
        /// Creates photo by photo properties.
        /// </summary>
        public Guid Create(string filter, string description, string path, string manufacturer, string model, int? iso, double? exposure, double? aperture, double? focalLength, string tags)
        {
            var photo = new Photo
            {
                Filter = new Filter(){Id = _unitOfWork.Filters.Find(f => f.Name == filter).FirstOrDefault()?.Id??Guid.Empty},
                Description = description,
                Path = path,
                Owner=new User(){Id = _currentUserService.CurrentUser.Id},
                CountViews = 0,

                Manufacturer = manufacturer,
                Model = model,
                Iso = iso,
                Exposure = exposure,
                Aperture = aperture,
                FocalLength = null
            };

            if (focalLength >= 3)
            {
                photo.FocalLength = focalLength;
            }

            _unitOfWork.Photos.Create(photo);

            if (!string.IsNullOrEmpty(tags))
            {
                foreach (var tag in tags.SplitTags(_unitOfWork.Tags.GetAll()))
                {
                    var tg = _unitOfWork.Tags.Find(t => t.Name == tag.Name).FirstOrDefault();

                    if (tg != null)
                    {
                        _unitOfWork.Tagings.Create(new Taging
                        {
                            Tag=new Tag(){Id = tg.Id},
                            Photo=new Photo{Id = photo.Id},
                        });
                    }
                }
            }

            _unitOfWork.Save();

            return photo.Id;
        }

        /// <summary>
        /// Async creates photo by photo properties.
        /// </summary>
        public async ValueTask<Guid> CreateAsync(string filter, string description, string path, string manufacturer, string model, int? iso, double? exposure, double? aperture, double? focalLength, string tags)
        {
            var photo = new Photo
            {
                Description = description,
                Path = path,
                Owner=new User(){Id = _currentUserService.CurrentUser.Id},
                CountViews = 0,

                Manufacturer = manufacturer,
                Model = model,
                Iso = iso,
                Exposure = exposure,
                Aperture = aperture,
                FocalLength = null
            };
            var tmp = _unitOfWork.Filters.Find(f => f.Name == filter).FirstOrDefault();
            if (tmp == null)
            {
				photo.Filter=new Filter()
				{
					Name = string.IsNullOrEmpty(filter)?"Default":filter
				};
	            await _unitOfWork.Filters.CreateAsync(photo.Filter);
            }
            else
            {
	            photo.Filter.Id = tmp.Id;
            }
            if (focalLength >= 3)
            {
                photo.FocalLength = focalLength;
            }

            await _unitOfWork.Photos.CreateAsync(photo);

            if (!string.IsNullOrEmpty(tags))
            {
                foreach (var tag in tags.SplitTags(_unitOfWork.Tags.GetAll()))
                {
                    var tg = _unitOfWork.Tags.Find(t => t.Name == tag.Name).FirstOrDefault();

                    if (tg != null)
                    {
                        await _unitOfWork.Tagings.CreateAsync(new Taging
                        {
                            Tag=new Tag(){Id = tg.Id},
                            Photo=new Photo{Id = photo.Id},
                        });
                    }
                }
            }

            await _unitOfWork.SaveAsync();

            return photo.Id;
        }

        /// <summary>
        /// Edits photo by photo properties.
        /// </summary>
        public void Edit(Guid id, string filter, string description, string tags, string model, string brand, int? iso, double? aperture, double? exposure, double? focalLength)
        {
            var photo = _unitOfWork.Photos.Get(id);

            if (photo != null)
            {
                var flt = _unitOfWork.Filters.Find(f => f.Name == filter).FirstOrDefault();

                if (flt != null)
                {
                    photo.Filter.Id = flt.Id;
                }

				//清空原Tag
                foreach (var taging in _unitOfWork.Tagings.Find(t => t.Photo.Id == photo.Id))
                {
                    _unitOfWork.Tagings.Delete(taging.Id);
                }

                if (!string.IsNullOrEmpty(tags))
                {
                    foreach (var tag in tags.SplitTags(_unitOfWork.Tags.GetAll()))
                    {
	                    _unitOfWork.Tagings.Create(new Taging
	                    {
	                        Tag=new Tag(){Id = tag.Id},
	                        Photo=new Photo{Id = photo.Id},
	                    });
                    }
                }

                if (!string.IsNullOrEmpty(description) && description != photo.Description)
                {
                    photo.Description = description;
                }

                if (string.IsNullOrEmpty(photo.Manufacturer) && !string.IsNullOrEmpty(brand))
                {
                    photo.Manufacturer = brand;
                }

                if (string.IsNullOrEmpty(photo.Model) && !string.IsNullOrEmpty(model))
                {
                    photo.Model = model;
                }

                if (photo.Iso == null && iso != null)
                {
                    photo.Iso = iso;
                }

                if (photo.Aperture == null && aperture != null)
                {
                    photo.Aperture = aperture;
                }

                if (photo.Exposure == null && exposure != null)
                {
                    photo.Exposure = exposure;
                }

                if (photo.FocalLength == null && focalLength != null)
                {
                    photo.FocalLength = focalLength;
                }

                _unitOfWork.Photos.Update(photo);
                _unitOfWork.Save();
            }
        }

        /// <summary>
        /// Async edits photo by photo properties.
        /// </summary>
        public async Task EditAsync(Guid id, string filter, string description, string tags, string model, string brand, int? iso, double? aperture, double? exposure, double? focalLength)
        {
            var photo = await _unitOfWork.Photos.GetAsync(id);

            if(photo != null)
            {
                var flt = _unitOfWork.Filters.Find(f => f.Name == filter).FirstOrDefault();

                if (flt != null)
                {
                    photo.Filter.Id = flt.Id;
                }

                foreach (var taging in _unitOfWork.Tagings.Find(t => t.Photo.Id == photo.Id))
                {
                    await _unitOfWork.Tagings.DeleteAsync(taging.Id);
                }

                if (!string.IsNullOrEmpty(tags))
                {
                    foreach (var tag in tags.SplitTags(_unitOfWork.Tags.GetAll()))
                    {
                        var tg = _unitOfWork.Tags.Find(t => t.Name == tag.Name).FirstOrDefault();

                        if (tg != null)
                        {
                            await _unitOfWork.Tagings.CreateAsync(new Taging
                            {
                                Tag=new Tag(){Id = tg.Id},
                                Photo=new Photo{Id = photo.Id},
                            });
                        }
                    }
                }

                if (!string.IsNullOrEmpty(description) && description != photo.Description)
                {
                    photo.Description = description;
                }

                if (string.IsNullOrEmpty(photo.Manufacturer) && !string.IsNullOrEmpty(brand))
                {
                    photo.Manufacturer = brand;
                }

                if (string.IsNullOrEmpty(photo.Model) && !string.IsNullOrEmpty(model))
                {
                    photo.Model = model;
                }

                if (photo.Iso == null && iso != null)
                {
                    photo.Iso = iso;
                }

                if (photo.Aperture == null && aperture != null)
                {
                    photo.Aperture = aperture;
                }

                if (photo.Exposure == null && exposure != null)
                {
                    photo.Exposure = exposure;
                }

                if (photo.FocalLength == null && focalLength != null)
                {
                    photo.FocalLength = focalLength;
                }

                _unitOfWork.Photos.Update(photo);
                await _unitOfWork.SaveAsync();
            }
        }

        /// <summary>
        /// Deletes photo by photo id.
        /// </summary>
        public void Delete(Guid id)
        {
            Photo photo = _unitOfWork.Photos.Get(id);

            if(photo != null)
            {
                _unitOfWork.Photos.Delete(photo.Id);
                _unitOfWork.Save();
            }
        }

        /// <summary>
        /// Async deletes photo by photo id.
        /// </summary>
        public async Task DeleteAsync(Guid id)
        {
            var photo = await _unitOfWork.Photos.GetAsync(id);

            if(photo != null)
            {
                await _unitOfWork.Photos.DeleteAsync(photo.Id);
                await _unitOfWork.SaveAsync();
            }
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Helps map photo data transfer object.
        /// </summary>
        protected PhotoDTO MapPhoto(Photo photo)
        {
	        return photo.ToDTO();
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

        ~PhotosService()
        {
            Dispose(false);
        }

        #endregion
    }
}