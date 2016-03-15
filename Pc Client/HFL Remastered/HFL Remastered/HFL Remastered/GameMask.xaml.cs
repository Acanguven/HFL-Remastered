using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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

        private const int SW_SHOW = 5;
        private const int SW_HIDE = 0;
        public ObservableCollection<handle> smurfList = new ObservableCollection<handle>();
        CustomWindow fake = new CustomWindow();
        BackgroundWorker bw = new BackgroundWorker();



        public GameMask()
        {
            InitializeComponent();
            this.DataContext = this;
            runningSmurfs.ItemsSource = smurfList;
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerAsync();
        }

        public int runningCount()
        {
            return smurfList.Count;
        }

        public void gameEnded(string username)
        {

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
                Thread.Sleep(100);
            }
        }

        public void addWindow(Process exe, string smurfName)
        {
            handle existedSmurf = smurfList.FirstOrDefault(e => (e.smurfName == smurfName));
            if (existedSmurf != null)
            {
                existedSmurf.process = exe;
            }
            else
            {
                handle newSmurf = new handle(exe, smurfName);
                smurfList.Add(newSmurf);
            }
            modifyWindows();
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
            internal Process process;

            public handle(Process _pro, string _smuf)
            {
                process = _pro;
                smurfName = _smuf;
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
    }
}
