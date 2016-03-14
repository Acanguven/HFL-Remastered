using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Win32;
using WinForms = System.Windows.Forms;

namespace HFL_Remastered
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
       
        private System.Windows.Forms.NotifyIcon m_notifyIcon;
        public Network net = new Network();
        private double remainingTrial { get; set; }
        public string text { get; set; }
        public Main()
        {
            FileManager.checkPaths();
            InitializeComponent();
            updateHome();
            net.init(remainingTrial, App.Client.UserData.Type);
            uiversion.Text = "Version " + App.version;
            App.gameContainer.Show();
        }

        private WindowState m_storedWindowState = WindowState.Normal;

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
            this.WindowState = WindowState.Minimized;
            if (m_notifyIcon != null) { 
                m_notifyIcon.ShowBalloonTip(2000);
            }
        }

        void OnStateChanged(object sender, EventArgs args)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
                if (m_notifyIcon != null)
                    m_notifyIcon.ShowBalloonTip(2000);
            }
            else
            {
                m_storedWindowState = WindowState;
            }
        }
        void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            CheckTrayIcon();
        }

        void m_notifyIcon_Click(object sender, dynamic e)
        {
            if (e.Button == WinForms.MouseButtons.Right)
            {
                ContextMenu menu = (ContextMenu)this.FindResource("NotifierContextMenu");
                menu.DataContext = net;
                menu.IsOpen = true;
            }
            else
            {
                try { 
                    this.Show();
                    this.Activate();
                    WindowState = m_storedWindowState;
                    this.WindowState = WindowState.Normal;
                    this.Visibility = Visibility.Visible;
                    this.Focus();
                    ShowWindow(new WindowInteropHelper(this).Handle, SW_RESTORE);
                }
                catch (Exception ex)
                {

                }
            }
        }

        private const int SW_RESTORE = 9;
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private void Menu_Open(object sender, RoutedEventArgs e)
        {
            this.Show();
            this.Activate();
            WindowState = m_storedWindowState;
            this.WindowState = WindowState.Normal;
            this.Visibility = Visibility.Visible;
            this.Focus();
            ShowWindow(new WindowInteropHelper(this).Handle, SW_RESTORE);
        }

        private void Menu_Close(object sender, RoutedEventArgs e)
        {
            try {
                m_notifyIcon.Visible = false;
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {

            }
        }

        void CheckTrayIcon()
        {
            ShowTrayIcon(!IsVisible);
        }

        void ShowTrayIcon(bool show)
        {
            if (m_notifyIcon != null)
                m_notifyIcon.Visible = show;
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            OnIsVisibleChanged(sender, e);
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            OnStateChanged(sender, e);
        }

        public void updateHome()
        {
            if (Properties.Settings.Default.startup)
            {
                startUp.IsChecked = true;
            }
            if (Properties.Settings.Default.logging)
            {
                logging.IsChecked = true;
            }
            HandleCK(startUp);

            this.DataContext = net;
            this.Visibility = Visibility.Visible;
            this.trial.Visibility = Visibility.Hidden;
            m_notifyIcon = new System.Windows.Forms.NotifyIcon();
            m_notifyIcon.BalloonTipText = "HFL has been minimized. Click the tray icon to show.";
            m_notifyIcon.BalloonTipTitle = "Hands Free Leveler";
            m_notifyIcon.Text = "Hands Free Leveler";
            Stream iconStream = Application.GetResourceStream(new Uri("pack://application:,,,/Resources/favicon.ico")).Stream;
            m_notifyIcon.Icon = new System.Drawing.Icon(iconStream);
            m_notifyIcon.Click += new EventHandler(m_notifyIcon_Click);

            this.username.Text = "Username: " + App.Client.ForumData.Name.ToString();
            double remainingHwid = Math.Round((double)((App.Client.UserData.HwidCanChange - App.Client.Date) / 60000));
            if (remainingHwid > 0)
            {
                this.hwid.Text = "Hwid reset: " + "Available in " + remainingHwid.ToString() + " minutes.";
                this.hwid.Foreground = new SolidColorBrush(Colors.Orange);
            }
            else
            {
                this.hwid.Text = "Hwid reset: Ready to reset";
                this.hwid.Foreground = new SolidColorBrush(Colors.Green);
            }

            remainingTrial = Math.Round((double)((App.Client.UserData.Trial - App.Client.Date) / 60000));
            if (remainingTrial > 0)
            {
                this.trial.Text = "Trial: " + remainingTrial.ToString() + " minutes remain.";
                this.trial.Foreground = new SolidColorBrush(Colors.Orange);
            }
            else
            {
                this.trial.Text = "Trial: Expired";
                this.trial.Foreground = new SolidColorBrush(Colors.Red);
            }

            if (App.Client.UserData.Type == (int)0)
            {
                acc.Text = "Account Type: Trial";
                this.trial.Visibility = Visibility.Visible;
            }

            if (App.Client.UserData.Type == (int)1)
            {
                acc.Text = "Account Type: Single Smurf";
            }

            if (App.Client.UserData.Type == (int)2)
            {
                acc.Text = "Account Type: Multi Smurf";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://remote.handsfreeleveler.com/");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://forum.handsfreeleveler.com/");
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            HandleCK(sender as CheckBox);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            HandleCK(sender as CheckBox);
        }

        private void HandleCK(CheckBox checkBox)
        {
            bool flag = checkBox.IsChecked.Value;
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (flag){
                rk.SetValue(System.AppDomain.CurrentDomain.FriendlyName, System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
            else{
                rk.DeleteValue(System.AppDomain.CurrentDomain.FriendlyName, false);           
            }

            Properties.Settings.Default.startup = flag;
            Properties.Settings.Default.Save();
        }

        private void HandleLoggingCK(CheckBox checkBox)
        {
            bool flag = checkBox.IsChecked.Value;
            Properties.Settings.Default.logging = flag;
            Properties.Settings.Default.Save();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://forum.handsfreeleveler.com/topic/24/how-to-buy-payment-methods");
        }

        private void logging_Checked(object sender, RoutedEventArgs e)
        {
            HandleLoggingCK(sender as CheckBox);
        }

        private void logging_Unchecked(object sender, RoutedEventArgs e)
        {
            HandleLoggingCK(sender as CheckBox);
        }
    }
}
