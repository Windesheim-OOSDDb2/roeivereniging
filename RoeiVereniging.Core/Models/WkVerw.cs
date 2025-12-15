using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RoeiVereniging.Core.Models
{
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
}
