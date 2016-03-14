namespace LoLAccountChecker.Data
{
    using System;
    using System.Runtime.CompilerServices;

    public class Spell
    {
        public Altimage[] altimages { get; set; }

        public float[] cooldown { get; set; }

        public string cooldownBurn { get; set; }

        public int[] cost { get; set; }

        public string costBurn { get; set; }

        public string costType { get; set; }

        public string description { get; set; }

        public float[][] effect { get; set; }

        public string[] effectBurn { get; set; }

        public string id { get; set; }

        public Image2 image { get; set; }

        public string key { get; set; }

        public Leveltip leveltip { get; set; }

        public string maxammo { get; set; }

        public int maxrank { get; set; }

        public object[] modes { get; set; }

        public string name { get; set; }

        public object range { get; set; }

        public string rangeBurn { get; set; }

        public string resource { get; set; }

        public int summonerLevel { get; set; }

        public string tooltip { get; set; }

        public Var[] vars { get; set; }
    }
}

