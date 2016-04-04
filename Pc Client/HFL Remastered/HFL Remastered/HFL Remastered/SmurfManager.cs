using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HFL_Remastered
{
    public static class SmurfManager
    {
        public static List<Smurf> smurfs = new List<Smurf>();
        public static List<Group> groups = new List<Group>();
        public static List<Smurf> recoverySmurfs = new List<Smurf>();
        public static List<Group> recoveryGroups = new List<Group>();

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
        
        public static void recover()
        {
            if (File.Exists("smurfs.recovery")) {
                foreach(Smurf smurf in FileManager.DeSerializeObject<List<Smurf>>("smurfs.recovery"))
                {
                    addSmurf(smurf);
                }
            }
            if (File.Exists("groups.recovery"))
            {
                foreach(Group group in FileManager.DeSerializeObject<List<Group>>("groups.recovery"))
                {
                    addGroup(group);
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
            bool containsSmurf = smurfs.Any(smurf => smurf.username == newSmurf.username && smurf.region == newSmurf.region);
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
            updateStatus();
        }

        public static void stopSmurf(Smurf smurf)
        {
            try { 
                Smurf containsSmurf = smurfs.First(pendSmurf => pendSmurf.username == smurf.username && pendSmurf.region == smurf.region);
                if (containsSmurf != null)
                {
                    
                    int indexNum = smurfs.IndexOf(containsSmurf);
                    containsSmurf.Dispose();
                    smurfs.RemoveAt(indexNum);
                    
                }
                updateStatus();
            }catch(Exception ex){

            }
        }

        public static void updateStatus()
        {
            Network.upateSmurfs(smurfs, groups);
        }
    }
}
