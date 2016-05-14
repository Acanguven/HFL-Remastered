using System;
using System.Collections.Generic;
using System.IO;

namespace HFL_3._0.Core
{
    public static class sessionManager
    {
        private static sessionSave currentSession;
        private static bool sessionExists = false;

        public static void createSession()
        {
            currentSession = new sessionSave()
            {
                startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                state = sessionState.STOPPED,
            };
            sessionExists = true;
        }

        public static void startSession()
        {
            currentSession.state = sessionState.RUNNING;
            Storage.SerializeObject(currentSession, "data/session.xml");
        }

        public static void addRotation(ref Rotation rot)
        {
            currentSession.rotations.Add(rot);
        }

        public static void updateCurrent()
        {
            if (sessionExists && sessionFileExists())
            {
                Storage.SerializeObject(currentSession, "data/session.xml");
            }
        }

        public static void smurfStart(string _username, LoLLauncher.Region _region, double _startXp, double _startIp, double _startLevel, double _expforNextLevel)
        {
            if (sessionExists)
            {
                currentSession.smurfs.Add(new SmurfDataSessionSave()
                {
                    username = _username,
                    region = _region,
                    startXp = _startXp,
                    startIp = _startIp,
                    startLevel = _startLevel,
                    startXpToNextLevel = _expforNextLevel
                });
                updateCurrent();
            }
        }

        public static void smurfEnd(string _username, LoLLauncher.Region _region, double _endXp, double _endIp, double _endLevel, double _expforNextLevel)
        {
            if (sessionExists)
            {
                currentSession.smurfs.Find(i => (i.username == _username && i.region == _region)).endXp = _endXp;
                currentSession.smurfs.Find(i => (i.username == _username && i.region == _region)).endIp = _endIp;
                currentSession.smurfs.Find(i => (i.username == _username && i.region == _region)).endLevel = _endLevel;
                currentSession.smurfs.Find(i => (i.username == _username && i.region == _region)).endXpToNextLevel = _expforNextLevel;
                updateCurrent();
            }
        }

        public static void endSession(sessionState st)
        {
            if (sessionExists)
            {
                sessionExists = false;
                currentSession.endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                currentSession.state = st;
                List<sessionSave> sessions;
                if (File.Exists("data/sessions.xml"))
                {
                    sessions = Storage.DeSerializeObject<List<sessionSave>>("data/sessions.xml");
                }
                else
                {
                    sessions = new List<sessionSave>();
                }
                sessions.Add(currentSession);
                if (sessionFileExists())
                {
                    File.Delete("data/session.xml");
                }
                Storage.SerializeObject(sessions, "data/sessions.xml");
            }
        }

        public static void setState(sessionState st)
        {
            if (sessionExists)
            {
                currentSession.state = st;
                updateCurrent();
            }
        }

        public static bool sessionFileExists()
        {
            if (File.Exists("data/session.xml"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }


    [Serializable]
    public class SmurfDataSessionSave
    {
        public string username { get; set; }
        public LoLLauncher.Region region { get; set; }
        public double startXp { get; set; }
        public double? endXp { get; set; }
        public double startIp { get; set; }
        public double? endIp { get; set; }
        public double startLevel { get; set; }
        public double? endLevel { get; set; }
        public double startXpToNextLevel { get; set; }
        public double? endXpToNextLevel { get; set; }
    }


    [Serializable]
    public class sessionSave
    {
        public string startTime;
        public string endTime;

        public List<SmurfDataSessionSave> smurfs;
        public List<Rotation> rotations;
        public sessionState state;
        public string errorText;

        public sessionSave()
        {
            rotations = new List<Rotation>();
            smurfs = new List<SmurfDataSessionSave>();
        }
    }


    [Serializable]
    public class sessionJobSave
    {
        public RotationType type { get; set; }
        public SmurfDataSessionSave smurf { get; set; }
        public int waittime { get; set; }
    }

    public enum sessionState
    {
        PAUSED,
        RUNNING,
        COMPLETED,
        STOPPED,
        FAILED
    }
}
