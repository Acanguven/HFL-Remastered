using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Awesomium.Core;

namespace HFL_3._0
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public static MainWindow main;
		public static User Client;
		public static string version = "2.9";
		protected override void OnStartup(StartupEventArgs e)
		{
			// Initialization must be performed here,
			// before creating a WebControl.
			if (!WebCore.IsInitialized)
			{
				WebCore.Initialize(new WebConfig()
				{
					HomeURL = new Uri("http://localhost:9000/Controller/", UriKind.Absolute),
					LogPath = @".\starter.log",
					LogLevel = LogLevel.Verbose
				});
			}
			main = new MainWindow();
			main.Show();

			base.OnStartup(e);
		}

		protected override void OnExit(ExitEventArgs e)
		{
			// Make sure we shutdown the core last.
			if (WebCore.IsInitialized)
				WebCore.Shutdown();

			base.OnExit(e);
		}
	}
}
