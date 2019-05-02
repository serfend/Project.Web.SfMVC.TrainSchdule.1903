using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrainSchdule.DAL.Interfaces;
using TrainSchdule.DAL.Data;
using TrainSchdule.DAL.Entities.UserInfo;

namespace TrainSchdule.DAL.Repositories
{
    /// <summary>
    /// Contains methods for processing DB entities in Users table.
    /// Implementation of <see cref="IRepository"/>.
    /// </summary>
    public class UsersRepository : IRepository<User>
    {
        #region Fields

        private readonly ApplicationDbContext _context;

        #endregion

        #region .ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersRepository"/>.
        /// </summary>
        public UsersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Logic

        /// <summary>
        /// Method for fetching all data from <see cref="UsersRepository"/>.
        /// </summary>
        public IEnumerable<User> GetAll()
        {
            return _context.AppUsers;
        }

        /// <summary>
        /// Method for fetching all data from <see cref="UsersRepository"/> with paggination.
        /// </summary>
        public IEnumerable<User> GetAll(int page, int pageSize)
        {
            return _context.AppUsers.Skip(page * pageSize).Take(pageSize);
        }

        /// <summary>
        /// Method for fetching <see cref="User"/> by id (primary key).
        /// </summary>
        public User Get(Guid id)
        {
            return _context.AppUsers.Find(id);
        }

        /// <summary>
        /// Async method for fetching <see cref="User"/> by id (primary key).
        /// </summary>
        public async Task<User> GetAsync(Guid id)
        {
            return await _context.AppUsers.FindAsync(id);
        }

        /// <summary>
        /// Method for fetching <see cref="User"/>(s) by predicate.
        /// </summary>
        public IQueryable<User> Find(Expression<Func<User, bool>> predicate)
        {
            return _context.AppUsers.Where(predicate);
        }

        /// <summary>
        /// Method for creating <see cref="User"/>.
        /// </summary>
        public void Create(User item)
        {
            _context.AppUsers.Add(item);
        }

        /// <summary>
        /// Async method for creating <see cref="User"/>.
        /// </summary>
        public async Task CreateAsync(User item)
        {
            await _context.AppUsers.AddAsync(item);
        }

        /// <summary>
        /// Method for updating <see cref="User"/>.
        /// </summary>
        public void Update(User item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        /// <summary>
        /// Method for deleting <see cref="User"/>.
        /// </summary>
        public void Delete(Guid id)
        {
            var item = _context.AppUsers.Find(id);

            if (item != null)
            {
                _context.AppUsers.Remove(item);
            }
        }

        /// <summary>
        /// Async method for deleting <see cref="User"/>.
        /// </summary>
        public async Task DeleteAsync(Guid id)
        {
            var item = await _context.AppUsers.FindAsync(id);

            if (item != null)
            {
                _context.AppUsers.Remove(item);
            }
        }

        #endregion
    }
}