namespace VoliBot.Utils
{
    using RitoBot.Utils.Region;
    using System;
    using System.Collections;

    internal class Config
    {
        public static ArrayList accounts = new ArrayList();
        public static ArrayList accountsNew = new ArrayList();
        public static bool buyBoost = false;
        public static string championAltPick = "";
        public static string championToPick = "";
        public static string clientVersion = "6.5.16_03_04_23_13";
        public static int connectedAccs = 0;
        public static string LauncherPath;
        public static int maxBots = 5;
        public static int maxLevel = 0x1f;
        public static BaseRegion Region;
        public static bool replaceConfig = false;
        public static bool rndIcon = true;
        public static bool rndSpell = false;
        public static bool showPopup = true;
        public static string spell1 = "flash";
        public static string spell2 = "ignite";
    }
}

