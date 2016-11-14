using System;
using CalendarParte1.Core;


// System
// System.Xml
// System.Xml.Linq
// System.Data.Linq
// Pango-sharp
// Mono.CSharp
// gtk-sharp
// glib-sharp
// gdk-sharp
// atk-sharp


namespace CalendarParte1.Ui
{
	public class Calendario
	{
		class MainClass
		{
			
			public static void Main(string[] args)
			{

				Gtk.Application.Init();
				MainWindow win = new MainWindow();
				win.ShowAll();
				Gtk.Application.Run();


			}
		}
	}
}

