using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace HFL_3._0
{

    [Serializable]
    public class Settings
    {
        public string theme { get; set; }
        public string language { get; set; }
        public bool cPacketSearch { get; set; }
        public bool buyBoost { get; set; }
        public bool reconnect { get; set; }
        public bool disableGpu { get; set; }
        public bool injection { get; set; }
        public double floatLeftPos { get; set; }
        public double floatTopPos { get; set; }
        public string bolFolder { get; set; }
        public string gameFolder { get; set; }

        public Settings()
        {

        }

        public void saveSettings()
        {
            if (File.Exists("settings.xml"))
            {
                Storage.SerializeObject(this, "settings.xml");
            }
        }

        public void loadSettings()
        {
            if (File.Exists("settings.xml"))
            {
                try
                {
                    var obj = Storage.DeSerializeObject<Settings>("settings.xml");
                    theme = obj.theme;
                    language = obj.language;
                    cPacketSearch = obj.cPacketSearch;
                    buyBoost = obj.buyBoost;
                    reconnect = obj.reconnect;
                    disableGpu = obj.disableGpu;
                    injection = obj.injection;
                    floatLeftPos = obj.floatLeftPos;
                    floatTopPos = obj.floatTopPos;
                    bolFolder = obj.bolFolder;
                    gameFolder = obj.gameFolder;
                }
                catch (Exception ex)
                {
                    theme = "simplex";
                    language = "English";
                    cPacketSearch = true;
                    buyBoost = false;
                    reconnect = true;
                    disableGpu = true;
                    injection = false;
                    floatLeftPos = 100;
                    floatTopPos = 100;
                    Storage.SerializeObject(this, "settings.xml");
                }
            }
            else
            {
                theme = "simplex";
                language = "English";
                cPacketSearch = true;
                buyBoost = false;
                reconnect = true;
                disableGpu = true;
                injection = false;
                floatLeftPos = 100;
                floatTopPos = 100;
                Storage.SerializeObject(this, "settings.xml");
            }
        }

        public bool checkFolderSettings()
        {
            while (gameFolder == null || !File.Exists(gameFolder))
            {
                MessageBox.Show("Please select lol.launcher.exe");
                var dlg = new OpenFileDialog();
                dlg.Filter = "lol.launcher" + "|" + "lol.launcher.exe";
                if (dlg.ShowDialog() == true)
                {
                    gameFolder = dlg.FileName;
                    saveSettings();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

    }
}
