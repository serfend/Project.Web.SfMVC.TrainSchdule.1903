using Newtonsoft.Json;

namespace TrainSchdule.WEB.ViewModels.Manage
{
    public class UserSettingsViewModel
    {
        [JsonProperty("theme")]
        public ThemeColorViewModel ThemeColor { get; set; }

        [JsonProperty("accent")]
        public ThemeColorViewModel AccentColor { get; set; }
    }
}