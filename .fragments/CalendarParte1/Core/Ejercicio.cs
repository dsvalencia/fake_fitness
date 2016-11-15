using System;

namespace CalendarParte1.Core
{
	public class Ejercicio
	{
		public Ejercicio (DateTime fecha, string ejercicio)
		{
			this.fecha = fecha;
			this.ejercicio = ejercicio;

		}

		public DateTime fecha {
			get;	private set;
		}
		public string ejercicio {
			get; private set;
		}

		public override string ToString(){

			return string.Format ("{0}: {1}", this.fecha, this.ejercicio);


		}
	}
}

