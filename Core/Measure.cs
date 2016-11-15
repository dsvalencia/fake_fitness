using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace FakeFitness.Core
{
	public class Measure
	{
		// VARS ...
		private int id;
		private short weight;
		private short size;
		private DateTime date;

		// PROPS ...
		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		public short Weight
		{
			get { return weight; }
			set { weight = value; }
		}

		public short Size
		{
			get { return size; }
			set { size = value; }
		}

		public DateTime Date
		{
			get { return date; }
			set { date = value; }
		}



		// CONSTRUCTOR ...

		// ... Para creación inicial.
		public Measure(int id, short weight, short size)
		{
			this.Id = id;
			this.Weight = weight;
			this.Size = size;
			this.Date = DateTime.Now;
		}

		// ... Para utilizar en la carga del XML.
		public Measure(int id, short weight, short size, DateTime date)
		{
			this.Id = id;
			this.Weight = weight;
			this.Size = size;
			this.Date = date;
		}
	}
}