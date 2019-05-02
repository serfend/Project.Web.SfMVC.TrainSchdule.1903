﻿using System.Collections.Generic;
using TrainSchdule.DAL.Entities.UserInfo;
using TrainSchdule.BLL.DTO;

namespace TrainSchdule.BLL.Extensions
{
    /// <summary>
    /// Methods for mapping tag entities to tag data transfer objects.
    /// </summary>
    public static class TagsExtensions
    {
        /// <summary>
        /// Maps tag entity to tag DTO.
        /// </summary>
        public static TagDTO ToDTO(this Tag item)
        {
            if (item == null)
            {
                return null;
            }

            return new TagDTO
            {
                id = item.Id,
                Name = item.Name
            };
        }

        /// <summary>
        /// Maps tag entities to tag DTOs.
        /// </summary>
        public static List<TagDTO> ToDTOs(this IEnumerable<Tag> items)
        {
            if (items == null)
            {
                return null;
            }

            var tags = new List<TagDTO>();

            foreach (var tag in items)
            {
                tags.Add(new TagDTO
                {
                    id = tag.Id,
                    Name = tag.Name
                });
            }

            return tags;
        }
    }
}
