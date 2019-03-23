﻿using System;

namespace TrainSchdule.DAL.Entities
{
    /// <summary>
    /// Following entity.
    /// Contains <see cref="UserId"/> and <see cref="FollowedUserId"/>.
    /// </summary>
    public class Following : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets and sets foreign key to user by id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets and sets user entity by foreign key.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Gets and sets foreign key to followed user by id.
        /// </summary>
        public Guid FollowedUserId { get; set; }

        /// <summary>
        /// Gets and sets user entity by foreign key.
        /// </summary>
        public virtual User FollowedUser { get; set; }

        #endregion
    }
}