using Newtonsoft.Json;
using System.Collections.Generic;

namespace InAGalaxyFarFarFarAway
{
    class StarshipWrapper
    {
        public int Count { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }

        [JsonProperty("results")]
        public List<Starship> Starships { get; set; }
    }
}
