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

namespace HandsFreeLeveler.HFL.View
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
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            webClient.DownloadFileAsync(new Uri("http://handsfreeleveler.com/HFL.exe"), "HFLnew.exe");
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            downloadProgress.Value = e.ProgressPercentage;
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            if (File.Exists("HFLnew.exe"))
            {
                System.IO.File.Move("HFL.exe", "HFLOLD.exe");
                System.IO.File.Move("HFLnew.exe", "HFL.exe");
            }

            System.Diagnostics.Process.Start("HFL.exe");
            System.Environment.Exit(1);
        }
    }
}
