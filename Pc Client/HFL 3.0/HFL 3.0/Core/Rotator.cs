using HFL_3._0.Client;
using HFL_3._0.Core;
using Microsoft.WindowsAPICodePack.Taskbar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HFL_3._0
{
    public static class Rotator
    {
        public static List<Rotation> rotationList = new List<Rotation>();
        public static Stopwatch stopWatch = new Stopwatch();
        public static Awesomium.Core.JSObject callbackarg;
        public static bool running = false;
        public static bool paused = false;
        public static bool done = false;

        public static int loopAmount = 1;
        public static int currentLoop = 1;

        public static void updateCallback(Awesomium.Core.JSObject cb)
        {
            callbackarg = cb;
            notifyBrowser();
        }

        public static string getSessionTime()
        {

            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}",
            ts.Hours, ts.Minutes, ts.Seconds);
            return elapsedTime;
        }

        public static void notifyBrowser()
        {
            App.main.Dispatcher.Invoke(() =>
            {
                if (callbackarg)
                {
                    Awesomium.Core.JSObject status = new Awesomium.Core.JSObject();
                    status["running"] = running;
                    status["paused"] = paused;
                    status["done"] = done;
                    status["currentLoop"] = currentLoop;
                    status["loopAmount"] = loopAmount;
                    callbackarg?.Invoke("call", callbackarg, status);
                }
            });

        }


        public static void rotationUpdate()
        {
            if (running)
            {
                Console.WriteLine("Rotations Updated");
                Rotation rt = rotationList.FirstOrDefault(j => j.done == false);
                if (rt != null)
                {
                    if (!paused)
                    {
                        rt.start();
                    }
                }
                else
                {
                    complete();
                }
            }
        }

        public static void setLimit(int _loopAmount)
        {
            loopAmount = _loopAmount;
        }

        public static void addRotation(Rotation rt)
        {
            rotationList.Add(rt);
        }

        public static void start()
        {
            if (!running && !paused)
            {
                try
                {
                    sessionManager.createSession();
                    buildRotationList();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                    return;
                }

            }
            if (!running || paused)
            {
                done = false;
                if (rotationList.Count > 0)
                {
                    if (!paused)
                    {
                        foreach (Rotation rt in rotationList)
                        {
                            rt.done = false;
                        }
                        sessionManager.startSession();
                    }
                    sessionManager.setState(sessionState.RUNNING);
                    Rotation firstRt = rotationList.FirstOrDefault(rt => rt.done == false);
                    if (firstRt != null)
                    {
                        running = true;
                        paused = false;
                        if (!stopWatch.IsRunning)
                        {
                            stopWatch.Reset();
                            stopWatch.Start();
                        }
                        firstRt.start();
                    }
                    else
                    {
                        foreach (Rotation rt in rotationList)
                        {
                            rt.done = false;
                        }
                        start();
                    }
                }
            }
            else
            {
                Console.WriteLine("Rotator already running");
            }
            notifyBrowser();
        }

        public static void pause()
        {
            if (running)
            {
                Console.WriteLine("Rotator paused");
                sessionManager.setState(sessionState.PAUSED);
                paused = true;
            }
            else
            {
                Console.WriteLine("Rotator is not running");
            }
            notifyBrowser();
        }

        public static void stop(bool preventRest = false)
        {
            if (running)
            {
                running = false;
                paused = false;
            }
            else
            {
                Console.WriteLine("Rotator is not running");
            }
            notifyBrowser();

            if (!running && !preventRest)
            {
                if (stopWatch.IsRunning)
                {
                    stopWatch.Stop();
                }
            }

            if (TaskbarManager.IsPlatformSupported)
            {
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
            }

            if (!preventRest)
            {
                sessionManager.endSession(sessionState.STOPPED);
            }
        }

        public static void complete()
        {
            if (currentLoop < loopAmount)
            {
                stop(true);
                currentLoop++;
                Console.WriteLine("Rotator round:" + currentLoop);
                start();
            }
            else
            {
                done = true;
                currentLoop = 1;
                Console.WriteLine("Rotator ended");
                sessionManager.endSession(sessionState.COMPLETED);
                stop();
            }
        }

        public static void buildRotationList()
        {
            LoLLauncher.RegionInfo.buildAll();
            rotationList.Clear();
            List<rotationJobSave> _TRotList = Storage.DeSerializeObject<List<rotationJobSave>>("rotations.xml");
            List<SmurfData> _TsmurfList = Storage.DeSerializeObject<List<SmurfData>>("smurfs.xml");
            List<GroupData> _groupList = Storage.DeSerializeObject<List<GroupData>>("groups.xml");
            Rotation tRot;
            foreach (rotationJobSave rot in _TRotList)
            {
                if (rot.queuePos != "" && rot.queuePos != null)
                {
                    if (rot.type == "wait")
                    {
                        tRot = new Rotation();
                        tRot.initRot();
                        tRot.type = rot.type;
                        tRot.waitTime = rot.waittime;
                        tRot.pcSleep = rot.pcsleep;
                        RotationJob task;
                        if (rot.pcsleep)
                        {
                            task = new RotationJob(ref tRot, rot.waittime, "Sleep Waiting Task", true);
                        }
                        else
                        {
                            task = new RotationJob(ref tRot, rot.waittime * 60, "Waiting Task");
                        }
                        
                        tRot.addTask(task);
                        sessionManager.addRotation(ref tRot);
                        addRotation(tRot);
                    }
                    if (rot.type == "task")
                    {
                        tRot = new Rotation();
                        tRot.initRot();
                        tRot.type = rot.type;
                        tRot.waitTime = rot.waittime;
                        tRot.endType = rot.endType;
                        RotationJob task;
                        foreach (var smrf in rot.smurfIds)
                        {
                            if (_TsmurfList.Exists(i => i.id == smrf))
                            {
                                SmurfData foundSmurf = _TsmurfList.First(i => i.id == smrf);
                                if (rot.endType == "timer")
                                {
                                    task = new RotationJob(ref tRot, foundSmurf, foundSmurf.username, rot.waittime);
                                }
                                else
                                {
                                    task = new RotationJob(ref tRot, foundSmurf, foundSmurf.username);
                                }
                                tRot.addTask(task);
                            }
                            else
                            {
                                throw new Exception("Smurf with id " + smrf + " can't be found on smurf list.");
                            }
                        }
                        sessionManager.addRotation(ref tRot);
                        addRotation(tRot);
                    }
                }
            }
        }
    }
}
