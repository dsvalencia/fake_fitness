using System;
using CalendarParte1.Core;

namespace CalendarParte1.Ui
{
	public partial class MainWindow {

		//Array de datos
		protected Ejercicio [] ejtotal;


		//Text EjercicioDia
		protected Gtk.Label EjercicioDia;

		//PARA INSERTAR LOS DATOS
		private void Datos(){

			//Valores

			//ejercicios
			var ej1 = new Ejercicio (new DateTime (2016, 10, 3), "Subir escaleras");
			var ej2 = new Ejercicio (new DateTime (2016, 10, 20), "Saltar caja");
			var ej3 = new Ejercicio (new DateTime (2016, 10, 24), "Saltar banca");
			var ej4 = new Ejercicio (new DateTime (2016, 10, 29), "Solo correr");
			var ej5 = new Ejercicio (new DateTime (2016, 11, 4), "Saltar cuerda");
			var ej6 = new Ejercicio (new DateTime (2016, 11, 16), "Saltar correr");
			var ej7 = new Ejercicio (new DateTime (2016, 11, 22), "Solo correr");
			var ej8 = new Ejercicio (new DateTime (2016, 9, 8), "Saltar banca");
			var ej9 = new Ejercicio (new DateTime (2016, 9, 11), "Saltar cuerda");
			var ej10 = new Ejercicio (new DateTime (2016, 9, 23), "Solo correr");
			var ej11 = new Ejercicio (new DateTime (2015, 10, 7), "Subir escaleras");
			var ej12 = new Ejercicio (new DateTime (2015, 10, 12), "Saltar caja");
			var ej13 = new Ejercicio (new DateTime (2015, 10, 30), "Saltar banca");
			var ej14 = new Ejercicio (new DateTime (2015, 10, 2), "Solo correr");
			var ej15 = new Ejercicio (new DateTime (2015, 11, 6), "Saltar banca");
			var ej16 = new Ejercicio (new DateTime (2015, 11, 8), "Saltar cuerda");


			this.ejtotal = new Ejercicio[] {ej1,ej2,ej3,ej4,ej5,ej6,ej7,ej8,ej9,ej10,ej11,ej12,ej13,ej14,ej15,ej16};

		}

		//PARA MOSTRAR EL EJERCICIO AL CLICAR EN EL DIA
		private void OnDaySelected(object sender, EventArgs args)
		{
			Gtk.Calendar MiCalendario = (Gtk.Calendar) sender;

			Boolean esDiaEjercicio = false;

			foreach(Ejercicio i in ejtotal){

				if (MiCalendario.Month + 1 == i.fecha.Month && MiCalendario.Year == i.fecha.Year && MiCalendario.Day == i.fecha.Day) {

					esDiaEjercicio = true;
					this.EjercicioDia.Text = "<b>-></b> " + i.ejercicio;
					this.EjercicioDia.UseMarkup = true;

				}

			}
			if (esDiaEjercicio == false) {

				this.EjercicioDia.Text = "";
			}

		}

		//AL PASAR MES LIMPIAR EL CALENDARIO Y PONER EL DEL MES SIGUIENTE
		private void OnMonthChanged (object sender, EventArgs args){

			Gtk.Calendar MiCalendario = (Gtk.Calendar) sender;

			//for each borrando todo
			for (uint i=0 ; i<=31 ;i++) {
				MiCalendario.UnmarkDay (i);

			}

			foreach(Ejercicio i in ejtotal){

				if (MiCalendario.Month+1 == i.fecha.Month && MiCalendario.Year == i.fecha.Year) {

					MiCalendario.MarkDay ((uint)i.fecha.Day);
					}
					
			}


		}

		//MARCAR DIAS DEL MES INICIAL CON EJERCICIO Y SI COINCIDE EL DIA PONER EL EJERCICIO QUE SE HACE
		private void MarcarMes(Gtk.Calendar MiCalendario){
			
			foreach(Ejercicio i in ejtotal){

				if (MiCalendario.Month+1 == i.fecha.Month && MiCalendario.Year == i.fecha.Year) {

					MiCalendario.MarkDay ((uint)i.fecha.Day);

					if (MiCalendario.Day == i.fecha.Day) {
						this.EjercicioDia.Text = "<b>-></b> " + i.ejercicio ;
						this.EjercicioDia.UseMarkup = true;
					}
				}

			}
		}

		private void OnClose() {
			Gtk.Application.Quit ();
		}

	}

}
