using System;
using System.Windows;
using Awesomium.Core;
using Microsoft.WindowsAPICodePack.ApplicationServices;
using Microsoft.Win32;

namespace HFL_3._0
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Settings settings = new Settings();
        public static MainWindow main;
        public static User Client;
        public static string version = "2.9";
        public SocketManager socketManager = new SocketManager();
        public LiveManager liveManager = new LiveManager();
        public static WindowMask gameMask;
        public static bool recovery = false;

        public static JSObject NET;

        protected override void OnStartup(StartupEventArgs e)
        {
            //Start init after update
            settings.loadSettings();
            SystemEvents.PowerModeChanged += OnPowerChange;
            RegisterARR();
            if (!WebCore.IsInitialized)
            {
                WebCore.Initialize(new WebConfig()
                {
                    HomeURL = new Uri("http://remote.handsfreeleveler.com/controlur", UriKind.Absolute),
                    LogLevel = LogLevel.None,
                });
            }
            main = new MainWindow();
            main.Show();
            gameMask = new WindowMask();
            base.OnStartup(e);
        }

        private void RegisterARR()
        {
            ApplicationRestartRecoveryManager.RegisterForApplicationRestart(new RestartSettings(string.Empty, RestartRestrictions.None));
            ApplicationRestartRecoveryManager.RegisterForApplicationRecovery(new RecoverySettings(new RecoveryData(PerformRecovery, null), 5000));
        }

        private int PerformRecovery(object parameter)
        {
            try
            {
                ApplicationRestartRecoveryManager.ApplicationRecoveryInProgress();
                if (System.IO.Directory.Exists("data") && System.IO.File.Exists("data/session.xml"))
                {
                    recovery = true;
                    new System.Threading.Thread(() =>
                    {
                        MessageBox.Show("Hands Free Leveler might be crashed during a sesion\nRecovering from last rotation\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "Recovery Restart", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }).Start();
                }
                
                ApplicationRestartRecoveryManager.ApplicationRecoveryFinished(true);
            }
            catch
            {
                ApplicationRestartRecoveryManager.ApplicationRecoveryFinished(false);
            }
            return 0;
        }

        private void UnregisterApplicationRecoveryAndRestart()
        {
            ApplicationRestartRecoveryManager.UnregisterApplicationRestart();
            ApplicationRestartRecoveryManager.UnregisterApplicationRecovery();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Make sure we shutdown the core last.
            if (WebCore.IsInitialized)
                WebCore.Shutdown();

            base.OnExit(e);
        }


        private void OnPowerChange(object s, PowerModeChangedEventArgs e)
        {
            switch (e.Mode)
            {
                case PowerModes.Resume:
                    Console.WriteLine("System Waken up");
                    break;
                case PowerModes.Suspend:
                    Console.WriteLine("System is going to sleep");
                    break;
            }
        }
    }
}
