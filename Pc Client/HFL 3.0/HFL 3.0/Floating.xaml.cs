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
		}

		private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.DragMove();
		}

		private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (Math.Abs(preLeft - this.Left) < 3 && Math.Abs(preTop - this.Top) < 3)
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
			preLeft = this.Left;
			preTop = this.Top;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			preLeft = this.Left;
			preTop = this.Top;
		}
	}
}
