using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace HFL_Remastered
{
    /// <summary>
    /// Interaction logic for User.xaml
    /// </summary>
    public partial class Login : Window
    {
        public bool cont = false;
        public  Login()
        {
            InitializeComponent();
            Localization.SetLanguageDictionary(this);
        }

        public async Task<bool> storageLogin()
        {
            if (Properties.Settings.Default.username != "" && Properties.Settings.Default.password != "")
            {
                bool loginStatus = await Connection.login(Properties.Settings.Default.username, Properties.Settings.Default.password, HWID.Generate());
                if (loginStatus)
                {
                    return true;
                }
                else
                {
                    Properties.Settings.Default.username = "";
                    Properties.Settings.Default.password = "";
                    Properties.Settings.Default.Save();
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private async void login_button_Click(object sender, RoutedEventArgs e)
        {
            bool loginStatus = await Connection.login(username_box.Text, pass_box.Password, HWID.Generate());
            
            if (loginStatus)
            {
                Properties.Settings.Default.username = username_box.Text;
                Properties.Settings.Default.password = pass_box.Password;
                Properties.Settings.Default.Save();

                App.mainwindow = new Main();
                App.mainwindow.Show();
                this.cont = true;
                this.Close();
            }
        }

        private void onCLose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!cont)
            {
                Process.GetCurrentProcess().Kill();
            }
        }


        private void openForum(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://forum.handsfreeleveler.com/");
        }
    }
}
