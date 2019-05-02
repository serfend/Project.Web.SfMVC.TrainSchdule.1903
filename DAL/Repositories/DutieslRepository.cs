using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using TrainSchdule.DAL.Data;
using TrainSchdule.DAL.Entities.UserInfo;
using TrainSchdule.DAL.Interfaces;
using TrainSchdule.DAL.Repositories;

namespace DAL.Repositories
{
	public  class DutiesRepository:IRepository<Duties>
	{
		#region Fields

		private readonly ApplicationDbContext _context;

		#endregion

		#region .ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="DutiesRepository"/>.
		/// </summary>
		public DutiesRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		#endregion

		#region Logic

		/// <summary>
		/// Method for fetching all data from <see cref="DutiesRepository"/>.
		/// </summary>
		public IEnumerable<Duties> GetAll()
		{
			return _context.Duties.OrderBy(l => l.Name);
		}

		/// <summary>
		/// Method for fetching all data from <see cref="DutiesRepository"/> with paggination.
		/// </summary>
		public IEnumerable<Duties> GetAll(int page, int pageSize)
		{
			return _context.Duties.OrderBy(l => l.Name).Skip(page * pageSize).Take(pageSize);
		}

		/// <summary>
		/// Method for fetching <see cref="Duties"/> by id (primary key).
		/// </summary>
		public Duties Get(Guid id)
		{
			return _context.Duties.Where(l => l.Id == id).FirstOrDefault();
		}

		/// <summary>
		/// Async method for fetching <see cref="Duties"/> by id (primary key).
		/// </summary>
		public async Task<Duties> GetAsync(Guid id)
		{
			return await _context.Duties.Where(l => l.Id == id).FirstOrDefaultAsync();
		}

		/// <summary>
		/// Method for fetching <see cref="Duties"/>(s) by predicate.
		/// </summary>
		public IQueryable<Duties> Find(Expression<Func<Duties, bool>> predicate)
		{
			return _context.Duties.Where(predicate);
		}

		/// <summary>
		/// Method for creating <see cref="Duties"/>.
		/// </summary>
		public void Create(Duties item)
		{
			_context.Duties.Add(item);
		}

		/// <summary>
		/// Async method for creating <see cref="Duties"/>.
		/// </summary>
		public async Task CreateAsync(Duties item)
		{
			await _context.Duties.AddAsync(item);
		}

		/// <summary>
		/// Method for updating <see cref="Duties"/>.
		/// </summary>
		public void Update(Duties item)
		{
			_context.Entry(item).State = EntityState.Modified;
		}

		/// <summary>
		/// Method for deleting <see cref="Duties"/>.
		/// </summary>
		public void Delete(Guid id)
		{
			var item = _context.Duties.Find(id);

			if (item != null)
			{
				_context.Duties.Remove(item);
			}
		}

		/// <summary>
		/// Async method for deleting <see cref="Duties"/>.
		/// </summary>
		public async Task DeleteAsync(Guid id)
		{
			var item = await _context.Duties.FindAsync(id);

			if (item != null)
			{
				_context.Duties.Remove(item);
			}
		}

		#endregion
	}
}
