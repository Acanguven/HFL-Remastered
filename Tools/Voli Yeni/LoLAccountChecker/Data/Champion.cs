namespace LoLAccountChecker.Data
{
    using System;
    using System.Runtime.CompilerServices;

    public class Champion
    {
        public string[] allytips { get; set; }

        public string blurb { get; set; }

        public string[] enemytips { get; set; }

        public string id { get; set; }

        public Image image { get; set; }

        public Info info { get; set; }

        public string key { get; set; }

        public string lore { get; set; }

        public string name { get; set; }

        public string partype { get; set; }

        public Passive passive { get; set; }

        public Recommended[] recommended { get; set; }

        public Skin[] skins { get; set; }

        public Spell[] spells { get; set; }

        public Stats stats { get; set; }

        public string[] tags { get; set; }

        public string title { get; set; }
    }
}

