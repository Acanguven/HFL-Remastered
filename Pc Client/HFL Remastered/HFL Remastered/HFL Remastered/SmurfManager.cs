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

        public static void addGroup(Group group)
        {
            bool containsGroup = groups.Any(gr => gr.name == group.name);
            if (!containsGroup)
            {
                groups.Add(group);
                Logger.Push("Group " + group.name + " registered to system. Waiting members to login to start...", "info");

                bool hostIgnited = false;
                foreach (var smurf in group.smurfs)
                {
                    if (!hostIgnited)
                    {
                        smurf.isHost = true;
                        hostIgnited = true;
                    }
                    else
                    {
                        smurf.hostCallback = group.smurfs[0];
                    }
                    smurf.groupMember = true;
                    smurf.queue = group.queue;
                    smurf.totalGroupLength = group.smurfs.Count;
                    smurf.loadSelf();
                    smurf.start();
                }
            }
        }

        public static void stopGroup(Group group)
        {
            try {
                Group groupFound = groups.First(gr => gr.name == group.name);
                if (groupFound != null)
                {
                    foreach (Smurf smurf in groupFound.smurfs)
                    {
                        smurf.stop();
                        group.smurfs.Remove(smurf);
                    }
                }
                groups.Remove(groupFound);
            }
            catch (Exception ex)
            {
                
            }
        }

        public static void addSmurf(Smurf newSmurf)
        {
            bool containsSmurf = smurfs.Any(smurf => smurf.username == newSmurf.username);
            if (!containsSmurf)
            {
                smurfs.Add(newSmurf);
                Logger.Push("Smurf " + newSmurf.username + " registered to system.", "info");
                newSmurf.loadSelf();
                newSmurf.start();
            }
            else
            {
                Logger.Push("Smurf " + newSmurf.username + " already registered to system!", "info");
                newSmurf.start();
            }
            SmurfManager.updateStatus();
        }

        public static void stopSmurf(Smurf smurf)
        {
            try { 
                Smurf containsSmurf = smurfs.First(pendSmurf => pendSmurf.username == smurf.username);
                if (containsSmurf != null)
                {
                    containsSmurf.stop();
                    smurfs.Remove(containsSmurf);
                }
                SmurfManager.updateStatus();
            }catch(Exception ex){

            }
        }

        public static void updateStatus()
        {
            Network.upateSmurfs(smurfs, groups);
        }
    }
}
