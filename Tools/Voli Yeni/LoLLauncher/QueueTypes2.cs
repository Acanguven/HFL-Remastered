namespace LoLLauncher
{
    using System;
    using System.Collections.Generic;

    public class QueueTypes2
    {
        public Dictionary<string, int> dict;

        public QueueTypes2()
        {
            Dictionary<string, int> dictionary1 = new Dictionary<string, int>();
            dictionary1.Add("NORMAL-5x5", 2);
            dictionary1.Add("RANKED_SOLO-5x5", 4);
            dictionary1.Add("BOT-5x5", 7);
            dictionary1.Add("NORMAL-3x3", 8);
            dictionary1.Add("NORMAL-5x5-draft", 14);
            dictionary1.Add("ODIN-5x5", 0x10);
            dictionary1.Add("ODIN-5x5-draft", 0x11);
            dictionary1.Add("ODINBOT-5x5", 0x19);
            dictionary1.Add("RANKED_TEAM-3x3", 0x29);
            dictionary1.Add("RANKED_TEAM-5x5", 0x2a);
            dictionary1.Add("BOT_TT-3x3", 0x34);
            dictionary1.Add("ARAM-5x5", 0x41);
            this.dict = dictionary1;
        }
    }
}

