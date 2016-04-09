using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace HFL_Remastered
{
    class CustomWindow : IDisposable
    {
        delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [System.Runtime.InteropServices.StructLayout(
            System.Runtime.InteropServices.LayoutKind.Sequential,
           CharSet = System.Runtime.InteropServices.CharSet.Unicode
        )]
        struct WNDCLASS
        {
            public uint style;
            public IntPtr lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
            public string lpszMenuName;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
            public string lpszClassName;
        }

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern System.UInt16 RegisterClassW(
            [System.Runtime.InteropServices.In] ref WNDCLASS lpWndClass
        );

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool UpdateWindow(IntPtr hWnd);

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr CreateWindowExW(
           UInt32 dwExStyle,
           [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
       string lpClassName,
           [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
       string lpWindowName,
           UInt32 dwStyle,
           Int32 x,
           Int32 y,
           Int32 nWidth,
           Int32 nHeight,
           IntPtr hWndParent,
           IntPtr hMenu,
           IntPtr hInstance,
           IntPtr lpParam
        );

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern System.IntPtr DefWindowProcW(
            IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam
        );

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern bool DestroyWindow(
            IntPtr hWnd
        );

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int ERROR_CLASS_ALREADY_EXISTS = 1410;

        private IntPtr m_hwnd;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose managed resources
            }

            // Dispose unmanaged resources
            if (m_hwnd != IntPtr.Zero)
            {
                DestroyWindow(m_hwnd);
                m_hwnd = IntPtr.Zero;
            }
        }

        public CustomWindow()
        {
            string class_name = "RiotWindowClass";

            m_wnd_proc_delegate = CustomWndProc;

            // Create WNDCLASS
            WNDCLASS wind_class = new WNDCLASS();
            wind_class.lpszClassName = class_name;
            wind_class.lpfnWndProc = System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(m_wnd_proc_delegate);

            UInt16 class_atom = RegisterClassW(ref wind_class);

            int last_error = System.Runtime.InteropServices.Marshal.GetLastWin32Error();

            // Create window
            m_hwnd = CreateWindowExW(
                0,
                class_name,
                "League of Legends (TM) Client",
                (uint)WindowStyles.WS_BORDER,
                0,
                0,
                640,
                480,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero
            );

            if (m_hwnd == IntPtr.Zero)
            {
                int lastError = Marshal.GetLastWin32Error();
                string errorMessage = new Win32Exception(lastError).Message;
            }

            ShowWindow(m_hwnd, (int)ShowWindowCommands.ShowDefault);
            UpdateWindow(m_hwnd);
        }

        private static IntPtr CustomWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            return DefWindowProcW(hWnd, msg, wParam, lParam);
        }



        private WndProc m_wnd_proc_delegate;
    }

    /// <summary>
    /// Window Styles.
    /// The following styles can be specified wherever a window style is required. After the control has been created, these styles cannot be modified, except as noted.
    /// </summary>
    [Flags()]
    public enum WindowStyles : uint
    {
        /// <summary>The window has a thin-line border.</summary>
        WS_BORDER = 0x800000,

        /// <summary>The window has a title bar (includes the WS_BORDER style).</summary>
        WS_CAPTION = 0xc00000,

        /// <summary>The window is a child window. A window with this style cannot have a menu bar. This style cannot be used with the WS_POPUP style.</summary>
        WS_CHILD = 0x40000000,

        /// <summary>Excludes the area occupied by child windows when drawing occurs within the parent window. This style is used when creating the parent window.</summary>
        WS_CLIPCHILDREN = 0x2000000,

        /// <summary>
        /// Clips child windows relative to each other; that is, when a particular child window receives a WM_PAINT message, the WS_CLIPSIBLINGS style clips all other overlapping child windows out of the region of the child window to be updated.
        /// If WS_CLIPSIBLINGS is not specified and child windows overlap, it is possible, when drawing within the client area of a child window, to draw within the client area of a neighboring child window.
        /// </summary>
        WS_CLIPSIBLINGS = 0x4000000,

        /// <summary>The window is initially disabled. A disabled window cannot receive input from the user. To change this after a window has been created, use the EnableWindow function.</summary>
        WS_DISABLED = 0x8000000,

        /// <summary>The window has a border of a style typically used with dialog boxes. A window with this style cannot have a title bar.</summary>
        WS_DLGFRAME = 0x400000,

        /// <summary>
        /// The window is the first control of a group of controls. The group consists of this first control and all controls defined after it, up to the next control with the WS_GROUP style.
        /// The first control in each group usually has the WS_TABSTOP style so that the user can move from group to group. The user can subsequently change the keyboard focus from one control in the group to the next control in the group by using the direction keys.
        /// You can turn this style on and off to change dialog box navigation. To change this style after a window has been created, use the SetWindowLong function.
        /// </summary>
        WS_GROUP = 0x20000,

        /// <summary>The window has a horizontal scroll bar.</summary>
        WS_HSCROLL = 0x100000,

        /// <summary>The window is initially maximized.</summary> 
        WS_MAXIMIZE = 0x1000000,

        /// <summary>The window has a maximize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified.</summary> 
        WS_MAXIMIZEBOX = 0x10000,

        /// <summary>The window is initially minimized.</summary>
        WS_MINIMIZE = 0x20000000,

        /// <summary>The window has a minimize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The WS_SYSMENU style must also be specified.</summary>
        WS_MINIMIZEBOX = 0x20000,

        /// <summary>The window is an overlapped window. An overlapped window has a title bar and a border.</summary>
        WS_OVERLAPPED = 0x0,

        /// <summary>The window is an overlapped window.</summary>
        WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_SIZEFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,

        /// <summary>The window is a pop-up window. This style cannot be used with the WS_CHILD style.</summary>
        WS_POPUP = 0x80000000u,

        /// <summary>The window is a pop-up window. The WS_CAPTION and WS_POPUPWINDOW styles must be combined to make the window menu visible.</summary>
        WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,

        /// <summary>The window has a sizing border.</summary>
        WS_SIZEFRAME = 0x40000,

        /// <summary>The window has a window menu on its title bar. The WS_CAPTION style must also be specified.</summary>
        WS_SYSMENU = 0x80000,

        /// <summary>
        /// The window is a control that can receive the keyboard focus when the user presses the TAB key.
        /// Pressing the TAB key changes the keyboard focus to the next control with the WS_TABSTOP style.  
        /// You can turn this style on and off to change dialog box navigation. To change this style after a window has been created, use the SetWindowLong function.
        /// For user-created windows and modeless dialogs to work with tab stops, alter the message loop to call the IsDialogMessage function.
        /// </summary>
        WS_TABSTOP = 0x10000,

        /// <summary>The window is initially visible. This style can be turned on and off by using the ShowWindow or SetWindowPos function.</summary>
        WS_VISIBLE = 0x10000000,

        /// <summary>The window has a vertical scroll bar.</summary>
        WS_VSCROLL = 0x200000
    }

    public enum ShowWindowCommands
    {
        /// <summary>
        /// Hides the window and activates another window.
        /// </summary>
        Hide = 0,
        /// <summary>
        /// Activates and displays a window. If the window is minimized or 
        /// maximized, the system restores it to its original size and position.
        /// An application should specify this flag when displaying the window 
        /// for the first time.
        /// </summary>
        Normal = 1,
        /// <summary>
        /// Activates the window and displays it as a minimized window.
        /// </summary>
        ShowMinimized = 2,
        /// <summary>
        /// Maximizes the specified window.
        /// </summary>
        Maximize = 3, // is this the right value?
        /// <summary>
        /// Activates the window and displays it as a maximized window.
        /// </summary>       
        ShowMaximized = 3,
        /// <summary>
        /// Displays a window in its most recent size and position. This value 
        /// is similar to <see cref="Win32.ShowWindowCommand.Normal"/>, except 
        /// the window is not activated.
        /// </summary>
        ShowNoActivate = 4,
        /// <summary>
        /// Activates the window and displays it in its current size and position. 
        /// </summary>
        Show = 5,
        /// <summary>
        /// Minimizes the specified window and activates the next top-level 
        /// window in the Z order.
        /// </summary>
        Minimize = 6,
        /// <summary>
        /// Displays the window as a minimized window. This value is similar to
        /// <see cref="Win32.ShowWindowCommand.ShowMinimized"/>, except the 
        /// window is not activated.
        /// </summary>
        ShowMinNoActive = 7,
        /// <summary>
        /// Displays the window in its current size and position. This value is 
        /// similar to <see cref="Win32.ShowWindowCommand.Show"/>, except the 
        /// window is not activated.
        /// </summary>
        ShowNA = 8,
        /// <summary>
        /// Activates and displays the window. If the window is minimized or 
        /// maximized, the system restores it to its original size and position. 
        /// An application should specify this flag when restoring a minimized window.
        /// </summary>
        Restore = 9,
        /// <summary>
        /// Sets the show state based on the SW_* value specified in the 
        /// STARTUPINFO structure passed to the CreateProcess function by the 
        /// program that started the application.
        /// </summary>
        ShowDefault = 10,
        /// <summary>
        ///  <b>Windows 2000/XP:</b> Minimizes a window, even if the thread 
        /// that owns the window is not responding. This flag should only be 
        /// used when minimizing windows from a different thread.
        /// </summary>
        ForceMinimize = 11
    }

}
