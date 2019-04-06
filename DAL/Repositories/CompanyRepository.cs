using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrainSchdule.DAL.Data;
using TrainSchdule.DAL.Entities;
using TrainSchdule.DAL.Interfaces;

namespace TrainSchdule.DAL.Repositories
{
	/// <summary>
	/// Contains methods for processing DB entities in Companies table.
	/// Implementation of <see cref="IRepository{T}"/>.
	/// </summary>
	public class CompanyRepository : IRepository<Company>
    {
        #region Fields

        private readonly ApplicationDbContext _context;

        #endregion

        #region .ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersRepository"/>.
        /// </summary>
        public CompanyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Logic

        /// <summary>
        /// Method for fetching all data from <see cref="UsersRepository"/>.
        /// </summary>
        public IEnumerable<Company> GetAll()
        {
            return _context.Companys;
        }

        /// <summary>
        /// Method for fetching all data from <see cref="UsersRepository"/> with paggination.
        /// </summary>
        public IEnumerable<Company> GetAll(int page, int pageSize)
        {
            return _context.Companys.Skip(page * pageSize).Take(pageSize);
        }

        /// <summary>
        /// Method for fetching <see cref="Company"/>.by id (primary key).
        /// </summary>
        public Company Get(Guid id)
        {
            return _context.Companys.Find(id);
        }

        /// <summary>
        /// Async method for fetching <see cref="Company"/>.by id (primary key).
        /// </summary>
        public async Task<Company> GetAsync(Guid id)
        {
            return await _context.Companys.FindAsync(id);
        }

        /// <summary>
        /// Method for fetching <see cref="Company"/>.s) by predicate.
        /// </summary>
        public IEnumerable<Company> Find(Func<Company, bool> predicate)
        {
            return _context.Companys.Where(predicate);
        }

        /// <summary>
        /// Method for creating <see cref="Company"/>.
        /// </summary>
        public void Create(Company item)
        {
            _context.Companys.Add(item);
        }

        /// <summary>
        /// Async method for creating <see cref="Company"/>.
        /// </summary>
        public async Task CreateAsync(Company item)
        {
            await _context.Companys.AddAsync(item);
        }

        /// <summary>
        /// Method for updating <see cref="Company"/>.
        /// </summary>
        public void Update(Company item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        /// <summary>
        /// Method for deleting <see cref="Company"/>.
        /// </summary>
        public void Delete(Guid id)
        {
            var item = _context.Companys.Find(id);

            if (item != null)
            {
                _context.Companys.Remove(item);
            }
        }

        /// <summary>
        /// Async method for deleting <see cref="Company"/>.
        /// </summary>
        public async Task DeleteAsync(Guid id)
        {
            var item = await _context.Companys.FindAsync(id);

            if (item != null)
            {
                _context.Companys.Remove(item);
            }
        }

        #endregion
    }
}
