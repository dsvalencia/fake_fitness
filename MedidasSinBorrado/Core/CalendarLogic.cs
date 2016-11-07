using System;
namespace proyectoDia
{
	class Calendar : Gtk.Window
	{
		private Gtk.Label label;
		public Calendar() : base("Calendario")
		{
			SetDefaultSize(300, 270);
			SetPosition(Gtk.WindowPosition.Center);

			label = new Gtk.Label("...");

			Gtk.Calendar calendar = new Gtk.Calendar();
			calendar.DaySelected += OnDaySelected;

			Gtk.Fixed fix = new Gtk.Fixed();
			fix.Put(calendar, 20, 20);
			fix.Put(label, 40, 230);

			Add(fix);

			ShowAll();
		}

		void OnDaySelected(object sender, EventArgs args)
		{
			Gtk.Calendar cal = (Gtk.Calendar)sender;
			label.Text = cal.Month + 1 + "/" + cal.Day + "/" + cal.Year;
		}
	}
}

