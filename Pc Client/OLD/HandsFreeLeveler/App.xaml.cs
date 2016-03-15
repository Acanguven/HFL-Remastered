using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.Collections.Specialized;
using System.IO;
using System.Net;

namespace HandsFreeLeveler
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 


    public partial class App : Application
    {
        public static string version = "3.18";
        public static ObservableCollection<Smurf> smurfList = new ObservableCollection<Smurf>();
        public static GameMask gameContainer = new GameMask();


        private async void Application_Startup_2(object sender, StartupEventArgs e)
        {
            /*if (!File.Exists("HFL.pdb"))
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile("http://handsfreeleveler.com/HFL.pdb", "HFL.pdb");
                    System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    Environment.Exit(1);
                }
            }*/
            /*bool updateExists = await Connection.updateCheck();
            if (File.Exists("HFLOLD.exe"))
            {
                File.Delete("HFLOLD.exe");
                MessageBox.Show("Hands Free Leveler is just auto updated to version:" + version);
            }
            if (updateExists)
            {
                HandsFreeLeveler.HFL.View.Update updater = new HandsFreeLeveler.HFL.View.Update();
                updater.Show();
            }
            else
            {*/
                Settings.firstTime = true;
                Settings.update();
                Language selector = new Language();
                selector.Show();
            //}
        }

    }
}
