using System.Linq;
using System.Collections.Generic;
using TrainSchdule.DAL.Entities.UserInfo;

namespace TrainSchdule.BLL.Helpers
{
    /// <summary>
    /// Extensions class with method for parsing strings with tag names. 
    /// </summary>
    public static class TagParserExtensions
    {
        /// <summary>
        /// Extension method for parsing tags from string to <see cref="Tag"/> array.
        /// </summary>
        public static IEnumerable<Tag> SplitTags(this string strings, IEnumerable<Tag> tags)
        {
            var response = new List<Tag>();

            foreach (var tagString in strings.Split(new char[] { ',', '.', '-', '_', ' ', ':', '/' }))
            {
				//新赋值的tag是否在tag库中
                var tag = tags.FirstOrDefault(t => t.Name == tagString);

                if(tag != null)
                {
                    response.Add(tag);
                }
				//仅保留最多5个tag
                if (response.Count > 5)
                {
                    break;
                }
            }

            return response;
        }
    }
}
