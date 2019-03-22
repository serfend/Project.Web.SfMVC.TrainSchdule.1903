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
		StudentDTO Get(int id);

		/// <summary>
		/// Async loads student, returns student DTO.
		/// </summary>
		Task<StudentDTO> GetAsync(int id);

		int Create(StudentDTO item);


		ValueTask<int> CreateAsync(StudentDTO item);

		void Edit(int id, StudentDTO item);


		Task EditAsync(int id, StudentDTO item);

		void Delete(int id);

		Task DeleteAsync(int id);

		#endregion
	}
}
