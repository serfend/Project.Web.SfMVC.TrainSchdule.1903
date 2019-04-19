using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrainSchdule.DAL.Data;
using TrainSchdule.DAL.Entities;
using TrainSchdule.DAL.Interfaces;

namespace TrainSchdule.DAL.Repositories
{
	/// <summary>
	/// Contains methods for processing DB entities in PermissionCompany table.
	/// Implementation of <see cref="IRepository{T}"/>.
	/// </summary>
	public class PermissionCompanyRepository : IRepository<PermissionCompany>
    {
        #region Fields

        private readonly ApplicationDbContext _context;

        #endregion

        #region .ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionCompanyRepository"/>.
        /// </summary>
        public PermissionCompanyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Logic

        /// <summary>
        /// Method for fetching all data from <see cref="PermissionCompanyRepository"/>.
        /// </summary>
        public IEnumerable<PermissionCompany> GetAll()
        {
            return _context.PermissionCompanies;
        }

        /// <summary>
        /// Method for fetching all data from <see cref="PermissionCompanyRepository"/> with paggination.
        /// </summary>
        public IEnumerable<PermissionCompany> GetAll(int page, int pageSize)
        {
            return _context.PermissionCompanies.Skip(page * pageSize).Take(pageSize);
        }

        /// <summary>
        /// Method for fetching <see cref="PermissionCompany"/>.by id (primary key).
        /// </summary>
        public PermissionCompany Get(Guid id)
        {
            return _context.PermissionCompanies.Find(id);
        }

        /// <summary>
        /// Async method for fetching <see cref="PermissionCompany"/>.by id (primary key).
        /// </summary>
        public async Task<PermissionCompany> GetAsync(Guid id)
        {
            return await _context.PermissionCompanies.FindAsync(id);
        }

        /// <summary>
        /// Method for fetching <see cref="PermissionCompany"/>.s) by predicate.
        /// </summary>
        public IQueryable<PermissionCompany> Find(Expression<Func<PermissionCompany, bool>> predicate)
        {
            return _context.PermissionCompanies.Where(predicate);
        }

        /// <summary>
        /// Method for creating <see cref="PermissionCompany"/>.
        /// </summary>
        public void Create(PermissionCompany item)
        {
            _context.PermissionCompanies.Add(item);
        }

        /// <summary>
        /// Async method for creating <see cref="PermissionCompany"/>.
        /// </summary>
        public async Task CreateAsync(PermissionCompany item)
        {
            await _context.PermissionCompanies.AddAsync(item);
        }

        /// <summary>
        /// Method for updating <see cref="PermissionCompany"/>.
        /// </summary>
        public void Update(PermissionCompany item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        /// <summary>
        /// Method for deleting <see cref="PermissionCompany"/>.
        /// </summary>
        public void Delete(Guid id)
        {
            var item = _context.PermissionCompanies.Find(id);

            if (item != null)
            {
                _context.PermissionCompanies.Remove(item);
            }
        }

        /// <summary>
        /// Async method for deleting <see cref="PermissionCompany"/>.
        /// </summary>
        public async Task DeleteAsync(Guid id)
        {
            var item = await _context.PermissionCompanies.FindAsync(id);

            if (item != null)
            {
                _context.PermissionCompanies.Remove(item);
            }
        }

        #endregion
    }
}
