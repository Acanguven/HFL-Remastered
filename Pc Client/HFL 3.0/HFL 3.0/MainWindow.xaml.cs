using System;
using System.Linq;
using System.Windows;
using Awesomium.Core;
using Awesomium.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Input;
using System.Timers;
using System.Windows.Threading;

namespace HFL_3._0
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public static Timer _Timer = new Timer();
		public int _TimerStop = 0;
		public int _Height = 0;
		public int _Width = 0;
		public int _Left = 0;
		public int _Top = 0;
		public double _RatioHeight = 0;
		public double _RatioWidth = 0; 
		public double _RatioLeft = 0;
		public double _RatioTop = 0; 
		public MainWindow()
		{
			InitializeComponent();
			this.Source = WebCore.Configuration.HomeURL;
			this.webControl.Loaded += WebViewOnDocumentReady;
			WebControlService.SetShowDesignTimeLogo(this.webControl, false);

			_Timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
			_Timer.Interval = 5;
			_Timer.Enabled = false;
			this.webControl.ShowContextMenu += Html5_ShowContextMenu;
 

			
		}

		private void Html5_ShowContextMenu(object sender, Awesomium.Core.ContextMenuEventArgs e)
		{
			e.Handled = true;
		}

		private void WebViewOnDocumentReady(object sender, EventArgs eventArgs)
		{
			
			JSObject jsobject = webControl.CreateGlobalJavascriptObject("NET");

			/* Login Window */
			jsobject.Bind("isAuthenticated", isAuthenticated);
			jsobject.Bind("resize", Resizer);
			jsobject.Bind("login", Login);
			jsobject.Bind("openRegister", openRegister);
			
		}

		private JSValue Resizer(object sender, JavascriptMethodEventArgs e)
		{
			_Height = Int32.Parse(e.Arguments[1].ToString());
			_Width = Int32.Parse(e.Arguments[0].ToString());

			_TimerStop = 0;
			_Timer.Enabled = true;
			_Timer.Start();
			CenterWindowOnScreen(_Width, _Height);
			return JSValue.Undefined;
		}

		private void CenterWindowOnScreen(double _Width, double _Height)
		{
			double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
			double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
			double windowWidth = _Width;
			double windowHeight = _Height;
			_Left = (int) Math.Round((screenWidth / 2) - (windowWidth / 2));
			_Top = (int) Math.Round((screenHeight / 2) - (windowHeight / 2));
		}

		private JSValue Login(object sender, JavascriptMethodEventArgs e)
		{
			Connection.login(e.Arguments[0].ToString(), e.Arguments[1].ToString(), HWID.Generate());
			return JSValue.Undefined;
		}

		private JSValue isAuthenticated(object sender, JavascriptMethodEventArgs e)
		{
			return false;
		}

		private JSValue openRegister(object sender, JavascriptMethodEventArgs e)
		{
			MessageBox.Show("Register");
			return JSValue.Undefined;
		}
		
		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			// Destroy the WebControl and its underlying view.
			webControl.Dispose();
		}

		private void OnTimedEvent(Object myObject, EventArgs myEventArgs)
		{
			App.main.Dispatcher.Invoke(new Action(() =>
			{
				if (_TimerStop == 0)
				{
					_RatioHeight = ((this.Height - _Height) / 15) * -1;
					_RatioWidth = ((this.Width - _Width) / 15) * -1;
					_RatioLeft = ((this.Left - _Left) / 15) * -1;
					_RatioTop = ((this.Top - _Top) / 15) * -1;
				}
				_TimerStop++;

				this.Height += _RatioHeight;
				this.Width += _RatioWidth;
				this.Left += _RatioLeft;
				this.Top += _RatioTop;

				webControl.Height = this.Height;
				webControl.Width = this.Width;

				if (_TimerStop == 15)
				{
					_Timer.Stop();
					_Timer.Enabled = false;

					_TimerStop = 0;

					this.Height = _Height;
					this.Width = _Width;
					this.Left = _Left;
					this.Top = _Top;

					webControl.Height = this.Height;
					webControl.Width = this.Width;
				}
			}), DispatcherPriority.ContextIdle);
		}


		public Uri Source
		{
			get { return (Uri)GetValue(SourceProperty); }
			set { SetValue(SourceProperty, value); }
		}

		public static readonly DependencyProperty SourceProperty =
				DependencyProperty.Register("Source",
				typeof(Uri), typeof(MainWindow),
				new FrameworkPropertyMetadata(null));
	}
}
