namespace RitoBot.Utils.Region
{
    using LoLLauncher;
    using System;
    using System.Net;

    public sealed class KR : BaseRegion
    {
        public void NASqlite()
        {
        }

        public override string ChatName
        {
            get
            {
                return "kr";
            }
        }

        public override bool Garena
        {
            get
            {
                return false;
            }
        }

        public override string InternalName
        {
            get
            {
                return "KR";
            }
        }

        public override string Locale
        {
            get
            {
                return "ko_KR";
            }
        }

        public override string Location
        {
            get
            {
                return null;
            }
        }

        public override Uri NewsAddress
        {
            get
            {
                return new Uri("");
            }
        }

        public override IPAddress[] PingAddresses
        {
            get
            {
                return new IPAddress[0];
            }
        }

        public override Region PVPRegion
        {
            get
            {
                return Region.KR;
            }
        }

        public override string RegionName
        {
            get
            {
                return "KR";
            }
        }

        public override string SpectatorIpAddress
        {
            get
            {
                return "110.45.191.11:80";
            }
            set
            {
            }
        }

        public override Uri SpectatorLink
        {
            get
            {
                return new Uri("http://QFKR1PROXY.kassad.in:8088/observer-mode/rest/");
            }
        }
    }
}

