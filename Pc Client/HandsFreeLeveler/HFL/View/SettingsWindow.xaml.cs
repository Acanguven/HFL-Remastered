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
using Microsoft.Win32;
using System.Windows.Shapes;
using System.IO;

namespace HandsFreeLeveler
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void setBol_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "BoL Studio.exe" + "|" + "BoL Studio.exe";
            if (dlg.ShowDialog() == true)
            {
                Settings.bolPath = dlg.FileName;
                Settings.dllPath = Settings.bolPath.Split(new string[] { "BoL Studio.exe" }, StringSplitOptions.None)[0] + "tangerine.dll";
                bolBox.Text = dlg.FileName;
                Settings.update();
            }

        }

        private void setGame_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "lol.launcher" + "|" + "lol.launcher.exe";
            if (dlg.ShowDialog() == true)
            {
                Settings.gamePath = dlg.FileName;

                gameBox.Text = dlg.FileName;
                Settings.update();
            }
        }

        private void restoreConfig_Click(object sender, RoutedEventArgs e)
        {
            Settings.restoreCfg();
        }

        private void Sleep_Checked(object sender, RoutedEventArgs e)
        {
            if (Sleep.IsChecked == true)
            {
                Settings.smurfBreak = true;
                Settings.update();
            }
            else
            {
                Settings.smurfBreak = false;
                Settings.update();
            }
        }

        private void bboost_Checked(object sender, RoutedEventArgs e)
        {
            if (bboost.IsChecked == true)
            {
                Settings.buyBoost = true;
                Settings.update();
            }
            else
            {
                Settings.buyBoost = false;
                Settings.update();
            }
        }

        private void dgpu_Checked(object sender, RoutedEventArgs e)
        {
            if (dgpu.IsChecked == true)
            {
                Settings.disableGpu = true;
                MessageBox.Show("When you enable GPU Disabling option you can't play normally on your computer even if you close HFL.exe, to play normally you have to uncheck this box again.");
                Settings.update();
                Settings.ReplaceGameConfig();
            }
            else
            {
                Settings.disableGpu = false;
                Settings.update();
                Settings.restoreCfg();
            }
        }

        private void rc_Checked(object sender, RoutedEventArgs e)
        {
            if (rc.IsChecked == true)
            {
                Settings.reconnect = true;
                Settings.update();
            }
            else
            {
                Settings.reconnect = false;
                Settings.update();
            }
        }

        private void minject_Checked(object sender, RoutedEventArgs e)
        {
            if (minject.IsChecked == true)
            {
                Settings.mInject = true;
                Settings.update();
            }
            else
            {
                Settings.mInject = false;
                Settings.update();
            }
        }

        private void dspec_Checked(object sender, RoutedEventArgs e)
        {
            if (dspec.IsChecked == true)
            {
                Settings.disableSpec = true;
                Settings.update();
            }
            else
            {
                Settings.disableSpec = false;
                Settings.update();
            }
        }

        private void SleepFor_TextChanged(object sender, TextChangedEventArgs e)
        {
            Settings.smurfSleep = Int32.Parse(SleepFor.Text);
            Settings.update();
        }

        private void SleepAfter_TextChanged(object sender, TextChangedEventArgs e)
        {
            Settings.smurfTimeoutAfter = Int32.Parse(SleepAfter.Text);
            Settings.update();
        }

        private void compability_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Title = "Please select the script you want to make compatible with HFL";
            dlg.Filter = "*.lua" + "|" + "*.lua";
            if (dlg.ShowDialog() == true)
            {
                if (File.Exists(dlg.FileName))
                {
                    try
                    {
                        string currentContent = File.ReadAllText(dlg.FileName);
                        if (currentContent.IndexOf("_G.OnDraw = function() end--HFL COMPABILITY\n_G.HFL_loaded = true --HFL COMPABILITY\n") > -1)
                        {
                            MessageBox.Show("This script is already compatible with Hands Free Leveler");
                        }
                        else
                        {
                            File.WriteAllText(dlg.FileName, "_G.OnDraw = function() end--HFL COMPABILITY\n_G.HFL_loaded = true --HFL COMPABILITY\n" + currentContent);
                            MessageBox.Show("Your script is now compatible with the Disable GPU option.");
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Be sure HFL has enough rights to change the selected file.");
                    }
                }
            }
        }
    }
}
