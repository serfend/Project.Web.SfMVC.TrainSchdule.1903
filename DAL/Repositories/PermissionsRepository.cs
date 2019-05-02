using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrainSchdule.DAL.Data;
using TrainSchdule.DAL.Entities;
using TrainSchdule.DAL.Entities.Permission;
using TrainSchdule.DAL.Interfaces;

namespace TrainSchdule.DAL.Repositories
{
	/// <summary>
	/// Contains methods for processing DB entities in Permissions table.
	/// Implementation of <see cref="IRepository{T}"/>.
	/// </summary>
	public class PermissionsRepository : IRepository<Permissions>
    {
        #region Fields

        private readonly ApplicationDbContext _context;

        #endregion

        #region .ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionsRepository"/>.
        /// </summary>
        public PermissionsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Logic

        /// <summary>
        /// Method for fetching all data from <see cref="PermissionsRepository"/>.
        /// </summary>
        public IEnumerable<Permissions> GetAll()
        {
            return _context.Permissions;
        }

        /// <summary>
        /// Method for fetching all data from <see cref="PermissionsRepository"/> with paggination.
        /// </summary>
        public IEnumerable<Permissions> GetAll(int page, int pageSize)
        {
            return _context.Permissions.Skip(page * pageSize).Take(pageSize);
        }

        /// <summary>
        /// Method for fetching <see cref="Permissions"/>.by id (primary key).
        /// </summary>
        public Permissions Get(Guid id)
        {
            return _context.Permissions.Find(id);
        }

        /// <summary>
        /// Async method for fetching <see cref="Permissions"/>.by id (primary key).
        /// </summary>
        public async Task<Permissions> GetAsync(Guid id)
        {
            return await _context.Permissions.FindAsync(id);
        }

        /// <summary>
        /// Method for fetching <see cref="Permissions"/>.s) by predicate.
        /// </summary>
        public IQueryable<Permissions> Find(Expression<Func<Permissions, bool>> predicate)
        {
            return _context.Permissions.Where(predicate);
        }

        /// <summary>
        /// Method for creating <see cref="Permissions"/>.
        /// </summary>
        public void Create(Permissions item)
        {
            _context.Permissions.Add(item);
        }

        /// <summary>
        /// Async method for creating <see cref="Permissions"/>.
        /// </summary>
        public async Task CreateAsync(Permissions item)
        {
            await _context.Permissions.AddAsync(item);
        }

        /// <summary>
        /// Method for updating <see cref="Permissions"/>.
        /// </summary>
        public void Update(Permissions item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        /// <summary>
        /// Method for deleting <see cref="Permissions"/>.
        /// </summary>
        public void Delete(Guid id)
        {
            var item = _context.Permissions.Find(id);

            if (item != null)
            {
                _context.Permissions.Remove(item);
            }
        }

        /// <summary>
        /// Async method for deleting <see cref="Permissions"/>.
        /// </summary>
        public async Task DeleteAsync(Guid id)
        {
            var item = await _context.Permissions.FindAsync(id);

            if (item != null)
            {
                _context.Permissions.Remove(item);
            }
        }

        #endregion
    }
}
