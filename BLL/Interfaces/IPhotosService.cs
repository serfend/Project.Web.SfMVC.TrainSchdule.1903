﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrainSchdule.BLL.DTO;

namespace TrainSchdule.BLL.Interfaces
{
    /// <summary>
    /// Interface for photos services.
    /// Contains methods with photos processing logic and some other methods.
    /// </summary>
    /// <remarks>
    /// This interface needs refactoring.
    /// </remarks>
    public interface IPhotosService : IDisposable
    {
        #region Properties

        /// <summary>
        /// Returns filter DTOs for the photo.
        /// </summary>
        List<FilterDTO> Filters { get; }

        /// <summary>
        /// Returns tag DTOs for the photo.
        /// </summary>
        List<TagDTO> Tags { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Loads all photos with paggination, returns collection of photo DTOs.
        /// </summary>
        IEnumerable<PhotoDTO> GetAll(int page, int pageSize);

        /// <summary>
        /// Loads photo, returns photo DTO.
        /// </summary>
        PhotoDTO Get(Guid id);

        /// <summary>
        /// Async loads photo, returns photo DTO.
        /// </summary>
        Task<PhotoDTO> GetAsync(Guid id);

        /// <summary>
        /// Loads photos for home page with paggination, returns collection of photo DTOs.
        /// </summary>
        IEnumerable<PhotoDTO> GetPhotosHome(int page, int pageSize);

        /// <summary>
        /// Loads photos for user page with paggination, returns collection of photo DTOs.
        /// </summary>
        IEnumerable<PhotoDTO> GetForUser(int page, string userName, int pageSize);

        /// <summary>
        /// Loads photos for tag page with paggination, returns collection of photo DTOs.
        /// </summary>
        IEnumerable<PhotoDTO> GetForTag(string tagName, int pageSize);

        /// <summary>
        /// Loads photos for bookmarks page with paggination, returns collection of photo DTOs.
        /// </summary>
        IEnumerable<PhotoDTO> GetBookmarks(int page, int pageSize);

        /// <summary>
        /// Loads photos for tag page with paggination, returns collection of photo DTOs.
        /// </summary>
        IEnumerable<PhotoDTO> GetTags(Guid tagId, int page, int pageSize);

        /// <summary>
        /// Loads photos by search parameters with paggination, returns collection of photo DTOs.
        /// </summary>
        IEnumerable<PhotoDTO> Search(int page, string search, int pageSize, int? iso, double? exposure, double? aperture, double? focalLength);

        /// <summary>
        /// Bookmarks photo by photo id.
        /// </summary>
        void Bookmark(Guid id);

        /// <summary>
        /// Async bookmarks photo by photo id.
        /// </summary>
        Task BookmarkAsync(Guid id);

        /// <summary>
        /// Dismiss photo bookmark by photo id.
        /// </summary>
        void DismissBookmark(Guid id);

        /// <summary>
        /// Async dismiss photo bookmark by photo id.
        /// </summary>
        Task DismissBookmarkAsync(Guid id);

        /// <summary>
        /// Reports photo by photo id and report text.
        /// </summary>
        void Report(Guid id, string text);

        /// <summary>
        /// Async reports photo by photo id and report text.
        /// </summary>
        Task ReportAsync(Guid id, string text);

        /// <summary>
        /// Creates photo by photo properties.
        /// </summary>
        Guid Create(string filter, string description, string path, string manufacturer, string model, int? iso, double? exposure, double? aperture, double? focalLength, string tags);

        /// <summary>
        /// Async creates photo by photo properties.
        /// </summary>
        ValueTask<Guid> CreateAsync(string filter, string description, string path, string manufacturer, string model, int? iso, double? exposure, double? aperture, double? focalLength, string tags);

        /// <summary>
        /// Edits photo by photo properties.
        /// </summary>
        void Edit(Guid id, string filter, string description, string tags, string model, string brand, int? iso, double? aperture, double? exposure, double? focalLength);

        /// <summary>
        /// Async edits photo by photo properties.
        /// </summary>
        Task EditAsync(Guid id, string filter, string description, string tags, string model, string brand, int? iso, double? aperture, double? exposure, double? focalLength);

        /// <summary>
        /// Deletes photo by photo id.
        /// </summary>
        void Delete(Guid id);

        /// <summary>
        /// Async deletes photo by photo id.
        /// </summary>
        Task DeleteAsync(Guid id);

        #endregion
    }
}
