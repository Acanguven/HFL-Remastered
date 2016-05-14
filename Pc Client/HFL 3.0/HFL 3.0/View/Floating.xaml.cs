using Awesomium.Core;
using System;
using System.Windows;
using System.Windows.Input;

namespace HFL_3._0
{
    /// <summary>
    /// Interaction logic for Floating.xaml
    /// </summary>
    public partial class Floating : Window
	{
		public double preLeft { get; set; }
		public double preTop { get; set; }
		public bool mainVisible = true;
		

		public Floating()
		{
			InitializeComponent();
            App.NET.Bind("updateNotificationCount", updateNotificationCount);
        }

        public JSValue updateNotificationCount(object sender, JavascriptMethodEventArgs e)
        {
            notificationCount.Text = e.Arguments[0].ToString();
            return JSValue.Undefined;
        }

        public void updateNotificationCount(int num)
        {
            notificationCount.Text = num.ToString();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
            DragMove();
		}

		private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (Math.Abs(preLeft - Left) < 3 && Math.Abs(preTop - Top) < 3)
			{
				if (!mainVisible) { 
					App.main.Show();
					mainVisible = true;
                }
				else
				{
					App.main.Hide();
					mainVisible = false;
				}
			}
			preLeft = Left;
			preTop = Top;
            App.settings.floatLeftPos = Left;
            App.settings.floatTopPos = Top;
            App.settings.saveSettings();
        }

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
            Left = App.settings.floatLeftPos;
            Top = App.settings.floatTopPos;
            preLeft = Left;
			preTop = Top;
		}
	}
}
