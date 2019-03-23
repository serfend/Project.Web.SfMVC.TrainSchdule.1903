using System.Collections.Generic;
using TrainSchdule.BLL.DTO;
using TrainSchdule.WEB.ViewModels;

namespace TrainSchdule.WEB.Extensions
{
    /// <summary>
    /// Methods for mapping like DTOs to like view models.
    /// </summary>
    public static class LikesExtensions
    {
        /// <summary>
        /// Maps like DTO to like view model.
        /// </summary>
        public static LikeViewModel ToViewModel(this LikeDTO item)
        {
            if (item == null)
            {
                return null;
            }

            return new LikeViewModel
            {
                id = item.id,
                Date = item.Date.ToString("MMMM dd, yyyy"),
                Owner = item.Owner.ToViewModel()
            };
        }

        /// <summary>
        /// Maps like DTOs to like view models.
        /// </summary>
        public static List<LikeViewModel> ToViewModels(this IEnumerable<LikeDTO> items)
        {
            if (items == null)
            {
                return null;
            }

            var likes = new List<LikeViewModel>();

            foreach (var item in items)
            {
                likes.Add(new LikeViewModel
                {
                    id = item.id,
                    Date = item.Date.ToString("MMMM dd, yyyy"),
                    Owner = item.Owner.ToViewModel()
                });
            }

            return likes;
        }
    }
}