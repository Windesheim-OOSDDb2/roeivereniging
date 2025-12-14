using System.Text.Json.Serialization;

namespace RoeiVereniging.Core.Models
{
    public class WeerLiveV2Response
    {
        [JsonPropertyName("liveweer")]
        public LiveWeerV2[] LiveWeer { get; set; } = Array.Empty<LiveWeerV2>();

        [JsonPropertyName("wk_verw")]
        public WkVerw[] WkVerw { get; set; } = Array.Empty<WkVerw>();
    }
}