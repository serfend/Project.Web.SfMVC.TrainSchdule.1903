using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainSchdule.DAL.Entities
{
    /// <summary>
    /// Black list entity.
    /// Contains <see cref="UserId"/> and <see cref="BlockedUserId"/>.
    /// </summary>
    public class BlackList : BaseEntity
    {
        /// <summary>
        /// Gets and sets user entity by foreign key.
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

      
        /// <summary>
        /// Gets and sets user entity by BlockedUserId foreign key.
        /// </summary>
        [ForeignKey("BlockedUserId")]
        public virtual User BlockedUser { get; set; }
    }
}