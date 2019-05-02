using System.Collections.Generic;
using Newtonsoft.Json;
using TrainSchdule.DAL.Entities.UserInfo;

namespace TrainSchdule.WEB.ViewModels
{
    public class UserDetailsViewModel : UserViewModel
    {
        #region Properties

        [JsonProperty("about")]
        public string About { get; set; }

        [JsonProperty("gender")]
        public GenderEnum Gender { get; set; }

        [JsonProperty("webSite")]
        public string WebSite { get; set; }

        [JsonProperty("followings")]
        public List<UserViewModel> Followings { get; set; }

        [JsonProperty("followers")]
        public List<UserViewModel> Followers { get; set; }

        [JsonProperty("mutuals")]
        public List<UserViewModel> Mutuals { get; set; }

        #endregion
    }
}