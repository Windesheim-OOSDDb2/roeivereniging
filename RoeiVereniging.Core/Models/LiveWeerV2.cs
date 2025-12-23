using System;
using System.Text.Json.Serialization;

namespace RoeiVereniging.Core.Models
{
    public class LiveWeerV2
    {
        [JsonPropertyName("plaats")]
        public string Plaats { get; set; }

        [JsonPropertyName("temp")]
        public double Temp { get; set; }

        [JsonPropertyName("gtemp")]
        public double GTemp { get; set; }

        [JsonPropertyName("samenv")]
        public string Samenv { get; set; }

        [JsonPropertyName("lv")]
        public int LV { get; set; }

        [JsonPropertyName("windr")]
        public string WindR { get; set; }

        [JsonPropertyName("windkmh")]
        public double WindKmH { get; set; }

        [JsonPropertyName("windbft")]
        public int WindBft { get; set; }

        [JsonPropertyName("sup")]
        public string Sup { get; set; }

        [JsonPropertyName("sunder")]
        public string Sunder { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }
    }
}
