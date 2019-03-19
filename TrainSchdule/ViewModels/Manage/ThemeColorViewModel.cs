using Newtonsoft.Json;

namespace TrainSchdule.WEB.ViewModels.Manage
{
    public class ThemeColorViewModel
    {
        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("class")]
        public string CssClass { get; set; }
    }
}