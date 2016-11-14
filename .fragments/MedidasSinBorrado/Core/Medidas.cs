using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace proyectoDia
{
	public class Medida
	{
		public Medida(short peso, short cadera)
		{
			this.Peso = peso;
			this.Cadera = cadera;
			this.Fecha = DateTime.Now;
		}

		public Medida(short peso, short cadera, DateTime fecha)
		{
			this.peso = peso;
			this.cadera = cadera;
			this.fecha = fecha;
		}

		public short Peso
		{
			get { return peso;}
			set { peso = value; }
		}

		public short Cadera
		{
			get { return cadera; }
			set { cadera = value; }
		}

		public DateTime Fecha
		{
			get { return fecha; }
			set { fecha = value; }
		}

		public override string ToString()
		{
			return string.Format("[Medidas: peso={0}, circunferencia abdominal={1}]", peso, cadera);
		}

		private short peso;
		private short cadera;
		private DateTime fecha;


	}
}
