﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TrainSchdule.BLL.Interfaces;

namespace TrainSchdule.WEB.Controllers.Api
{
    [Route("api/comments")]
    public class CommentsController : Controller
    {
        #region Fields

        private readonly ICommentsService _commentsService;

        private bool _isDisposed;

        #endregion

        #region .ctors

        public CommentsController(ICommentsService commentsService)
        {
            _commentsService = commentsService;
        }

        #endregion

        #region Logic

        [Authorize, HttpPost, Route("add")]
        public async Task<Guid?> Add(Guid photoId, string text)
        {
            return await _commentsService.AddAsync(photoId, text);
        }
        
        [Authorize, HttpPost, Route("delete/{id}")]
        public async Task Delete(Guid id)
        {
            await _commentsService.DeleteAsync(id);
        }

        #endregion

        #region Disposing

        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _commentsService.Dispose();
                }

                _isDisposed = true;

                base.Dispose(disposing);
            }
        }

        #endregion
    }
}