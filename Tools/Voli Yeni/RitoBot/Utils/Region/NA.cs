namespace RitoBot.Utils.Region
{
    using LoLLauncher;
    using System;
    using System.Net;

    public sealed class NA : BaseRegion
    {
        public void NASqlite()
        {
        }

        public override string ChatName
        {
            get
            {
                return "na2";
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
                return "NA1";
            }
        }

        public override string Locale
        {
            get
            {
                return "en_US";
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
                return new Uri("http://na.leagueoflegends.com/en/rss.xml");
            }
        }

        public override IPAddress[] PingAddresses
        {
            get
            {
                return new IPAddress[] { IPAddress.Parse("216.52.241.254"), IPAddress.Parse("64.7.194.1"), IPAddress.Parse("66.150.148.1") };
            }
        }

        public override Region PVPRegion
        {
            get
            {
                return Region.NA;
            }
        }

        public override string RegionName
        {
            get
            {
                return "NA";
            }
        }

        public override string SpectatorIpAddress
        {
            get
            {
                return "spectator.na2.lol.riotgames.com:80";
            }
            set
            {
            }
        }

        public override Uri SpectatorLink
        {
            get
            {
                return new Uri("http://spectator.na2.lol.riotgames.com:80/observer-mode/rest/");
            }
        }
    }
}

