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
using System.ComponentModel;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;

namespace HandsFreeLeveler
{
    /// <summary>
    /// Interaction logic for SmurfListWindow.xaml
    /// </summary>
    /// 
    public partial class SmurfListWindow : Window
    {

        private static BackgroundWorker bgWorker = new BackgroundWorker();
        public SmurfListWindow()
        {
            InitializeComponent();
            smurfLister.ItemsSource = App.smurfList;
            if (bgWorker.IsBusy)
            {
                bgWorker.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            }

            if (bgWorker.IsBusy == false)
            {
                bgWorker.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
                bgWorker.WorkerReportsProgress = true;
                bgWorker.DoWork += worker;
                bgWorker.RunWorkerAsync();
            }
        }

        private void addNewSmurfButton_Click(object sender, RoutedEventArgs e)
        {
            AddSmurfWindow addNew = new AddSmurfWindow();
            addNew.Show();
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            var itemsToRemove = App.smurfList.Where(smu => (smu.thread == null)).ToList();

            foreach (var itemToRemove in itemsToRemove)
            {
                App.smurfList.Remove(itemToRemove);
            }

            Settings.update();
        }

        private string smurfCount()
        {
            return "Calculated Ram Usage: " + (App.smurfList.Count * 630).ToString();
        }



        void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete) { 
                DataGrid dg = sender as DataGrid;
                if (dg != null)
                {
                    dynamic curSmurf = dg.CurrentItem;
                    if (curSmurf.thread != null)
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        App.smurfList.Remove(curSmurf);
                        Settings.update();
                    }
                }
            }
        }

        private static void worker(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                windowReport report = new windowReport();
                if (App.smurfList.Count > 0)
                {
                    report.host = "Host Smurf:" + App.smurfList.First().username;
                }
                else
                {
                    report.host = "Host Smurf:";
                }
                report.totalSmurfs = "Total Smurfs:" + App.smurfList.Count;
                report.ramUsage = "Total Ram Usage:" + (App.smurfList.Count * 630).ToString() + " mb";
                report.runnigs = "Running Smurfs:" + App.smurfList.Select(smurf => (smurf.thread != null)).Count().ToString();

                bgWorker.ReportProgress(0, report);
                Thread.Sleep(100);
            }
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (this.totalHostCount != null)
            {
                dynamic report = e.UserState;
                this.totalSmurfCount.Content = report.totalSmurfs;
                this.totalHostCount.Content = report.host;
                this.totalRamCount.Content = report.ramUsage;
                this.totalRunningCount.Content = report.runnigs;
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        public class windowReport
        {
            public string totalSmurfs;
            public string ramUsage;
            public string host;
            public string regions;
            public string runnigs;
        }

        private void rowEditedEvent(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataGrid dg = sender as DataGrid;

            if (dg != null)
            {
                dynamic curSmurf = dg.CurrentItem;
                if (curSmurf.thread != null)
                {
                    e.Cancel = true;
                }

                if (!e.Row.IsEditing)
                {
                    this.smurfLister.Items.Refresh();
                }
            }
        }
    }
}
