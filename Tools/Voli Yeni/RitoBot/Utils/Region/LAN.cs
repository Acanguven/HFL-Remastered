namespace RitoBot.Utils.Region
{
    using LoLLauncher;
    using System;
    using System.Net;

    public sealed class LAN : BaseRegion
    {
        public override string ChatName
        {
            get
            {
                return "la1";
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
                return "LA1";
            }
        }

        public override string Locale
        {
            get
            {
                return "es_MX";
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
                return new Uri("http://lan.leagueoflegends.com/es/rss.xml");
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
                return Region.LA1;
            }
        }

        public override string RegionName
        {
            get
            {
                return "LAN";
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
                return new Uri("http://spectator.la1.lol.riotgames.com:80/observer-mode/rest/");
            }
        }
    }
}

