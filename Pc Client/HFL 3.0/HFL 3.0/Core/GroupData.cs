using System;
using System.Collections.Generic;

namespace HFL_3._0
{

    [Serializable]
    public class GroupData
	{
        public List<string> smurfIds = new List<string>();
        public string name { get; set; }
        public string id { get; set; }
        public string queue { get; set; }
    }
}
