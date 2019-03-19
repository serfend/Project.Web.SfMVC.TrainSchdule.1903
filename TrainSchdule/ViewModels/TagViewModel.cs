using Newtonsoft.Json;

namespace TrainSchdule.WEB.ViewModels
{
    public class TagViewModel
    {
        #region Properties

        [JsonProperty("$id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        #endregion
    }
}