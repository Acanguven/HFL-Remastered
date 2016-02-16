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
using System.Net;
using System.Threading;
using System.ComponentModel;
using System.IO;

namespace HFL_Remastered
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Update : Window
    {
        public Update()
        {
            InitializeComponent();
            startDownloadUpdate();
        }

        public void startDownloadUpdate(){
            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed0);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            webClient.DownloadFileAsync(new Uri("http://remote.handsfreeleveler.com/HFL%20Remastered.exe"), "HFL Remasterednew.exe");
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            downloadProgress.Value = e.ProgressPercentage;
        }

        private void Completed0(object sender, AsyncCompletedEventArgs e)
        {
            File.Delete("HFL Remastered.pdb");
            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed1);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            webClient.DownloadFileAsync(new Uri("http://remote.handsfreeleveler.com/HFL%20Remastered.pdb"), "HFL Remastered.pdb");
        }

        private void Completed1(object sender, AsyncCompletedEventArgs e)
        {
            if (File.Exists("HFL Remasterednew.exe"))
            {
                File.Move("HFL Remastered.exe", "HFL Remasteredold.exe");
                File.Move("HFL Remasterednew.exe", "HFL Remastered.exe");
            }

            System.Diagnostics.Process.Start("HFL Remastered.exe");
            System.Environment.Exit(1);
        }
    }
}
