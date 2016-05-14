using System.Runtime.Serialization;

namespace HFL_3._0
{
		[DataContract]
    public class User
    {
				[DataMember]
        public long Date { get; set; }

        public string Hwid { get; set; }

				[DataMember]
        public string Token { get; set; }

				[DataMember]
        public forumData ForumData { get; set; }

				[DataMember]
        public userData UserData { get; set; } 

        User(){
            Token = "";
            Hwid = HWID.Generate();
        }
    }

		[DataContract]
    public class forumData
    {
				[DataMember]
        public string Name { get; set; }
    }

		[DataContract]
    public class userData
    {
				[DataMember]
        public long Trial { get; set; }
				[DataMember]
        public long HwidCanChange { get; set; }
				[DataMember]
        public int Type { get; set; }
				[DataMember]
        public settings Settings { get; set; }
    }

		[DataContract]
    public class settings
    {
				[DataMember]
        public bool PacketSearch { get; set; }

				[DataMember]
        public bool BuyBoost { get; set; }

				[DataMember]
        public bool Reconnect { get; set; }

				[DataMember]
        public bool DisableGpu { get; set; }

				[DataMember]
        public bool ManualInjection { get; set; }
    }
}