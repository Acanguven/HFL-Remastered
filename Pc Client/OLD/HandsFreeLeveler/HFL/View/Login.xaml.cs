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

namespace HandsFreeLeveler
{
    /// <summary>
    /// Interaction logic for User.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Dictionary dic = new Dictionary();
        public bool cont = false;
        public Login()
        {
            InitializeComponent();
            login_button.Content = dic.text("loginButton");
            register_button.Content = dic.text("registerButton");
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void login_button_Click(object sender, RoutedEventArgs e)
        {
            string loginStatus = await Connection.login(username_box.Text, pass_box.Password, HWID.Generate());
            if (loginStatus == "true")
            {
                /*Bol bolUpdate = new Bol();*/
                cont = true;
                Dashboard home = new Dashboard();
                this.Close();
                /*bolUpdate.Show();*/
                home.Show();
            }
            else
            {
                MessageBox.Show(loginStatus);
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Connection.register();
        }

        private void onCLose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!cont) { 
                Application.Current.Shutdown();
            }
        }
    }
}
