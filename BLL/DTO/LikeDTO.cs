﻿using System;

namespace TrainSchdule.BLL.DTO.UserInfo
{
    /// <summary>
    /// Like data transfer object.
    /// Contains <see cref="Id"/>, <see cref="Date"/> and <see cref="Owner"/> DTO.
    /// </summary>
    public class LikeDTO
    {
        /// <summary>
        /// Gets and sets like id.
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// Gets and sets like date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets and sets like owner DTO.
        /// </summary>
        public UserDTO Owner { get; set; }
    }
}