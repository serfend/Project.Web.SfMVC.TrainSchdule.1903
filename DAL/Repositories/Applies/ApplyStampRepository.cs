
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using TrainSchdule.DAL.Data;
using TrainSchdule.DAL.Interfaces;
using TrainSchdule.DAL.Repositories;

namespace DAL.Repositories.Applies
{
	/// <summary>
	/// Contains methods for processing DB entities in Companies table.
	/// Implementation of <see cref="IRepository{T}"/>.
	/// </summary>
	public class ApplyStampsRepository : IRepository<ApplyStamp>
	{
		#region Fields

		private readonly ApplicationDbContext _context;

		#endregion

		#region .ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="DAL.Repositories.ApplyStampsRepository"/>.
		/// </summary>
		public ApplyStampsRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		#endregion

		#region Logic

		/// <summary>
		/// Method for fetching all data from <see cref="DAL.Repositories.ApplyStampsRepository"/>.
		/// </summary>
		public IEnumerable<ApplyStamp> GetAll()
		{
			return _context.ApplyStamps;
		}

		/// <summary>
		/// Method for fetching all data from <see cref="DAL.Repositories.ApplyStampsRepository"/> with paggination.
		/// </summary>
		public IEnumerable<ApplyStamp> GetAll(int page, int pageSize)
		{
			return _context.ApplyStamps.Skip(page * pageSize).Take(pageSize);
		}

		/// <summary>
		/// Method for fetching <see cref="ApplyStamp"/>.by id (primary key).
		/// </summary>
		public ApplyStamp Get(Guid id)
		{
			return _context.ApplyStamps.Find(id);
		}

		/// <summary>
		/// Async method for fetching <see cref="ApplyStamp"/>.by id (primary key).
		/// </summary>
		public async Task<ApplyStamp> GetAsync(Guid id)
		{
			return await _context.ApplyStamps.FindAsync(id);
		}

		/// <summary>
		/// Method for fetching <see cref="ApplyStamp"/>.s) by predicate.
		/// </summary>
		public IQueryable<ApplyStamp> Find(Expression<Func<ApplyStamp, bool>> predicate)
		{
			return _context.ApplyStamps.Where(predicate);
		}

		/// <summary>
		/// Method for creating <see cref="ApplyStamp"/>.
		/// </summary>
		public void Create(ApplyStamp item)
		{
			_context.ApplyStamps.Add(item);
		}

		/// <summary>
		/// Async method for creating <see cref="ApplyStamp"/>.
		/// </summary>
		public async Task CreateAsync(ApplyStamp item)
		{
			await _context.ApplyStamps.AddAsync(item);
		}

		/// <summary>
		/// Method for updating <see cref="ApplyStamp"/>.
		/// </summary>
		public void Update(ApplyStamp item)
		{
			_context.Entry(item).State = EntityState.Modified;
		}

		/// <summary>
		/// Method for deleting <see cref="ApplyStamp"/>.
		/// </summary>
		public void Delete(Guid id)
		{
			var item = _context.ApplyStamps.Find(id);

			if (item != null)
			{
				_context.ApplyStamps.Remove(item);
			}
		}

		/// <summary>
		/// Async method for deleting <see cref="ApplyStamp"/>.
		/// </summary>
		public async Task DeleteAsync(Guid id)
		{
			var item = await _context.ApplyStamps.FindAsync(id);

			if (item != null)
			{
				_context.ApplyStamps.Remove(item);
			}
		}

		#endregion
	}
}
