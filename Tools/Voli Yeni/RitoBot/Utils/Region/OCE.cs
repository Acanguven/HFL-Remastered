namespace RitoBot.Utils.Region
{
    using LoLLauncher;
    using System;
    using System.Net;

    public sealed class OCE : BaseRegion
    {
        public override string ChatName
        {
            get
            {
                return "oc1";
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
                return "OC1";
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
                return new Uri("http://oce.leagueoflegends.com/en/rss.xml");
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
                return Region.OCE;
            }
        }

        public override string RegionName
        {
            get
            {
                return "OCE";
            }
        }

        public override string SpectatorIpAddress
        {
            get
            {
                return "192.64.169.29";
            }
            set
            {
            }
        }

        public override Uri SpectatorLink
        {
            get
            {
                return new Uri("http://spectator.oc1.lol.riotgames.com/observer-mode/rest/");
            }
        }
    }
}

