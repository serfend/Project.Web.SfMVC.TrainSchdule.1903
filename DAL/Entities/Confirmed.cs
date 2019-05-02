using System;

namespace TrainSchdule.DAL.Entities.UserInfo
{
    /// <summary>
    /// Confirmed entity.
    /// Contains <see cref="UserId"/> and <see cref="AdminId"/>.
    /// </summary>
    public class Confirmed : BaseEntity
    {
        #region Properties

        /// <summary>
        /// Gets and sets user by <see cref="UserId"/> foreign key.
        /// </summary>
        public virtual User User { get; set; }


        /// <summary>
        /// Gets and sets admin by <see cref="AdminId"/> foreign key.
        /// </summary>
        public virtual User Admin { get; set; }

        #endregion
    }
}