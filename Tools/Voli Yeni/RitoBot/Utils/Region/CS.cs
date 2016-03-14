namespace RitoBot.Utils.Region
{
    using LoLLauncher;
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Net;

    internal class CS : BaseRegion
    {
        private static string location;
        private static readonly Dictionary<string, string> vals = getSettings();

        private static Dictionary<string, string> getSettings()
        {
            OpenFileDialog dialog1 = new OpenFileDialog {
                Title = "Find League Of Legends",
                Multiselect = false
            };
            dialog1.ShowDialog();
            location = dialog1.FileName;
            return new Dictionary<string, string>();
        }

        public override string ChatName
        {
            get
            {
                char[] separator = new char[] { '.' };
                return vals["host"].Split(separator)[1];
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
                return vals["platformId"];
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
                return this.Location;
            }
        }

        public override Uri NewsAddress
        {
            get
            {
                return new Uri("http://ll.leagueoflegends.com/landingpage/data/br/en_US.js");
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
                return Region.CS;
            }
        }

        public override string RegionName
        {
            get
            {
                return vals["regionTag"];
            }
        }

        public override string SpectatorIpAddress
        {
            get
            {
                return "nil";
            }
            set
            {
            }
        }

        public override Uri SpectatorLink
        {
            get
            {
                return new Uri(vals["featuredGamesURL"].Replace("featured", ""));
            }
        }
    }
}

