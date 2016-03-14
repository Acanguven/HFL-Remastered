namespace LoLAccountChecker.Data
{
    using System;
    using System.Runtime.CompilerServices;

    public class Block
    {
        public string hideIfSummonerSpell { get; set; }

        public Item[] items { get; set; }

        public int maxSummonerLevel { get; set; }

        public int minSummonerLevel { get; set; }

        public bool recMath { get; set; }

        public string showIfSummonerSpell { get; set; }

        public string type { get; set; }
    }
}

