namespace RitoBot.Utils.Region
{
    using LoLLauncher;
    using System;
    using System.Net;

    public sealed class BR : BaseRegion
    {
        public override string ChatName
        {
            get
            {
                return "br";
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
                return "BR1";
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
                return new Uri("http://br.leagueoflegends.com/pt/rss.xml");
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
                return Region.BR;
            }
        }

        public override string RegionName
        {
            get
            {
                return "BR";
            }
        }

        public override string SpectatorIpAddress
        {
            get
            {
                return "66.151.33.19:80";
            }
            set
            {
            }
        }

        public override Uri SpectatorLink
        {
            get
            {
                return new Uri("http://spectator.br.lol.riotgames.com:80/observer-mode/rest/");
            }
        }
    }
}

