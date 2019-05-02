using System;
using System.Threading.Tasks;
using DAL.Entities;
using TrainSchdule.DAL.Data;
using TrainSchdule.DAL.Entities;
using TrainSchdule.DAL.Entities.Permission;
using Apply = DAL.Entities.Apply;
using Company = TrainSchdule.DAL.Entities.Company;
using User = TrainSchdule.DAL.Entities.User;

namespace TrainSchdule.DAL.Interfaces
{
    /// <summary>
    /// Interface for accessing DB by repositories.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        #region Properties
		ApplicationDbContext Context { get; }
        /// <summary>
        /// Gets photo repository.
        /// </summary>
        IRepository<Photo> Photos { get; }

        /// <summary>
        /// Gets user identity repository.
        /// </summary>
        IRepository<ApplicationUser> IdentityUsers { get; }

        /// <summary>
        /// Gets user repository.
        /// </summary>
        IRepository<User> Users { get; }

        /// <summary>
        /// Gets comment repository.
        /// </summary>
        IRepository<Comment> Comments { get; }

        /// <summary>
        /// Gets like repository.
        /// </summary>
        IRepository<Like> Likes { get; }

        /// <summary>
        /// Gets confirmations repository.
        /// </summary>
        IRepository<Confirmed> Confirmations { get; }

        /// <summary>
        /// Gets following repository.
        /// </summary>
        IRepository<Following> Followings { get; }

        /// <summary>
        /// Gets blacklist repository.
        /// </summary>
        IRepository<BlackList> Blockings { get; }

        /// <summary>
        /// Gets bookmark repository.
        /// </summary>
        IRepository<Bookmark> Bookmarks { get; }

        /// <summary>
        /// Gets filter repository.
        /// </summary>
        IRepository<Filter> Filters { get; }

        /// <summary>
        /// Gets tag repository.
        /// </summary>
        IRepository<Tag> Tags { get; }

        /// <summary>
        /// Gets taging repository.
        /// </summary>
        IRepository<Taging> Tagings { get; }

        /// <summary>
        /// Gets photo report repository.
        /// </summary>
        IRepository<PhotoReport> PhotoReports { get; }

        /// <summary>
        /// Gets user report repository.
        /// </summary>
        IRepository<UserReport> UserReports { get; }

		IRepository<Student> Students { get; }
		IRepository<Company> Companies { get; }
		IRepository<Permissions>Permissions{ get; }
		IRepository<Duties> Duties { get; }
		IRepository<Apply> Applies { get; }

		IRepository<ApplyResponse> ApplyResponses { get; }
		IRepository<ApplyRequest> ApplyRequests { get; }
		IRepository<ApplyStamp> ApplyStamps { get; }
		#endregion

        #region Methods

        /// <summary>
        /// Method for saving db changes.
        /// </summary>
        void Save();

        /// <summary>
        /// Async method for saving db changes.
        /// </summary>
        Task SaveAsync();

        #endregion
    }
}
