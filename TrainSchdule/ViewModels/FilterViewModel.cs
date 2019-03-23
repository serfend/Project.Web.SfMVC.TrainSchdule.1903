using System;
using Newtonsoft.Json;

namespace TrainSchdule.WEB.ViewModels
{
    public class FilterViewModel
    {
        #region Properties

        [JsonProperty("$id")]
        public Guid id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        #endregion
    }
}
