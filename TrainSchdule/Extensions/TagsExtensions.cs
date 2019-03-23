using System.Collections.Generic;
using TrainSchdule.BLL.DTO;
using TrainSchdule.WEB.ViewModels;

namespace TrainSchdule.WEB.Extensions
{
    /// <summary>
    /// Methods for mapping tag DTOs to tag view models.
    /// </summary>
    public static class TagsExtensions
    {
        /// <summary>
        /// Maps tag DTO to tag view model.
        /// </summary>
        public static TagViewModel ToViewModel(this TagDTO item)
        {
            if (item == null)
            {
                return null;
            }

            return new TagViewModel
            {
                id = item.id,
                Name = item.Name
            };
        }

        /// <summary>
        /// Maps tag DTOs to tag view models.
        /// </summary>
        public static List<TagViewModel> ToViewModels(this IEnumerable<TagDTO> items)
        {
            if (items == null)
            {
                return null;
            }

            var tags = new List<TagViewModel>();

            foreach (var item in items)
            {
                tags.Add(new TagViewModel
                {
                    id = item.id,
                    Name = item.Name
                });
            }

            return tags;
        }
    }
}
