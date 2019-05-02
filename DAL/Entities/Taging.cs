using System;

namespace TrainSchdule.DAL.Entities.UserInfo
{
    /// <summary>
    /// Taging entity.
    /// Contains <see cref="PhotoId"/> and <see cref="TagId"/>.
    /// </summary>
    public class Taging : BaseEntity
    {
        /// <summary>
        /// Gets and sets photo entity by foreign key.
        /// </summary>
        public virtual Photo Photo { get; set; }


        /// <summary>
        /// Gets and sets tag entity by foreign key.
        /// </summary>
        public virtual Tag Tag { get; set; }

    }
}
