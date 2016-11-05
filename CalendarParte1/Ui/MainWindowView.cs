using System;
using CalendarParte1.Core;

namespace CalendarParte1.Ui
{
	public partial class MainWindow: Gtk.Window {


		public MainWindow()
			:base( Gtk.WindowType.Toplevel )
		{
			//Datos a insertar
			Datos ();


			this.Build();
		}

		private void Build() {
			var vBox = new Gtk.VBox( false, 5 );



			var Titulo = new Gtk.Label( "<b>CALENDARIO</b>" );
			Titulo.UseMarkup = true;
			var MiCalendario = new Gtk.Calendar();
			//MiCalendario.DragDrop ();
			//boRRAR DRAG 
			var H1 = new Gtk.Label( "<b>EJERCICIO</b>" );
			H1.UseMarkup = true;
			EjercicioDia = new Gtk.Label();
			EjercicioDia.SetAlignment (0, 0);
			MarcarMes (MiCalendario);

			MiCalendario.DaySelected += OnDaySelected;
			MiCalendario.MonthChanged += OnMonthChanged;

			vBox.PackStart( Titulo, true, false, 10 );
			vBox.PackStart( MiCalendario, true, true, 10 );
			vBox.PackStart (H1,true,false, 5);
			vBox.PackStart (EjercicioDia, true, true, 5);

			this.Add( vBox );

		}

			
	}
}

