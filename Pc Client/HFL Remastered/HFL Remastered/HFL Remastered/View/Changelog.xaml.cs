using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace HFL_Remastered
{
    /// <summary>
    /// Interaction logic for Changelog.xaml
    /// </summary>
    public partial class Changelog : Window
    {
        public Changelog()
        {
            InitializeComponent();
            getLogs();
        }

        public async void getLogs()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://remote.handsfreeleveler.com/");
            client.DefaultRequestHeaders.Accept.Clear();

            // HTTP GET
            try
            {
                HttpResponseMessage response = await client.GetAsync("changelog.txt");
                if (response.IsSuccessStatusCode)
                {
                    String data = await response.Content.ReadAsStringAsync();
                    logText.Text = data;
                }
                else
                {
                    logText.Text = "Failed to load change logs";
                }
            }
            catch (Exception)
            {
                logText.Text = "Failed to load change logs";
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            return;
        }
    }
}
