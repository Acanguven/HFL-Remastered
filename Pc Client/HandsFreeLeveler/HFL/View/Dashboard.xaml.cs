using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Diagnostics;
using System.Threading;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Threading;

namespace HandsFreeLeveler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        private static BackgroundWorker bgWorker = new BackgroundWorker();
        public Dashboard()
        {
            InitializeComponent();
            smurfListDashBoard.ItemsSource = App.smurfList;
            
            bgWorker.WorkerSupportsCancellation = false;
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bgWorker.WorkerReportsProgress = true;
            bgWorker.DoWork += dashworker;
            bgWorker.RunWorkerAsync();

            Settings.EventActions += updateLanguage;

            queueTypeButton.Header = "Queue Type: " + Settings.queueType;
            
            this.Activate();
            this.Focus();

            UsernameLabel.Content = "Username: " + User.username;

            if(User.multiSmurf){
                PType.Content = "Package Type: Multi Smurf";
            }else{
                PType.Content = "Package Type: Single Smurf";
            }
            
            TrLabel.Content = User.trialRemains;
            smurfListDashBoard.SelectionChanged += (obj, e) => Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() => smurfListDashBoard.UnselectAll()));

            App.gameContainer.Show();
            App.gameContainer.Visibility = Visibility.Hidden;

            this.Title = this.Title + " " + App.version;
        }

        private void Account_Button_Click(object sender, RoutedEventArgs e)
        {
            Account accWindow = new Account();
            accWindow.Show();
        }

        private void smurfListButton_Click(object sender, RoutedEventArgs e)
        {
            SmurfListWindow smurfList = new SmurfListWindow();
            smurfList.Show();
        }

        private static void dashworker(object sender, DoWorkEventArgs e)
        {
            bool bolPre = true;
            while (true) { 
                Process[] pname = Process.GetProcessesByName("Bol Studio");
                Report report = new Report();

                report.smurfCount = App.smurfList.Count.ToString();
                if (pname.Length == 0)
                {
                    if (bolPre) { 
                        report.bol = false;
                        bgWorker.ReportProgress(0, report);
                        bolPre = false;
                    }
                }
                else
                {
                    if (!bolPre) { 
                        report.bol = true;
                        bgWorker.ReportProgress(0, report);
                        bolPre = true;
                    }
                }
                Thread.Sleep(200);
            }

        }

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            dynamic report = e.UserState;
            if(report.bol){
                BolStatus.Content = "Running";
                BolStatus.Foreground = new SolidColorBrush(Colors.Red);
                MessageBox.Show("Please close Bol Studio!");
            }else{
                BolStatus.Content = "Not Running";
                BolStatus.Foreground = new SolidColorBrush(Colors.Green);
            }
            NumOfSums.Content = "Number Of Smurfs: " + report.smurfCount;
        }

        private class Report{
            public bool bol;
            public string smurfCount;
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow sett = new SettingsWindow();
            sett.Show();
        }

        private void languageButton_Click(object sender, RoutedEventArgs e)
        {
            Language lang = new Language(false);
            lang.Show();
        }

        public void updateLanguage()
        {
            
        }

        private void start_stop_Button(object sender, RoutedEventArgs e)
        {
            FrameworkElement ownerGui = ((FrameworkElement)sender);
            Smurf obj = ownerGui.DataContext as Smurf;
            int index = App.smurfList.IndexOf(obj);
            if (User.multiSmurf || index == 0) { 
                if (obj.thread == null)
                {
                    obj.button = ((Button)sender);
                    obj.start();
                    ((Button)sender).Content = "Stop";
                }
                else
                {
                    obj.button = ((Button)sender);
                    obj.stop();
                    ((Button)sender).Content = "Start";
                }
            }
            else
            {
                MessageBox.Show("You can only start the first smurf you added, to run multiple smurfs you can upgrade your account.");
                Account toUpgrade = new Account();
                toUpgrade.Show();
            }
        }

        private void queueTypeButton_clicked(object sender, RoutedEventArgs e)
        {
            Settings.queueType = Enum.GetValues(typeof(LoLLauncher.QueueTypes)).Cast<LoLLauncher.QueueTypes>().Concat(new[] { default(LoLLauncher.QueueTypes) }).SkipWhile(_ => _ != Settings.queueType).Skip(1).First();
            if (Settings.queueType == 0)
            {
                Settings.queueType = LoLLauncher.QueueTypes.NORMAL_5x5;
            }
            queueTypeButton.Header = "Queue Type: " + Settings.queueType;
            Settings.update();
        }

        private void log_Button(object sender, RoutedEventArgs e)
        {
            FrameworkElement ownerGui = ((FrameworkElement)sender);
            Smurf obj = ownerGui.DataContext as Smurf;

            LogWindow logger = new LogWindow(obj);
            logger.Show();
        }

        private void gmWindows_click(object sender, RoutedEventArgs e)
        {
            App.gameContainer.Visibility = Visibility.Visible;
        }

        private void window_terminate(object sender, CancelEventArgs e)
        {
            App.gameContainer.killAll();
            Application.Current.Shutdown();
        }

        private void aiscript_Click(object sender, RoutedEventArgs e)
        {
            ScriptStatus stats = new ScriptStatus();
            stats.Show();
        }
    }
}
