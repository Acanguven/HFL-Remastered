using LoLLauncher;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HFL_Remastered
{
    public class Group
    {
        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("queue")]
        public QueueTypes queue { get; set; }

        [JsonProperty("smurfs")]
        public List<Smurf> smurfs { get; set; }
    }
}
