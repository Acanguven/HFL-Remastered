using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bol_Code_Refresher
{
    class Program
    {
        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        static int registeredFiles = 0;

        [STAThread]
        static void Main(string[] args)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = true;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                foreach (String file in dlg.FileNames)
                {
                    CreateFileWatcher(file);
                    registeredFiles++;
                }
            }

            while (registeredFiles > 0) {
                Thread.Sleep(10);
            }
        }

        public static void CreateFileWatcher(string path)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = Path.GetDirectoryName(path);
            watcher.Filter = Path.GetFileName(path);
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
        }

        private async static void OnChanged(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
            await refreshBol();
        }

        private async static Task refreshBol(){
            Process p = Process.GetProcessesByName("League of Legends").FirstOrDefault();
            if( p != null)
            {
                IntPtr h = p.MainWindowHandle;
                SetForegroundWindow(h);
                while (GetForegroundWindow() != h){}
                SendKeys.SendWait("{F9}");
                Thread.Sleep(1000);
            }
        }

    }
}
