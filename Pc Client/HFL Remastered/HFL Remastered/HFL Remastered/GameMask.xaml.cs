using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Threading;

namespace HFL_Remastered
{
    /// <summary>
    /// Interaction logic for GameMask.xaml
    /// </summary>
    public partial class GameMask : Window
    {

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr Handle, int Msg, int wParam, int lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll")]
        static extern bool EnableWindow(IntPtr hWnd, bool enable);
        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("winmm.dll", EntryPoint = "waveOutSetVolume")]
        public static extern int WaveOutSetVolume(IntPtr hwo, uint dwVolume);
        [DllImport("winmm.dll", SetLastError = true)]
        public static extern bool PlaySound(string pszSound, IntPtr hmod, uint fdwSound);
        [DllImport("user32.dll")]
        static extern int SetWindowText(IntPtr hWnd, string text);
        [DllImport("user32.dll")]
        static extern bool UpdateWindow(IntPtr hWnd);
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);


        private const int SW_SHOW = 5;
        private const int SW_HIDE = 0;
        public ObservableCollection<handle> smurfList = new ObservableCollection<handle>();
        CustomWindow fake = new CustomWindow();
        BackgroundWorker bw = new BackgroundWorker();
        BackgroundWorker md = new BackgroundWorker();
        BackgroundWorker antiSplat = new BackgroundWorker();
        IntPtr windowHandle;


        public GameMask()
        {
            InitializeComponent();
            this.DataContext = this;
            runningSmurfs.ItemsSource = smurfList;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            md.DoWork += new DoWorkEventHandler(modifier);
            antiSplat.DoWork += new DoWorkEventHandler(antiBugsplat);

            terminateAllGames();

            md.RunWorkerAsync();
            antiSplat.RunWorkerAsync();
        }

        public int runningCount()
        {
            return smurfList.Count;
        }

        public void gameEnded(string username)
        {

        }

        private void antiBugsplat(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (true)
                {
                    Process[] anotherApps = Process.GetProcessesByName("League of Legends");
                    foreach (Process game in anotherApps)
                    {
                        if (anotherApps != null)
                        {
                            List<System.IntPtr> allChildWindows = new WindowHandleInfo(game.MainWindowHandle).GetAllChildHandles();
                            if (allChildWindows.Count != 0 && allChildWindows.Count < 10)
                            {
                                game.Kill();
                            }
                        }
                    }
                    Thread.Sleep(3500);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void terminateAllGames()
        {
            while (Process.GetProcessesByName("League of Legends").Length > 0)
            {
                Process[] anotherApps = Process.GetProcessesByName("League of Legends");
                foreach (Process game in anotherApps)
                {
                    game.Kill();
                    Thread.Sleep(500);
                }
            }
            bw.RunWorkerAsync();
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            IntPtr HWND;
            while (FindWindow("RiotWindowClass", "League of Legends (TM) Client") == (IntPtr)0)
            {
                Thread.Sleep(100);
            }
            HWND = FindWindow("RiotWindowClass", "League of Legends (TM) Client");
            while (true)
            {
                if (HWND != (IntPtr)0)
                {
                    if (IsWindowVisible(HWND))
                    {
                        ShowWindow(HWND, SW_HIDE);
                    }
                }
                Thread.Sleep(1000);
            }
        }

        public void addWindow(Process exe, string smurfName, string region)
        {
            handle existedSmurf = smurfList.FirstOrDefault(e => (e.smurfName == smurfName));
            if (existedSmurf != null)
            {
                existedSmurf.process = exe;
            }
            else
            {
                handle newSmurf = new handle(exe, smurfName, region);
                smurfList.Add(newSmurf);
            }
            modifyWindows();
        }

        public void modifier(object sender, DoWorkEventArgs e)
        {
            try
            {
                Random rnd = new Random();
                while (true)
                {
                    if (windowHandle == (IntPtr)0)
                    {
                        windowHandle = new WindowInteropHelper(this).Handle;
                    }
                    else
                    {
                        foreach (handle smurf in smurfList)
                        {

                            if (GetParent(smurf.process.MainWindowHandle) != windowHandle)
                            {
                                SetParent(smurf.process.MainWindowHandle, windowHandle);
                                MoveWindow(smurf.process.MainWindowHandle, 0, 0, 640, 480, true);
                                SetWindowLong(smurf.process.MainWindowHandle, -16, 0x11800000);
                            }
                            UpdateWindow(smurf.process.MainWindowHandle);
                        }
                        SetWindowText(Process.GetProcessesByName("League of Legends")[0].MainWindowHandle, "Grey you are awesome!  " + rnd.Next(1, 50).ToString());
                    }
                }
                Thread.Sleep(3000);
            }
            catch (Exception ex)
            {

            }
        }


        public void modifyWindows()
        {
            foreach (handle smurf in smurfList)
            {
                Thread.Sleep(150);
                IntPtr windowHandle = new WindowInteropHelper(this).Handle;
                SetParent(smurf.process.MainWindowHandle, windowHandle);
                MoveWindow(smurf.process.MainWindowHandle, 0, 0, 640, 480, true);
                SetWindowLong(smurf.process.MainWindowHandle, -16, 0x11800000);
                EnableWindow(smurf.process.MainWindowHandle, false);
                ShowWindow(smurf.process.MainWindowHandle, SW_HIDE);
            }
        }

        public class handle
        {
            public string smurfName { get; set; }
            public string smurfRegion { get; set; }
            internal Process process;

            public handle(Process _pro, string _smuf, string _region)
            {
                process = _pro;
                smurfName = _smuf;
                smurfRegion = _region;
            }
        }

        private void smurfWindow_click(object sender, RoutedEventArgs e)
        {
            hideAll();
            FrameworkElement ownerGui = ((FrameworkElement)sender);
            handle obj = ownerGui.DataContext as handle;
            ShowWindow(obj.process.MainWindowHandle, SW_SHOW);

        }

        public void hideAll()
        {
            foreach (handle exe in smurfList)
            {
                ShowWindow(exe.process.MainWindowHandle, SW_HIDE);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = true;
        }

        public void killAll()
        {
            foreach (handle exe in smurfList)
            {
                try
                {
                    exe.process.Kill();
                }
                catch (Exception)
                {

                }
            }
        }

        public void terminateUserGame(string username)
        {
            try
            {
                Process toKill = null;
                Process[] processes = Process.GetProcesses();
                foreach (var process in processes)
                {
                    if (process.MainWindowTitle == username)
                    {
                        toKill = process;
                    }
                }
                if (toKill != null)
                {
                    toKill.Kill();
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
