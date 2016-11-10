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
		private string name;
		private short val;
		private DateTime date;

		// PROPS ...
		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public short Val
		{
			get { return val; }
			set { val = value; }
		}

		public DateTime Date
		{
			get { return date; }
			set { date = value; }
		}



		// CONSTRUCTOR ...

		// ... Para creación inicial.
		public Measure(int id, string name, short val)
		{
			this.Id = id;
			this.Name = name;
			this.Val = val;
			this.Date = DateTime.Now;
		}

		// ... Para utilizar en la carga del XML.
		public Measure(int id, string name, short val, DateTime date)
		{
			this.Id = id;
			this.Name = name;
			this.Val = val;
			this.Date = date;
		}
	}
}