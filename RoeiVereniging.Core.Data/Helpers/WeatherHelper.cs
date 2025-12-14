using RoeiVereniging.Core.Models;
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
            try
            {
                if (string.IsNullOrWhiteSpace(locatie)) throw new ArgumentException("locatie is required", nameof(locatie));
                if (string.IsNullOrWhiteSpace(apiKey)) throw new ArgumentException("apiKey is required", nameof(apiKey));

                string endpoint = $"api/weerlive_api_v2.php?key={Uri.EscapeDataString(apiKey)}&locatie={Uri.EscapeDataString(locatie)}";

                using var response = await _httpClient.GetAsync(endpoint, cancellationToken).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                var data = JsonSerializer.Deserialize<WeerLiveV2Response>(json, _jsonOptions);

                return data;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching weather data: {ex.Message}");
                return null;
            }
        }
    }

}