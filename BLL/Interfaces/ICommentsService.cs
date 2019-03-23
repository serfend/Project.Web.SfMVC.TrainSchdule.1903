using System;
using System.Threading.Tasks;

namespace TrainSchdule.BLL.Interfaces
{
    /// <summary>
    /// Interface for comments services.
    /// Contains methods with comments processing logic.
    /// </summary>
    public interface ICommentsService : IDisposable
    {
        /// <summary>
        /// Adds comment by commented photo id and comment text.
        /// </summary>
        Guid? Add(Guid photoId, string text);

        /// <summary>
        /// Async adds comment by commented photo id and comment text.
        /// </summary>
        Task<Guid?> AddAsync(Guid photoId, string text);

        /// <summary>
        /// Deletes comment by comment id.
        /// </summary>
        void Delete(Guid id);

        /// <summary>
        /// Async deletes comment by comment id.
        /// </summary>
        Task DeleteAsync(Guid id);
    }
}
