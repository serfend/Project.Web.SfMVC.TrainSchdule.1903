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
    /// Contains methods for processing DB entities in Confirmations table.
    /// Implementation of <see cref="IRepository"/>.
    /// </summary>
    public class FiltersRepository : IRepository<Filter>
    {
        #region Fields

        private readonly ApplicationDbContext _context;

        #endregion

        #region .ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="FiltersRepository"/>.
        /// </summary>
        public FiltersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Logic

        /// <summary>
        /// Method for fetching all data from <see cref="FiltersRepository"/>.
        /// </summary>
        public IEnumerable<Filter> GetAll()
        {
            return _context.Filters;
        }

        /// <summary>
        /// Method for fetching all data from <see cref="FiltersRepository"/> with paggination.
        /// </summary>
        public IEnumerable<Filter> GetAll(int page, int pageSize)
        {
            return _context.Filters.Skip(page * pageSize).Take(pageSize);
        }

        /// <summary>
        /// Method for fetching <see cref="Filter"/>(s) by predicate.
        /// </summary>
        public IQueryable<Filter> Find(Expression<Func<Filter, bool>> predicate)
        {
            return _context.Filters.Where(predicate);
        }

        /// <summary>
        /// Method for fetching <see cref="Filter"/> by id (primary key).
        /// </summary>
        public Filter Get(Guid id)
        {
            return _context.Filters.Where(c => c.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Async method for fetching <see cref="Filter"/> by id (primary key).
        /// </summary>
        public async Task<Filter> GetAsync(Guid id)
        {
            return await _context.Filters.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Method for creating <see cref="Filter"/>.
        /// </summary>
        public void Create(Filter item)
        {
            _context.Filters.Add(item);
        }

        /// <summary>
        /// Async method for creating <see cref="Filter"/>.
        /// </summary>
        public async Task CreateAsync(Filter item)
        {
            await _context.Filters.AddAsync(item);
        }

        /// <summary>
        /// Method for updating <see cref="Filter"/>.
        /// </summary>
        public void Update(Filter item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        /// <summary>
        /// Method for deleting <see cref="Filter"/>.
        /// </summary>
        public void Delete(Guid id)
        {
            var item = _context.Filters.Find(id);

            if (item != null)
            {
                _context.Filters.Remove(item);
            }
        }

        /// <summary>
        /// Async method for deleting <see cref="Filter"/>.
        /// </summary>
        public async Task DeleteAsync(Guid id)
        {
            var item = await _context.Filters.FindAsync(id);

            if (item != null)
            {
                _context.Filters.Remove(item);
            }
        }

        #endregion
    }
}
