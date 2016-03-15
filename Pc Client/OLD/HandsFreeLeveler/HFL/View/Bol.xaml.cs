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
using System.Windows.Shapes;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;

namespace HandsFreeLeveler
{
    /// <summary>
    /// Interaction logic for Bol.xaml
    /// </summary>
    public partial class Bol : Window
    {
        public static Dictionary dic = new Dictionary();
        private static BackgroundWorker bgWorker = new BackgroundWorker();
        public Bol()
        {
            InitializeComponent();
            t1.Content = dic.text("t1");

            bgWorker.WorkerSupportsCancellation = false;
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bgWorker.WorkerReportsProgress = true;
            bgWorker.DoWork += bolDetector;
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bgWorker.RunWorkerAsync();
        }

        private static void bolDetector(object sender, DoWorkEventArgs e)
        {
            bgWorker.ReportProgress(0, 0);
            bool Bolfirstran = false;
            while (Bolfirstran == false)
            {
                Process[] pname = Process.GetProcessesByName("Bol Studio");
                if (pname.Length == 0)
                {
                    Bolfirstran = false;
                }
                else
                {
                    Bolfirstran = true;
                }
            }
            bgWorker.ReportProgress(0, 1);
            Thread.Sleep(3000);
            while (Bolfirstran)
            {
                Process[] pname = Process.GetProcessesByName("Bol Studio");
                if (pname.Length == 0)
                {
                    Bolfirstran = false;
                }
                else
                {
                    Bolfirstran = true;
                }
            }
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if ( e.UserState.ToString() == "0" ){
                t1.Foreground = new SolidColorBrush(Colors.Red);
                t2.Foreground = new SolidColorBrush(Colors.Red);
                t3.Foreground = new SolidColorBrush(Colors.Red);
                gif1.Visibility = System.Windows.Visibility.Visible;
            }

            if (e.UserState.ToString() == "1")
            {
                t1.Foreground = new SolidColorBrush(Colors.Green);
                t2.Foreground = new SolidColorBrush(Colors.Gray);
                t3.Foreground = new SolidColorBrush(Colors.Red);
                gif1.Visibility = System.Windows.Visibility.Hidden;
                gif2.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Dashboard home = new Dashboard();
            home.Show();
            this.Close();
        }
    }
}
