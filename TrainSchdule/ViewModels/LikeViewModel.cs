using Newtonsoft.Json;

namespace TrainSchdule.WEB.ViewModels
{
    public class LikeViewModel
    {
        #region Properties

        [JsonProperty("$id")]
        public int Id { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("owner")]
        public UserViewModel Owner { get; set; }

        #endregion
    }
}