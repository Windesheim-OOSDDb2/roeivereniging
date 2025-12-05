using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace RoeiVereniging.Core.Data.Helpers
{
    /// <summary>
    /// Self-contained helper that contains WeerLive V2 DTOs and a simple method to fetch live weather.
    /// </summary>
    public static class WeatherHelper
    {
        private static readonly HttpClient _httpClient = new() { BaseAddress = new Uri("https://weerlive.nl/") };
        private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        /// <summary>
        /// Calls the WeerLive V2 API and returns the first live weather item for the specified location.
        /// </summary>
        /// <param name="locatie">Location name or "lat,lon".</param>
        /// <param name="apiKey">API key for weerlive.nl</param>
        public static async Task<WeerLiveV2Response?> GetWeatherAsync(string locatie, string apiKey, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(locatie)) throw new ArgumentException("locatie is required", nameof(locatie));
            if (string.IsNullOrWhiteSpace(apiKey)) throw new ArgumentException("apiKey is required", nameof(apiKey));

            string endpoint = $"api/weerlive_api_v2.php?key={Uri.EscapeDataString(apiKey)}&locatie={Uri.EscapeDataString(locatie)}";

            using var response = await _httpClient.GetAsync(endpoint, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            var data = JsonSerializer.Deserialize<WeerLiveV2Response>(json, _jsonOptions);

            // show output in debugger
            Debug.WriteLine(JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }));

            return data;
        }
    }

    public class WeerLiveV2Response
    {
        [JsonPropertyName("liveweer")]
        public LiveWeerV2[] LiveWeer { get; set; } = Array.Empty<LiveWeerV2>();

        [JsonPropertyName("wk_verw")]
        public WkVerw[] WkVerw { get; set; } = Array.Empty<WkVerw>();

        //[JsonPropertyName("uur_verw")]
        //public UurVerw[] UurVerw { get; set; } = Array.Empty<UurVerw>();
    }

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

    public class WkVerw
    {
        [JsonPropertyName("dag")]
        public string Dag { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("max_temp")]
        public double MaxTemp { get; set; }

        [JsonPropertyName("min_temp")]
        public double MinTemp { get; set; }

        [JsonPropertyName("windbft")]
        public int WindBft { get; set; }

        [JsonPropertyName("windkmh")]
        public double WindKmH { get; set; }

        [JsonPropertyName("windknp")]
        public int WindKnp { get; set; }

        [JsonPropertyName("windms")]
        public double WindMs { get; set; }

        [JsonPropertyName("windrgr")]
        public int WindRGr { get; set; }

        [JsonPropertyName("windr")]
        public string WindR { get; set; }

        [JsonPropertyName("neersl_perc_dag")]
        public int NeerslPercDag { get; set; }

        [JsonPropertyName("zond_perc_dag")]
        public int ZondPercDag { get; set; }
    }

    //public class UurVerw
    //{
    //    [JsonPropertyName("uur")]
    //    public string Uur { get; set; }

    //    [JsonPropertyName("timestamp")]
    //    public long Timestamp { get; set; }

    //    [JsonPropertyName("image")]
    //    public string Image { get; set; }

    //    [JsonPropertyName("temp")]
    //    public double Temp { get; set; }

    //    [JsonPropertyName("windbft")]
    //    public int WindBft { get; set; }

    //    [JsonPropertyName("windkmh")]
    //    public double WindKmH { get; set; }

    //    [JsonPropertyName("windknp")]
    //    public int WindKnp { get; set; }

    //    [JsonPropertyName("windms")]
    //    public double WindMs { get; set; }

    //    [JsonPropertyName("windrgr")]
    //    public int WindRGr { get; set; }

    //    [JsonPropertyName("windr")]
    //    public string WindR { get; set; }

    //    [JsonPropertyName("neersl")]
    //    public double Neersl { get; set; }

    //    [JsonPropertyName("gr")]
    //    public int Gr { get; set; }
    //}
}