using DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DAL.Entities.UserInfo;
using DAL.Entities.UserInfo.Permission;
using Apply = DAL.Entities.Apply;
using Company = DAL.Entities.UserInfo.Company;
using User = DAL.Entities.UserInfo.User;

namespace DAL.Data
{
    /// <summary>
    /// Main DB context in the application.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        #region Properties

        public DbSet<User> AppUsers { get; set; }

		public DbSet<Company> Companies { get; set; }
		public DbSet<Permissions>Permissions { get; set; }
		public DbSet<Duties> Duties { get; set; }
		public DbSet<Apply> Applies { get; set; }
		
		public DbSet<ApplyResponse> ApplyResponses { get; set; }
		public DbSet<ApplyRequest> ApplyRequests { get; set; }
		public DbSet<ApplyStamp> ApplyStamps { get; set; }

        #endregion

        #region .ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/>.
        /// </summary>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        #endregion

        #region Logic
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        #endregion
    }
}
