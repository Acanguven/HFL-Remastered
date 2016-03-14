namespace RitoBot.Utils.Region
{
    using LoLLauncher;
    using System;
    using System.Net;

    public sealed class TR : BaseRegion
    {
        public override string ChatName
        {
            get
            {
                return "tr";
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
                return "TR1";
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
                return new Uri("http://tr.leagueoflegends.com/tr/rss.xml");
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
                return Region.TR;
            }
        }

        public override string RegionName
        {
            get
            {
                return "TR";
            }
        }

        public override string SpectatorIpAddress
        {
            get
            {
                return "95.172.65.242:80";
            }
            set
            {
            }
        }

        public override Uri SpectatorLink
        {
            get
            {
                return new Uri("http://spectator.tr.lol.riotgames.com:80/observer-mode/rest/");
            }
        }
    }
}

