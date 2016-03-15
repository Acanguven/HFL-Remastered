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
using System.Diagnostics;

namespace HandsFreeLeveler
{
    /// <summary>
    /// Interaction logic for Account.xaml
    /// </summary>
    public partial class Account : Window
    {
        public Account()
        {
            InitializeComponent();
            username.Content = "Username: " + User.username;
            password.Content = "Password: " + User.password;
            Trial.Content = User.trialRemains;
            multiButton.Visibility = System.Windows.Visibility.Hidden;
            singleButton.Visibility = System.Windows.Visibility.Hidden;
            upButton.Visibility = System.Windows.Visibility.Hidden;

            if (User.multiSmurf)
            {
                AccountType.Content = "Account Type: Multi Smurf";
            }
            else
            {
                AccountType.Content = "Account Type: Single Smurf";
                if (!User.trial)
                {
                    upButton.Visibility = System.Windows.Visibility.Visible;
                }
            }

            if (User.trial)
            {
                singleButton.Visibility = System.Windows.Visibility.Visible;
                multiButton.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void multiButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_xclick&business=xcfzxb@gmail.com&item_name=Hands+Free+Leveler+(Multi+Smurf)&amount=30&currency_code=USD");
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong! Rest assured the cleanup monkeys have been sent!", "Woops!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void singleButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_xclick&business=xcfzxb@gmail.com&item_name=Hands+Free+Leveler+(Single+Smurf)&amount=20&currency_code=USD");
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong! Rest assured the cleanup monkeys have been sent!", "Woops!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void upButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_xclick&business=xcfzxb@gmail.com&item_name=Hands+Free+Leveler+(Upgrade+to+Multi)&amount=10&currency_code=USD");
            }
            catch (Exception)
            {
                MessageBox.Show("Something went wrong! Rest assured the cleanup monkeys have been sent!", "Woops!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
