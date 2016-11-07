using System;

namespace proyectoDia
{
	public class MainClass
	{
		public static void Main(string[] args)
		{
			Gtk.Application.Init();
			var mainWindow = new MainWindow();
			mainWindow.ShowAll();
			Gtk.Application.Run();
		}
	}
}
