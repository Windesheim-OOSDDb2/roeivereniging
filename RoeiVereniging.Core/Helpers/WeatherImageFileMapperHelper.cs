using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVereniging.Core.Helpers
{
    public class WeatherImageFileMapperHelper
    {
        public static string MapToImageFile(string? key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return $"bewolkt.png";

            var k = key.Trim().ToLowerInvariant();
            string filename = k switch
            {
                "zonnig" => "zonnig.png",
                "bliksem" => "bliksem.png",
                "regen" => "regen.png",
                "buien" => "buien.png",
                "hagel" => "hagel.png",
                "mist" => "mist.png",
                "sneeuw" => "sneeuw.png",
                "bewolkt" => "bewolkt.png",
                "lichtbewolkt" => "lichtbewolkt.png",
                "halfbewolkt" => "halfbewolkt.png",
                "halfbewolkt_regen" => "halfbewolkt_regen.png",
                "zwaarbewolkt" => "zwaarbewolkt.png",
                "nachtmist" => "nachtmist.png",
                "helderenacht" => "helderenacht.png",
                "nachtbewolkt" => "nachtbewolkt.png",
                _ => $"{k}.png"
            };

            return $"{filename}";
        }
    }

}
