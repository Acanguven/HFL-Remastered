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

namespace HFL_Remastered
{

    public partial class App : Application
    {
        public static User Client;
        public static Main mainwindow;
        public Login loginWindow = new Login();

        private async void igniter(object sender, StartupEventArgs e)
        {
            if (!await loginWindow.storageLogin())
            {
                loginWindow.Show();
            }
            else
            {
                mainwindow = new Main();
                mainwindow.Show();
                loginWindow.cont = true;
                loginWindow.Close();
            }
        }
    }

}