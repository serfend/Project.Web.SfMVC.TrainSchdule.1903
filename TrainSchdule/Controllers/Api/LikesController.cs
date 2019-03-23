﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TrainSchdule.BLL.Interfaces;

namespace TrainSchdule.WEB.Controllers.Api
{
    [Route("api/likes")]
    public class LikesController : Controller
    {
        #region Fields

        private readonly ILikesService _likesService;

        private bool _isDisposed;

        #endregion

        #region .ctors

        public LikesController(ILikesService likesService)
        {
            _likesService = likesService;
        }

        #endregion

        #region Logic

        [Authorize, HttpPost, Route("add/{photoId}")]
        public async Task Add(Guid photoId)
        {
            await _likesService.AddAsync(photoId);
        }

        [Authorize, HttpPost, Route("delete/{photoId}")]
        public async Task Delete(Guid photoId)
        {
            await _likesService.DeleteAsync(photoId);
        }

        #endregion

        #region Disposing

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _likesService.Dispose();
                }

                _isDisposed = true;

                base.Dispose(disposing);
            }
        }

        #endregion
    }
}