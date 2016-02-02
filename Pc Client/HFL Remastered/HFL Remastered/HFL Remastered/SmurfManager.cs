using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoLLauncher;
using Newtonsoft.Json;

namespace HFL_Remastered
{
    public static class SmurfManager
    {
        public static List<Smurf> smurfs = new List<Smurf>();
        public static List<Group> groups = new List<Group>();

        public static void addSmurf(Smurf newSmurf)
        {
            bool containsSmurf = smurfs.Any(smurf => smurf.username == newSmurf.username);
            if (!containsSmurf)
            {
                smurfs.Add(newSmurf);
                Logger.Log newLog = new Logger.Log("info");
                newLog.Text = "Smurf " + newSmurf.username + " registered to system.";
                Logger.Push(newLog);
                newSmurf.loadSelf();
            }
            else
            {
                Logger.Log newLog = new Logger.Log("warning");
                newLog.Text = "Smurf " + newSmurf.username + " already registered to system!";
                Logger.Push(newLog);
            }
        }
    }
}
