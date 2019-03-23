using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks.Dataflow;

namespace TrainSchdule.DAL.Entities
{
    /// <summary>
    /// Black list entity.
    /// Contains <see cref="UserId"/> and <see cref="BlockedUserId"/>.
    /// </summary>
    public class BlackList : BaseEntity
    {
        /// <summary>
        /// Gets and sets foreign key to user by id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets and sets user entity by foreign key.
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        /// <summary>
        /// Gets and sets foreign key to blocked user by id.
        /// </summary>
        public Guid BlockedUserId { get; set; }

        /// <summary>
        /// Gets and sets user entity by BlockedUserId foreign key.
        /// </summary>
        [ForeignKey("BlockedUserId")]
        public virtual User BlockedUser { get; set; }
    }
}