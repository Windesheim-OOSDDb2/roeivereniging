using RoeiVereniging.Core.Models;

namespace RoeiVereniging.Core.Helpers
{
    public static class WeatherEvaluationHelper
    {
        public static bool IsDangerousWeather(
            LiveWeerV2? weather,
            string[] dangerKeywords,
            int maxWindBft)
        {
            if (weather is null)
                return false;

            string imageKey = weather.Image ?? string.Empty;

            foreach (var kw in dangerKeywords)
            {
                if (!imageKey.Contains(kw, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            if (weather.WindBft >= maxWindBft)
            {
                return true;
            }

            return false;
        }

        public static string GetBackgroundColorForWeather(
            LiveWeerV2? weather,
            string[] dangerKeywords,
            int maxWindBft,
            double coldThreshold)
        {
            if (IsDangerousWeather(weather, dangerKeywords, maxWindBft))
            {
                return "#D7263D";
            }

            return GetBackgroundColorForTemperature(weather?.Temp, coldThreshold);
        }

        public static string GetBackgroundColorForTemperature(double? temp, double coldThreshold)
        {
            if (temp is null)
            {
                return "#D7263D";
            }

            return temp < coldThreshold ? "#D7263D" : "#0854D1";
        }
    }
}