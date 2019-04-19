
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
		public class ApplyResponsesRepository : IRepository<ApplyResponse>
		{
			#region Fields

			private readonly ApplicationDbContext _context;

			#endregion

			#region .ctors

			/// <summary>
			/// Initializes a new instance of the <see cref="AppliesRepository"/>.
			/// </summary>
			public ApplyResponsesRepository(ApplicationDbContext context)
			{
				_context = context;
			}

			#endregion

			#region Logic

			/// <summary>
			/// Method for fetching all data from <see cref="AppliesRepository"/>.
			/// </summary>
			public IEnumerable<ApplyResponse> GetAll()
			{
				return _context.ApplyResponses;
			}

			/// <summary>
			/// Method for fetching all data from <see cref="AppliesRepository"/> with paggination.
			/// </summary>
			public IEnumerable<ApplyResponse> GetAll(int page, int pageSize)
			{
				return _context.ApplyResponses.Skip(page * pageSize).Take(pageSize);
			}

			/// <summary>
			/// Method for fetching <see cref="ApplyResponse"/>.by id (primary key).
			/// </summary>
			public ApplyResponse Get(Guid id)
			{
				return _context.ApplyResponses.Find(id);
			}

			/// <summary>
			/// Async method for fetching <see cref="ApplyResponse"/>.by id (primary key).
			/// </summary>
			public async Task<ApplyResponse> GetAsync(Guid id)
			{
				return await _context.ApplyResponses.FindAsync(id);
			}

			/// <summary>
			/// Method for fetching <see cref="ApplyResponse"/>.s) by predicate.
			/// </summary>
			public IQueryable<ApplyResponse> Find(Expression<Func<ApplyResponse, bool>> predicate)
			{
				return _context.ApplyResponses.Where(predicate);
			}

			/// <summary>
			/// Method for creating <see cref="ApplyResponse"/>.
			/// </summary>
			public void Create(ApplyResponse item)
			{
				_context.ApplyResponses.Add(item);
			}

			/// <summary>
			/// Async method for creating <see cref="ApplyResponse"/>.
			/// </summary>
			public async Task CreateAsync(ApplyResponse item)
			{
				await _context.ApplyResponses.AddAsync(item);
			}

			/// <summary>
			/// Method for updating <see cref="ApplyResponse"/>.
			/// </summary>
			public void Update(ApplyResponse item)
			{
				_context.Entry(item).State = EntityState.Modified;
			}

			/// <summary>
			/// Method for deleting <see cref="ApplyResponse"/>.
			/// </summary>
			public void Delete(Guid id)
			{
				var item = _context.ApplyResponses.Find(id);

				if (item != null)
				{
					_context.ApplyResponses.Remove(item);
				}
			}

			/// <summary>
			/// Async method for deleting <see cref="ApplyResponse"/>.
			/// </summary>
			public async Task DeleteAsync(Guid id)
			{
				var item = await _context.ApplyResponses.FindAsync(id);

				if (item != null)
				{
					_context.ApplyResponses.Remove(item);
				}
			}

			#endregion
		}
	}
