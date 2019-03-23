using System;

namespace TrainSchdule.BLL.DTO
{
    /// <summary>
    /// Tag data transfer object.
    /// Contains tag id and name.
    /// </summary>
    public class TagDTO
    {
        /// <summary>
        /// Gets and sets tag id.
        /// </summary>
        public Guid id { get; set; }

        /// <summary>
        /// Gets and sets tag name.
        /// </summary>
        public string Name { get; set; }
    }
}
