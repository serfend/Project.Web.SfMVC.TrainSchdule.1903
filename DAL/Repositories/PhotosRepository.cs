using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrainSchdule.DAL.Interfaces;
using TrainSchdule.DAL.Data;
using TrainSchdule.DAL.Entities;

namespace TrainSchdule.DAL.Repositories
{
    /// <summary>
    /// Contains methods for processing DB entities in Photos table.
    /// Implementation of <see cref="IRepository"/>.
    /// </summary>
    public class PhotosRepository : IRepository<Photo>
    {
        #region Fields

        private readonly ApplicationDbContext _context;

        #endregion

        #region .ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="PhotosRepository"/>.
        /// </summary>
        public PhotosRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Logic

        /// <summary>
        /// Method for fetching all data from <see cref="PhotosRepository"/>.
        /// </summary>
        public IEnumerable<Photo> GetAll()
        {
            return _context.Photos.OrderByDescending(p => p.Date);
        }

        /// <summary>
        /// Method for fetching all data from <see cref="PhotosRepository"/> with paggination.
        /// </summary>
        public IEnumerable<Photo> GetAll(int page, int pageSize)
        {
            return _context.Photos.Skip(page * pageSize).Take(pageSize);
        }

        /// <summary>
        /// Method for fetching <see cref="Photo"/> by id (primary key).
        /// </summary>
        public Photo Get(Guid id)
        {
            return _context.Photos.FirstOrDefault(p => p.Id == id);
        }

        /// <summary>
        /// Async method for fetching <see cref="Photo"/> by id (primary key).
        /// </summary>
        public async Task<Photo> GetAsync(Guid id)
        {
            return await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <summary>
        /// Method for fetching <see cref="Photo"/>(s) by predicate.
        /// </summary>
        public IEnumerable<Photo> Find(Func<Photo, bool> predicate)
        {
            return _context.Photos.Where(predicate);
        }

        /// <summary>
        /// Method for creating <see cref="Photo"/>.
        /// </summary>
        public void Create(Photo item)
        {
            _context.Photos.Add(item);
        }

        /// <summary>
        /// Async method for creating <see cref="Photo"/>.
        /// </summary>
        public async Task CreateAsync(Photo item)
        {
            await _context.Photos.AddAsync(item);
        }

        /// <summary>
        /// Method for updating <see cref="Photo"/>.
        /// </summary>
        public void Update(Photo item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        /// <summary>
        /// Method for deleting <see cref="Photo"/>.
        /// </summary>
        public void Delete(Guid id)
        {
            var item = _context.Photos.Find(id);

            if (item != null)
            {
                _context.Photos.Remove(item);
            }
        }

        /// <summary>
        /// Async method for deleting <see cref="Photo"/>.
        /// </summary>
        public async Task DeleteAsync(Guid id)
        {
            var item = await _context.Photos.FindAsync(id);

            if (item != null)
            {
                _context.Photos.Remove(item);
            }
        }

        #endregion
    }
}