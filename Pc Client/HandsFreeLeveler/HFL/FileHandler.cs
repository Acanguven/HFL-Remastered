using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Windows;

namespace HandsFreeLeveler
{
    public static class FileHandler
    {
        public static bool settingsExists()
        {
            if (File.Exists("settings.xml"))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public static void updateSettings(XDocument settings)
        {
            try
            {
                settings.Save("settings.xml");
            }
            catch (Exception exception)
            {
                FileHandler.traceReporter(exception.Message);
                MessageBox.Show("An error occured saving your settings file, please send the error.txt file to law");
            }
            return;
        }

        public static void traceReporter(string data)
        {
            string filePath = "Error.txt";
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("Message :" + data);
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
            }
        }

        public static XDocument loadSettings()
        {
            try
            {
                XDocument settings = XDocument.Load("settings.xml");
                return settings;
            }
            catch (Exception exception)
            {
                XDocument settings = new XDocument();
                return settings;
            }
        }
    }
}

