using System;
using System.Threading;
using System.Windows;
using System.IO;
using System.Windows.Threading;

namespace HFL_Remastered
{

    public partial class App : Application
    {
        public static User Client;
        public static Main mainwindow;
        public Login loginWindow = new Login();
        public static string version = "2.4";
        public static GameMask gameContainer = new GameMask();
        
        private void DispatcherOnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs dispatcherUnhandledExceptionEventArgs)
        {
            MessageBox.Show("Wide dispatcher exception is sent to Law, program will continue");
            dispatcherUnhandledExceptionEventArgs.Handled = true;
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            MessageBox.Show("Domain exception is sent to Law, program will continue");
        }

        private void CurrentOnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs dispatcherUnhandledExceptionEventArgs)
        {
            MessageBox.Show("Current Dispatcher exception is sent to Law, program will continue");
            dispatcherUnhandledExceptionEventArgs.Handled = true;
        }

        private void SetLanguageDictionary()
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {
                case "en-US":
                    dict.Source = new Uri("..\\Resources\\StringResources.en-us.xaml", UriKind.Relative);
                    break;
                case "tr-TR":
                    dict.Source = new Uri("..\\Resources\\StringResources.tr-tr.xaml", UriKind.Relative);
                    break;
            }
            this.Resources.MergedDictionaries.Add(dict);
        }


        private async void igniter(object sender, StartupEventArgs e)
        {
            SetLanguageDictionary();
            Application.Current.DispatcherUnhandledException += CurrentOnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            Dispatcher.UnhandledException += DispatcherOnUnhandledException;

            if (File.Exists("HFL Remasteredold.exe"))
            {
                File.Delete("HFL Remasteredold.exe");
                Changelog ch = new Changelog();
                ch.Show();
            }
            bool updateExists = await Connection.updateCheck();
            if (updateExists)
            {
                Update updater = new Update();
                updater.Show();
            }
            else
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

}