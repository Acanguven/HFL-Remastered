using HFL_3._0.Client;
using Microsoft.Win32.SafeHandles;
using Microsoft.WindowsAPICodePack.Taskbar;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

namespace HFL_3._0
{
    public class RotationJob
    {
        [DllImport("kernel32.dll")]
        public static extern SafeWaitHandle CreateWaitableTimer(IntPtr lpTimerAttributes,
                                                          bool bManualReset,
                                                        string lpTimerName);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWaitableTimer(SafeWaitHandle hTimer,
                                                    [In] ref long pDueTime,
                                                              int lPeriod,
                                                           IntPtr pfnCompletionRoutine,
                                                           IntPtr lpArgToCompletionRoutine,
                                                             bool fResume);

        [DllImport("Powrprof.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);


        private Rotation rotation;
        private bool _done = false;
        public bool running = false;
        private BackgroundWorker bgWorker = new BackgroundWorker();

        public bool done
        {
            get { return _done; }
            set
            {
                _done = value;
                if (running)
                {
                    if (value == true)
                    {
                        stop();
                    }
                    rotation.taskUpdate();
                }
            }
        }
        public RotationType type;
        public int waitTime = 0;
        public string name;
        public SmurfData smurfData;
        public int smurfTime = 0;

        public RotationJob(ref Rotation _rotation, SmurfData smurf, string _name)
        {
            rotation = _rotation;
            type = RotationType.SmurfDone;
            name = _name;
            smurfData = smurf;
        }

        public RotationJob(ref Rotation _rotation, SmurfData smurf, string _name, int minutes)
        {
            rotation = _rotation;
            type = RotationType.SmurfTime;
            name = _name;
            smurfTime = minutes;
            smurfData = smurf;
        }

        public RotationJob(ref Rotation _rotation, GroupData group, string _name)
        {
            rotation = _rotation;
            type = RotationType.GroupDone;
            name = _name;
        }

        public RotationJob(ref Rotation _rotation, GroupData group, string _name, int minutes)
        {
            rotation = _rotation;
            type = RotationType.GroupTime;
            name = _name;
            smurfTime = minutes;
        }

        public RotationJob(ref Rotation _rotation, int minutes, string _name)
        {
            rotation = _rotation;
            type = RotationType.Wait;
            waitTime = minutes;
            name = _name;
        }

        public RotationJob(ref Rotation _rotation, int minutes, string _name, bool hibernate)
        {
            rotation = _rotation;
            type = RotationType.SleepWait;
            waitTime = minutes;
            name = _name;
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            bgWorker.RunWorkerCompleted +=
              new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
        }

        public void run()
        {

            switch (type)
            {
                case RotationType.Wait:
                    wait();
                    break;
                case RotationType.SleepWait:
                    DateTime utc = DateTime.Now.AddMinutes(waitTime);
                    long waketime = utc.ToFileTime();
                    running = true;
                    bgWorker.RunWorkerAsync(waketime);
                    SetSuspendState(false, true, true);
                    break;
                case RotationType.SmurfDone:
                    running = true;
                    Smurf smurfDoneInstance = new Smurf(smurfData, type);
                    smurfDoneInstance.smurfCompleted += smurfCompleted;
                    break;
                case RotationType.SmurfTime:
                    running = true;
                    Smurf smurfTimeInstance = new Smurf(smurfData, type, smurfTime);
                    smurfTimeInstance.smurfCompleted += smurfCompleted;
                    break;
            }
        }

        public void complete()
        {
            if (type == RotationType.SleepWait)
            {
                Console.WriteLine("Task compeleted after wake ->" + name);
            }
            else
            {
                Console.WriteLine("Task compeleted ->" + name);
            }

            /* Complete Task */
            done = true;
        }

        private void smurfCompleted(object sender)
        {
            complete();
        }

        public void stop()
        {
            if (running)
            {
                Console.WriteLine("Task stopped. ->" + name);

                /* Stop Task */
                running = false;
            }
        }

        private void wait()
        {
            Console.WriteLine("Waiting " + waitTime + " Task Starting ->" + name);
            new Thread(() =>
            {
                Thread.CurrentThread.Name = name;
                running = true;
                int totalWaited = 0;
                while (totalWaited < waitTime * 1000)
                {
                    if (Rotator.running)
                    {
                        Thread.Sleep(1000);
                        if (!Rotator.paused)
                        {
                            totalWaited = totalWaited + 1000;
                            if (TaskbarManager.IsPlatformSupported)
                            {
                                if (!Rotator.running)
                                {
                                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
                                }
                                else
                                {
                                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Normal);
                                    TaskbarManager.Instance.SetProgressValue(totalWaited, waitTime * 1000);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (TaskbarManager.IsPlatformSupported)
                        {
                            TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
                        }
                        break;
                    }
                }
                complete();
            }).Start();
        }


        void bgWorker_RunWorkerCompleted(object sender,
                      RunWorkerCompletedEventArgs e)
        {
            complete();
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Console.WriteLine("Sleeping " + waitTime + " Task Starting ->" + name);
            long waketime = (long)e.Argument;

            using (SafeWaitHandle handle =
                      CreateWaitableTimer(IntPtr.Zero, true, "Hands Free Leveler Wake Up Timer"))
            {
                if (SetWaitableTimer(handle, ref waketime, 0,
                                       IntPtr.Zero, IntPtr.Zero, true))
                {
                    using (EventWaitHandle wh = new EventWaitHandle(false,
                                                           EventResetMode.AutoReset))
                    {
                        wh.SafeWaitHandle = handle;
                        wh.WaitOne();
                    }
                }
                else
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
        }
    }

}
