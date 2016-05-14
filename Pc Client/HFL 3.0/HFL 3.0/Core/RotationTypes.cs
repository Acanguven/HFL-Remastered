using System;
using System.Collections.Generic;

namespace HFL_3._0
{
    public enum RotationType
	{
		Wait,
		SleepWait,
		SmurfDone,
		SmurfTime,
		GroupDone,
		GroupTime,
	};


    [Serializable]
    public class rotationJobSave
    {
        public string name { get; set; }
        public string type { get;set;}
        public double left { get; set; }
        public double top { get; set; }
        public string id { get; set; }
        public string queuePos { get; set; }
        public string endType { get; set; }
        public List<string> smurfIds = new List<string>();
        public List<string> groupIds = new List<string>();
        public int waittime { get; set; }
        public bool pcsleep { get; set; }
    }
}
