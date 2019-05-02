﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace TrainSchdule.DAL.Entities.UserInfo
{
    /// <summary>
    /// Photo entity.
    /// Contains photo properties, iso data, <see cref="OwnerId"/>, <see cref="FilterId"/>.
    /// </summary>
    public class Photo : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets and sets photo path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets and sets photo description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets and sets photo date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets and sets count of photo views.
        /// </summary>
        public int CountViews { get; set; }

        /// <summary>
        /// Gets and sets photo camera manufacturer.
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets and sets photo camera model.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Gets and sets photo iso.
        /// </summary>
        public int? Iso { get; set; }

        /// <summary>
        /// Gets and sets photo exposure in sec.
        /// </summary>
        public double? Exposure { get; set; }

        /// <summary>
        /// Gets and sets photo aperture in f/.
        /// </summary>
        public double? Aperture { get; set; }

        /// <summary>
        /// Gets and sets photo focal length in mm.
        /// </summary>
        public double? FocalLength { get; set; }

        /// <summary>
        /// Gets and sets user entity by foreign key.
        /// </summary>
        public virtual User Owner { get; set; }

        /// <summary>
        /// Gets and sets filter entity by foreign key.
        /// </summary>
        public virtual Filter Filter { get; set; }

        /// <summary>
        /// Gets and sets collection with likes.
        /// </summary>
        public virtual ICollection<Like> Likes { get; set; }

        /// <summary>
        /// Gets and sets collection with comments.
        /// </summary>
        public virtual ICollection<Comment> Comments { get; set; }

        #endregion

        #region .ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="Photo"/>, sets default value to properties.
        /// </summary>
        public Photo()
        {
            Date = DateTime.Now;
            CountViews = 0;
        }

        #endregion
    }
}