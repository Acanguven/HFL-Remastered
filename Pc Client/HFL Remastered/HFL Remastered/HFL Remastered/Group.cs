using LoLLauncher;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFL_Remastered
{
    public class Group
    {
        [JsonProperty("name")]
        public QueueTypes name { get; set; }

        [JsonProperty("queue")]
        public QueueTypes queue { get; set; }

        [JsonProperty("smurfs")]
        public List<Smurf> smurfs { get; set; }
    }
}
