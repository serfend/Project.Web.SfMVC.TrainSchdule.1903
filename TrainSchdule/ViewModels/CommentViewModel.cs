using System;
using Newtonsoft.Json;

namespace TrainSchdule.WEB.ViewModels
{
    public class CommentViewModel
    {
        #region Properties

        [JsonProperty("$id")]
        public Guid id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("owner")]
        public UserViewModel Owner { get; set; }

        #endregion
    }
}