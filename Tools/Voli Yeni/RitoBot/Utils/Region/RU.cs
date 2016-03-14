namespace RitoBot.Utils.Region
{
    using LoLLauncher;
    using System;
    using System.Net;

    public sealed class RU : BaseRegion
    {
        public override string ChatName
        {
            get
            {
                return "ru";
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
                return "RU";
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
                return new Uri("http://ru.leagueoflegends.com/ru/rss.xml");
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
                return Region.RU;
            }
        }

        public override string RegionName
        {
            get
            {
                return "RU";
            }
        }

        public override string SpectatorIpAddress
        {
            get
            {
                return "95.172.65.242";
            }
            set
            {
            }
        }

        public override Uri SpectatorLink
        {
            get
            {
                return new Uri("http://spectator.ru.lol.riotgames.com/observer-mode/rest/");
            }
        }
    }
}

