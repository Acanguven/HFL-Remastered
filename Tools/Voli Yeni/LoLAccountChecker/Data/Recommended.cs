namespace LoLAccountChecker.Data
{
    using System;
    using System.Runtime.CompilerServices;

    public class Recommended
    {
        public Block[] blocks { get; set; }

        public string champion { get; set; }

        public string map { get; set; }

        public string mode { get; set; }

        public bool priority { get; set; }

        public string title { get; set; }

        public string type { get; set; }
    }
}

