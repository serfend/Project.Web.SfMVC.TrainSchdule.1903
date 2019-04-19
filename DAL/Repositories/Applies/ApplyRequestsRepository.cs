
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
	public class ApplyRequestsRepository : IRepository<ApplyRequest>
	{
		#region Fields

		private readonly ApplicationDbContext _context;

		#endregion

		#region .ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="ApplyRequestsRepository"/>.
		/// </summary>
		public ApplyRequestsRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		#endregion

		#region Logic

		/// <summary>
		/// Method for fetching all data from <see cref="ApplyRequestsRepository"/>.
		/// </summary>
		public IEnumerable<ApplyRequest> GetAll()
		{
			return _context.ApplyRequests;
		}

		/// <summary>
		/// Method for fetching all data from <see cref="ApplyRequestsRepository"/> with paggination.
		/// </summary>
		public IEnumerable<ApplyRequest> GetAll(int page, int pageSize)
		{
			return _context.ApplyRequests.Skip(page * pageSize).Take(pageSize);
		}

		/// <summary>
		/// Method for fetching <see cref="ApplyRequest"/>.by id (primary key).
		/// </summary>
		public ApplyRequest Get(Guid id)
		{
			return _context.ApplyRequests.Find(id);
		}

		/// <summary>
		/// Async method for fetching <see cref="ApplyRequest"/>.by id (primary key).
		/// </summary>
		public async Task<ApplyRequest> GetAsync(Guid id)
		{
			return await _context.ApplyRequests.FindAsync(id);
		}

		/// <summary>
		/// Method for fetching <see cref="ApplyRequest"/>.s) by predicate.
		/// </summary>
		public IQueryable<ApplyRequest> Find(Expression<Func<ApplyRequest, bool>> predicate)
		{
			return _context.ApplyRequests.Where(predicate);
		}

		/// <summary>
		/// Method for creating <see cref="ApplyRequest"/>.
		/// </summary>
		public void Create(ApplyRequest item)
		{
			_context.ApplyRequests.Add(item);
		}

		/// <summary>
		/// Async method for creating <see cref="ApplyRequest"/>.
		/// </summary>
		public async Task CreateAsync(ApplyRequest item)
		{
			await _context.ApplyRequests.AddAsync(item);
		}

		/// <summary>
		/// Method for updating <see cref="ApplyRequest"/>.
		/// </summary>
		public void Update(ApplyRequest item)
		{
			_context.Entry(item).State = EntityState.Modified;
		}

		/// <summary>
		/// Method for deleting <see cref="ApplyRequest"/>.
		/// </summary>
		public void Delete(Guid id)
		{
			var item = _context.ApplyRequests.Find(id);

			if (item != null)
			{
				_context.ApplyRequests.Remove(item);
			}
		}

		/// <summary>
		/// Async method for deleting <see cref="ApplyRequest"/>.
		/// </summary>
		public async Task DeleteAsync(Guid id)
		{
			var item = await _context.ApplyRequests.FindAsync(id);

			if (item != null)
			{
				_context.ApplyRequests.Remove(item);
			}
		}

		#endregion
	}
}
