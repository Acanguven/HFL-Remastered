using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace HFL_Remastered
{
    public class BolManager
    {
        [DllImport("psapi.dll", SetLastError = true)]
        public static extern uint GetMappedFileName(IntPtr m_hProcess, IntPtr lpv, StringBuilder lpFilename, uint nSize);

        [DllImport("kernel32.dll")]
        static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

        [DllImport("gdi32.dll")]
        private static extern int BitBlt(IntPtr srchDc, int srcX, int srcY, int srcW, int srcH,
                                 IntPtr desthDc, int destX, int destY, int op);

        public static bool dllInjectionCompleted(Process exe)
        {
            bool dllExist = false;
            try
            {
                long MaxAddress = 0x7fffffff;
                StringBuilder fn = new StringBuilder(250);
                IntPtr game = exe.Handle;
                long address = 0;
                do
                {
                    MEMORY_BASIC_INFORMATION m;
                    int result = VirtualQueryEx(game, (IntPtr)address, out m, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION)));

                    GetMappedFileName(game, m.BaseAddress, fn, 250);
                    if (fn.ToString().IndexOf("agent.dll") > -1 || fn.ToString().IndexOf("tangerine.dll") > -1)
                    {
                        dllExist = true;
                        break;
                    }
                    if (address == (long)m.BaseAddress + (long)m.RegionSize)
                        break;
                    address = (long)m.BaseAddress + (long)m.RegionSize;
                } while (address <= MaxAddress);
            }
            catch (Exception)
            {
                return dllExist;
            }
            return dllExist;
        }

        public enum AllocationProtect : uint
        {
            PAGE_EXECUTE = 0x00000010,
            PAGE_EXECUTE_READ = 0x00000020,
            PAGE_EXECUTE_READWRITE = 0x00000040,
            PAGE_EXECUTE_WRITECOPY = 0x00000080,
            PAGE_NOACCESS = 0x00000001,
            PAGE_READONLY = 0x00000002,
            PAGE_READWRITE = 0x00000004,
            PAGE_WRITECOPY = 0x00000008,
            PAGE_GUARD = 0x00000100,
            PAGE_NOCACHE = 0x00000200,
            PAGE_WRITECOMBINE = 0x00000400
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr BaseAddress;
            public IntPtr AllocationBase;
            public uint AllocationProtect;
            public IntPtr RegionSize;
            public uint State;
            public uint Protect;
            public uint Type;
        }
    }
}
