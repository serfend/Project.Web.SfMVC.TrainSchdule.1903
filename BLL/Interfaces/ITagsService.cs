using System;
using System.Collections.Generic;
using TrainSchdule.BLL.DTO.UserInfo;

namespace TrainSchdule.BLL.Interfaces
{
    /// <summary>
    /// Interface for tag services.
    /// Contains methods with tags processing logic.
    /// </summary>
    public interface ITagsService : IDisposable
    {
        /// <summary>
        /// Loads all tags and returns collection of tag DTOs.
        /// </summary>
        IEnumerable<TagDTO> GetAll();

        /// <summary>
        /// Loads tag by name and returns tag DTO.
        /// </summary>
        TagDTO Get(string name);
    }
}
