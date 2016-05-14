using Awesomium.Core;
using Microsoft.WindowsAPICodePack.Taskbar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Interop;

namespace HFL_3._0
{
    public class WindowMask
    {

        #region Constants

        static readonly int GWL_STYLE = -16;

        static readonly int DWM_TNP_VISIBLE = 0x8;
        static readonly int DWM_TNP_OPACITY = 0x4;
        static readonly int DWM_TNP_RECTDESTINATION = 0x1;

        static readonly ulong WS_VISIBLE = 0x10000000L;
        static readonly ulong WS_BORDER = 0x00800000L;
        static readonly ulong TARGETWINDOW = WS_BORDER | WS_VISIBLE;

        private const int GWL_EX_STYLE = -20;
        private const int WS_EX_APPWINDOW = 0x00040000, WS_EX_TOOLWINDOW = 0x00000080;

        private const int SW_HIDE = 0x00;
        private const int SW_SHOW = 0x05;

        private const int GWL_EXSTYLE = -0x14;

        [Flags]
        public enum SetWindowPosFlags : uint
        {
            // ReSharper disable InconsistentNaming

            /// <summary>
            ///     If the calling thread and the thread that owns the window are attached to different input queues, the system posts the request to the thread that owns the window. This prevents the calling thread from blocking its execution while other threads process the request.
            /// </summary>
            SWP_ASYNCWINDOWPOS = 0x4000,

            /// <summary>
            ///     Prevents generation of the WM_SYNCPAINT message.
            /// </summary>
            SWP_DEFERERASE = 0x2000,

            /// <summary>
            ///     Draws a frame (defined in the window's class description) around the window.
            /// </summary>
            SWP_DRAWFRAME = 0x0020,

            /// <summary>
            ///     Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE is sent only when the window's size is being changed.
            /// </summary>
            SWP_FRAMECHANGED = 0x0020,

            /// <summary>
            ///     Hides the window.
            /// </summary>
            SWP_HIDEWINDOW = 0x0080,

            /// <summary>
            ///     Does not activate the window. If this flag is not set, the window is activated and moved to the top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter parameter).
            /// </summary>
            SWP_NOACTIVATE = 0x0010,

            /// <summary>
            ///     Discards the entire contents of the client area. If this flag is not specified, the valid contents of the client area are saved and copied back into the client area after the window is sized or repositioned.
            /// </summary>
            SWP_NOCOPYBITS = 0x0100,

            /// <summary>
            ///     Retains the current position (ignores X and Y parameters).
            /// </summary>
            SWP_NOMOVE = 0x0002,

            /// <summary>
            ///     Does not change the owner window's position in the Z order.
            /// </summary>
            SWP_NOOWNERZORDER = 0x0200,

            /// <summary>
            ///     Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent window uncovered as a result of the window being moved. When this flag is set, the application must explicitly invalidate or redraw any parts of the window and parent window that need redrawing.
            /// </summary>
            SWP_NOREDRAW = 0x0008,

            /// <summary>
            ///     Same as the SWP_NOOWNERZORDER flag.
            /// </summary>
            SWP_NOREPOSITION = 0x0200,

            /// <summary>
            ///     Prevents the window from receiving the WM_WINDOWPOSCHANGING message.
            /// </summary>
            SWP_NOSENDCHANGING = 0x0400,

            /// <summary>
            ///     Retains the current size (ignores the cx and cy parameters).
            /// </summary>
            SWP_NOSIZE = 0x0001,

            /// <summary>
            ///     Retains the current Z order (ignores the hWndInsertAfter parameter).
            /// </summary>
            SWP_NOZORDER = 0x0004,

            /// <summary>
            ///     Displays the window.
            /// </summary>
            SWP_SHOWWINDOW = 0x0040,

            // ReSharper restore InconsistentNaming
        }

        #endregion

        #region DWM functions

        [DllImport("dwmapi.dll")]
        static extern int DwmRegisterThumbnail(IntPtr dest, IntPtr src, out IntPtr thumb);

        [DllImport("dwmapi.dll")]
        static extern int DwmUnregisterThumbnail(IntPtr thumb);

        [DllImport("dwmapi.dll")]
        static extern int DwmQueryThumbnailSourceSize(IntPtr thumb, out PSIZE size);

        [DllImport("dwmapi.dll")]
        static extern int DwmUpdateThumbnailProperties(IntPtr hThumb, ref DWM_THUMBNAIL_PROPERTIES props);

        #endregion

        #region Win32 helper functions

        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        [DllImport("user32.dll")]
        static extern ulong GetWindowLongA(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int EnumWindows(EnumWindowsCallback lpEnumFunc, int lParam);
        delegate bool EnumWindowsCallback(IntPtr hwnd, int lParam);

        [DllImport("user32.dll")]
        public static extern void GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnableWindow(IntPtr hWnd, bool bEnable);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

        [DllImport("user32.dll")]
        static extern int SetWindowText(IntPtr hWnd, string text);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);


        #endregion

        #region Interop structs

        [StructLayout(LayoutKind.Sequential)]
        internal struct DWM_THUMBNAIL_PROPERTIES
        {
            public int dwFlags;
            public Rect rcDestination;
            public Rect rcSource;
            public byte opacity;
            public bool fVisible;
            public bool fSourceClientAreaOnly;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Rect
        {
            internal Rect(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct PSIZE
        {
            public int x;
            public int y;
        }

        #endregion

        private IntPtr thumb;

        List<Mask> gameList = new List<Mask>();

        public WindowMask()
        {
            App.NET.BindAsync("loadMaskedGames", loadMaskedGames);
            App.NET.BindAsync("showMaskGoOn", showMaskGoOn);
            App.NET.BindAsync("ClearDW", ClearDW);
            App.NET.BindAsync("renderOpt", renderOpt);
        }

        public void addGame(string username, string region, Process process)
        {
            Mask t = new Mask();
            t.username = username;
            t.region = region;
            t.process = process;
            t.maskStatus = MaskStatus.NotMasked;
            gameList.Add(t);

            if (!App.main.bolRunning && App.settings.disableGpu)
            {
                workOn(process);
            }
        }

        public void removeGame(Process process)
        {
            if (gameExist(process.Id))
            {
                gameList.Remove(gameList.Single(s => s.process.Id == process.Id));
            }
        }

        private void renderOpt(object sender, JavascriptMethodEventArgs e)
        {
            if (e.Arguments[0] != JSValue.Undefined)
            {
                if (!e.Arguments[1])
                {
                    maskEnable((int)e.Arguments[0]);
                }
                else
                {
                    maskDisable((int)e.Arguments[0]);
                }

                JSObject callbackarg = e.Arguments[2];
                JSObject cb = callbackarg.Clone();

                if (gameExist((int)e.Arguments[0]))
                {
                    bool masked = gameList.Find(i => i.process.Id == (int)e.Arguments[0]).maskStatus == MaskStatus.Masked;
                    cb?.Invoke("call", callbackarg, masked);
                }
            }
        }

        private void ClearDW(object sender, JavascriptMethodEventArgs e)
        {
            DwmUnregisterThumbnail(thumb);
        }

        private void loadMaskedGames(object sender, JavascriptMethodEventArgs e)
        {
            JSObject callbackarg = e.Arguments[0];
            JSObject cb = callbackarg.Clone();

            JSValue[] js_arr = new JSValue[gameList.Count];
            JSObject game;
            int count = 0;
            foreach (var _game in gameList)
            {

                game = new JSObject();
                game["username"] = _game.username;
                game["region"] = _game.region;
                game["processId"] = _game.process.Id;
                game["masked"] = _game.maskStatus == MaskStatus.Masked;
                js_arr[count] = game;
                count++;
            }
            cb?.Invoke("call", callbackarg, js_arr);
        }

        public void findAndKillHflWindow(string username, string region)
        {
            string titleName = "HFL:" + username + region;
            IntPtr hwnd = FindWindow(null, titleName);
            if(hwnd != (IntPtr)0)
            {
                uint processID = 0;
                GetWindowThreadProcessId(hwnd, out processID);
                Process.GetProcessById((int)processID).Kill();
            }          
        }

        private void showMaskGoOn(object sender, JavascriptMethodEventArgs e)
        {
            if (Process.GetProcesses().Any(x => x.Id == (int)e.Arguments[0]))
            {
                makeDWMvisible((int)e.Arguments[0], (int)e.Arguments[1], (int)e.Arguments[2], (int)e.Arguments[3], (int)e.Arguments[4]);
            }
        }

        private void makeDWMvisible(int processId, int left, int top, int width, int height)
        {
            App.main.Dispatcher.Invoke(() =>
            {
                if (thumb != IntPtr.Zero)
                    DwmUnregisterThumbnail(thumb);

                IntPtr mainHandle = new WindowInteropHelper(App.main).Handle;
                IntPtr targetProcess = Process.GetProcessById(processId).MainWindowHandle;
                int i = DwmRegisterThumbnail(mainHandle, targetProcess, out thumb);



                if (i == 0)
                    UpdateThumb(left, top, width, height);
            });
        }

        public void workOn(int processId)
        {
            if (App.settings.disableGpu)
            {
                maskEnable(processId);
            }

            string titleName = "HFL:" + gameList.Find(i => i.process.Id == processId).username + gameList.Find(i => i.process.Id == processId).region;
            while (!Process.GetProcessById(processId).MainWindowTitle.Contains("HFL"))
            {
                var Handle = Process.GetProcessById(processId).MainWindowHandle;                
                SetWindowText(Handle, titleName);
                System.Threading.Thread.Sleep(500);
            }
        }

        public void workOn(Process process)
        {
            if (App.settings.disableGpu)
            {
                maskEnable(process.Id);
            }

            string titleName = "HFL:" + gameList.Find(i => i.process.Id == process.Id).username + gameList.Find(i => i.process.Id == process.Id).region;
            while (!Process.GetProcessById(process.Id).MainWindowTitle.Contains("HFL"))
            {
                var Handle = Process.GetProcessById(process.Id).MainWindowHandle;                
                SetWindowText(Handle, titleName);
                System.Threading.Thread.Sleep(500);
            }
        }

        public void maskEnable(int processID)
        {
            if (gameExist(processID) && gameList.Find(i => i.process.Id == processID).maskStatus == MaskStatus.NotMasked)
            {
                var Handle = Process.GetProcessById(processID).MainWindowHandle;


                if (Process.GetProcesses().Any(x => x.Id == processID))
                {
                    Rect windowRect = new Rect();
                    bool positionSuccess = false;
                    while (!positionSuccess)
                    {
                        Handle = Process.GetProcessById(processID).MainWindowHandle;
                        EnableWindow(Handle, false);
                        SetWindowPos(Handle, new IntPtr(1), -10000, -10000, 0, 0, SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOACTIVATE);

                        System.Threading.Thread.Sleep(500);
                        GetWindowRect(Handle, ref windowRect);
                        if (Math.Abs(windowRect.Left + 10000) < 100 && Math.Abs(windowRect.Top + 10000) < 100)
                        {
                            positionSuccess = true;
                        }
                    }

                    if (TaskbarManager.IsPlatformSupported)
                    {
                        TaskbarManager.Instance.SetApplicationIdForSpecificWindow(Handle, "HFLGROUP");
                    }
                    gameList.Find(i => i.process.Id == processID).maskStatus = MaskStatus.Masked;
                }
                else
                {
                    gameList.Remove(gameList.Single(s => s.process.Id == processID));
                }
            }
        }

        public void maskDisable(int processID)
        {
            if (gameExist(processID) && gameList.Find(i => i.process.Id == processID).maskStatus != MaskStatus.NotMasked)
            {
                if (Process.GetProcesses().Any(x => x.Id == processID))
                {
                    var Handle = Process.GetProcessById(processID).MainWindowHandle;
                    EnableWindow(Handle, true);
                    SetWindowPos(Handle, new IntPtr(1), 0, 0, 0, 0, SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOACTIVATE);
                    gameList.Find(i => i.process.Id == processID).maskStatus = MaskStatus.NotMasked;
                    if (TaskbarManager.IsPlatformSupported)
                    {
                        Random rnd = new Random();
                        TaskbarManager.Instance.SetApplicationIdForSpecificWindow(Handle, "HFLSEP" + rnd.Next(1, 50000));
                    }
                }
                else
                {
                    gameList.Remove(gameList.Single(s => s.process.Id == processID));
                }
            }
        }

        public bool gameExist(int processId)
        {
            return gameList.Exists(i => i.process.Id == processId);
        }

        #region Update thumbnail properties

        private void UpdateThumb(int left, int top, int width, int height)
        {
            if (thumb != IntPtr.Zero)
            {
                PSIZE size;
                DwmQueryThumbnailSourceSize(thumb, out size);

                DWM_THUMBNAIL_PROPERTIES props = new DWM_THUMBNAIL_PROPERTIES();

                props.fVisible = true;
                props.dwFlags = DWM_TNP_VISIBLE | DWM_TNP_RECTDESTINATION | DWM_TNP_OPACITY;
                props.opacity = (byte)255;
                props.rcDestination = new Rect(left, top, left + width, top + height);

                if (size.x < width)
                    props.rcDestination.Right = props.rcDestination.Left + size.x;

                if (size.y < height)
                    props.rcDestination.Bottom = props.rcDestination.Top + size.y;

                DwmUpdateThumbnailProperties(thumb, ref props);
            }
        }

        #endregion
    }

    public class Mask
    {
        public Process process { get; set; }
        public MaskStatus maskStatus { get; set; }
        public string username { get; set; }
        public string region { get; set; }
    }

    public enum MaskStatus
    {
        Masked,
        NotMasked
    }
}
