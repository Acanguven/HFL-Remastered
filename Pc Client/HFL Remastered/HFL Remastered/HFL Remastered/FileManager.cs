using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HFL_Remastered
{
    public static class FileManager
    {
        public static bool checkPaths()
        {
            if (!checkBol()) setBol();
            if (!checkLol()) setLol();
            return checkBol() && checkLol();
        }

        public static bool checkBol()
        {
            if (Properties.Settings.Default.bolPath != "")
            {
                if (File.Exists(Properties.Settings.Default.bolPath))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool checkLol()
        {
            if (Properties.Settings.Default.lolPath != "")
            {
                if (File.Exists(Properties.Settings.Default.lolPath))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static void setBol()
        {
            bool fileSet = false;
            while (!fileSet)
            {
                MessageBox.Show("Please select Bol Studio.exe");
                var dlg = new OpenFileDialog();
                dlg.Filter = "BoL Studio.exe" + "|" + "BoL Studio.exe";
                if (dlg.ShowDialog() == true)
                {
                    if (File.Exists(dlg.FileName))
                    {
                        fileSet = true;
                        Properties.Settings.Default.bolPath = dlg.FileName;
                        Properties.Settings.Default.Save();
                    }
                }
            }
        }

        public static void setLol()
        {
            bool fileSet = false;
            while (!fileSet)
            {
                MessageBox.Show("Please select lol.launcher.exe");
                var dlg = new OpenFileDialog();
                dlg.Filter = "lol.launcher" + "|" + "lol.launcher.exe";
                if (dlg.ShowDialog() == true)
                {
                    if (File.Exists(dlg.FileName))
                    {
                        fileSet = true;
                        Properties.Settings.Default.lolPath = dlg.FileName;
                        Properties.Settings.Default.Save();
                    }
                }
            }
        }
    }
}
