using System.Collections.Generic;
using TrainSchdule.BLL.DTO.UserInfo;
using TrainSchdule.WEB.ViewModels;

namespace TrainSchdule.WEB.Extensions
{
    /// <summary>
    /// Methods for mapping comment DTOs to comment view models.
    /// </summary>
    public static class CommentsExtensions
    {
        /// <summary>
        /// Maps comment DTO to comment view model.
        /// </summary>
        public static CommentViewModel ToViewModel(this CommentDTO item)
        {
            if (item == null)
            {
                return null;
            }

            return new CommentViewModel
            {
                id = item.id,
                Text = item.Text,
                Date = item.Date.ToString("MMMM dd, yyyy"),
                Owner = item.Owner.ToViewModel()
            };
        }

        /// <summary>
        /// Maps comment DTOs to comment view models.
        /// </summary>
        public static List<CommentViewModel> ToViewModels(this IEnumerable<CommentDTO> items)
        {
            if (items == null)
            {
                return null;
            }

            var comments = new List<CommentViewModel>();

            foreach (var item in items)
            {
                comments.Add(new CommentViewModel
                {
                    id = item.id,
                    Text = item.Text,
                    Date = item.Date.ToString("MMMM dd, yyyy"),
                    Owner = item.Owner.ToViewModel()
                });
            }

            return comments;
        }
    }
}