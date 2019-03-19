using System.Collections.Generic;
using TrainSchdule.BLL.DTO;
using TrainSchdule.WEB.ViewModels;

namespace TrainSchdule.WEB.Extensions
{
    /// <summary>
    /// Methods for mapping filter DTOs to filter view models.
    /// </summary>
    public static class FiltersExtensions
    {
        /// <summary>
        /// Maps filter DTO to filter view model.
        /// </summary>
        public static FilterViewModel ToViewModel(this FilterDTO item)
        {
            if (item == null)
            {
                return null;
            }

            return new FilterViewModel
            {
                Id = item.Id,
                Name = item.Name
            };
        }

        /// <summary>
        /// Maps filter DTOs to filter view models.
        /// </summary>
        public static List<FilterViewModel> ToViewModels(this IEnumerable<FilterDTO> items)
        {
            if (items == null)
            {
                return null;
            }

            var filters = new List<FilterViewModel>();

            foreach (var item in items)
            {
                filters.Add(new FilterViewModel
                {
                    Id = item.Id,
                    Name = item.Name
                });
            }

            return filters;
        }
    }
}
