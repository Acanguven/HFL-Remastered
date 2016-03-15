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
    /// Interaction logic for addSmurfWindow.xaml
    /// </summary>
    public partial class AddSmurfWindow : Window
    {
        public AddSmurfWindow()
        {
            InitializeComponent();
            rgCombo.ItemsSource = Enum.GetValues(typeof(LoLLauncher.Region)).Cast<LoLLauncher.Region>();
            IList<string> generalLineWidthRange = new List<string>();
            for (int i = 1; i < 30; i++)
            {
                generalLineWidthRange.Add(i.ToString());
            }
            generalLineWidthRange.Add("Unlimited");
            maxLevels.ItemsSource = generalLineWidthRange;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (maxLevels.SelectedValue != null && rgCombo.SelectedValue != null)
            {
                LoLLauncher.Region regi = (LoLLauncher.Region)System.Enum.Parse(typeof(LoLLauncher.Region), rgCombo.SelectedValue.ToString());
                if (username.Text.Length > 0 && password.Text.Length > 0)
                {
                    bool smurfFound = App.smurfList.Any(smurf => (smurf.username == username.Text && smurf.region == regi));
                    if (!smurfFound)
                    {
                        Smurf newSmurf = new Smurf();
                        newSmurf.username = username.Text;
                        newSmurf.password = password.Text;
                        newSmurf.region = regi;

                        if (maxLevels.SelectedValue.ToString() == "Unlimited")
                        {
                            newSmurf.maxLevel = 31;
                        }
                        else
                        {
                            newSmurf.maxLevel = Int32.Parse(maxLevels.SelectedValue.ToString());
                        }
                        App.smurfList.Add(newSmurf);
                        Settings.update();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("This smurf already exists");
                    }
                }
                else
                {
                    MessageBox.Show("Please fill all inputs");
                }
            }
            else
            {
                MessageBox.Show("Please fill all inputs");
            }
        }
    }
}


