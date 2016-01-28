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
            if (!File.Exists("HFL.pdb"))
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile("http://handsfreeleveler.com/HFL.pdb", "HFL.pdb");
                    System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    Environment.Exit(1);
                }
            }
            bool updateExists = await Connection.updateCheck();
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
            {
                if (FileHandler.settingsExists())
                {
                    if (File.Exists("Smurfs.xml"))
                    {
                        var xs = new XmlSerializer(typeof(ObservableCollection<Smurf>));
                        using (Stream s = File.OpenRead("Smurfs.xml"))
                        {
                            smurfList = (ObservableCollection<Smurf>)xs.Deserialize(s);
                        }
                        smurfList.Select(c => { c.Logs = 1; c.expCalc = 0; return c; }).ToList();
                    }
                    XDocument settings = XDocument.Load("settings.xml");
                    Settings.firstTime = false;
                    var errorSettings = false;
                    try
                    {
                        //Settings.language = settings.Element("HFL").Element("Settings").Element("language").Value.ToString();
                        Settings.bolPath = settings.Element("HFL").Element("Paths").Element("BolPath").Value.ToString();
                        Settings.dllPath = Settings.bolPath.Split(new string[] { "BoL Studio.exe" }, StringSplitOptions.None)[0] + "tangerine.dll";
                        Settings.gamePath = settings.Element("HFL").Element("Paths").Element("GamePath").Value.ToString();
                        Settings.buyBoost = Boolean.Parse(settings.Element("HFL").Element("Settings").Element("BuyBoost").Value);
                        Settings.disableGpu = Boolean.Parse(settings.Element("HFL").Element("Settings").Element("DisableGpu").Value);
                        if (Settings.disableGpu)
                        {
                            Settings.ReplaceGameConfig();
                        }
                        Settings.smurfBreak = Boolean.Parse(settings.Element("HFL").Element("Settings").Element("smurfBreak").Value);
                        Settings.smurfSleep = Int32.Parse(settings.Element("HFL").Element("Settings").Element("smurfSleep").Value.ToString());
                        Settings.smurfTimeoutAfter = Int32.Parse(settings.Element("HFL").Element("Settings").Element("smurfTimeoutAfter").Value.ToString());
                        Settings.reconnect = Boolean.Parse(settings.Element("HFL").Element("Settings").Element("reconnect").Value);
                        Settings.mInject = Boolean.Parse(settings.Element("HFL").Element("Settings").Element("mInject").Value);
                        Settings.disableSpec = Boolean.Parse(settings.Element("HFL").Element("Settings").Element("disableSpec").Value);
                        string username = settings.Element("HFL").Element("Account").Element("Username").Value.ToString();
                        string password = settings.Element("HFL").Element("Account").Element("Password").Value.ToString();
                        User.username = username;
                        User.password = password;

                        Settings.update();
                    }
                    catch (Exception)
                    {
                        errorSettings = true;
                    }

                    if (!errorSettings)
                    {
                        string loginStatus = await Connection.login(User.username, User.password, HWID.Generate());

                        if (loginStatus == "true")
                        {
                            /*Bol bolde = new Bol();
                            bolde.Show();*/
                            Dashboard home = new Dashboard();
                            home.Show();
                        }
                        else
                        {
                            Login loginWindow = new Login();
                            loginWindow.Show();
                        }
                    }
                    else
                    {
                        File.Delete("settings.xml");
                        if (File.Exists("Smurfs.xml"))
                        {
                            File.Delete("Smurfs.xml");
                        }
                        Settings.firstTime = true;
                        Settings.update();
                        Language selector = new Language();
                        selector.Show();
                    }
                }
                else
                {
                    Settings.firstTime = true;
                    Settings.update();
                    Language selector = new Language();
                    selector.Show();
                }
            }
        }

    }
}
