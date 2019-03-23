using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrainSchdule.BLL.DTO;

namespace TrainSchdule.BLL.Interfaces
{
	public interface IStudentService:IDisposable
	{

		#region Properties

		/// <summary>
		/// Returns filter DTOs for the photo.
		/// </summary>
		List<FilterDTO> Filters { get; }

		/// <summary>
		/// Returns tag DTOs for the photo.
		/// </summary>
		List<TagDTO> Tags { get; }

		#endregion

		#region Methods

		/// <summary>
		/// Loads all students with paggination, returns collection of student DTOs.
		/// </summary>
		IEnumerable<StudentDTO> GetAll(int page, int pageSize);

		/// <summary>
		/// Loads student, returns student DTO.
		/// </summary>
		StudentDTO Get(Guid id);

		/// <summary>
		/// Async loads student, returns student DTO.
		/// </summary>
		Task<StudentDTO> GetAsync(Guid id);

		Guid Create(StudentDTO item);


		ValueTask<Guid> CreateAsync(StudentDTO item);

		void Edit(Guid id, StudentDTO item);


		Task EditAsync(Guid id, StudentDTO item);

		void Delete(Guid id);

		Task DeleteAsync(Guid id);

		#endregion
	}
}
