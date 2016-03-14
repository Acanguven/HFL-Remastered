namespace LoLAccountChecker.Data
{
    using System;
    using System.Runtime.CompilerServices;

    public class ChampionFull
    {
        public Champion[] data { get; set; }

        public string format { get; set; }

        public string[] keys { get; set; }

        public string type { get; set; }

        public string version { get; set; }
    }
}

