using System;

namespace FakeFitness.Core
{
	public class Exercise
	{
		// VARS ...
		private int id;
		private string name;
		private short meters;
		private short minutes;
		private DateTime date;

		// PROPS ...
		public int Id
		{
			get { return id; }
			set { this.id = value; }
		}

		public string Name
		{
			get { return name; }
			set { this.name = value; }
		}

		public short Meters
		{
			get { return meters; }
			set { this.meters = value; }
		}

		public short Minutes
		{
			get { return minutes; }
			set { this.minutes = value; }
		}

		public DateTime Date
		{
			get { return date; }
			set { this.date = value; }
		}


		// CONSTRUCTOR ...

		// ... Para creación inicial.
		public Exercise(int id, string name, short meters, short minutes)
		{
			this.Id = id;
			this.Name = name;
			this.Meters = meters;
			this.Minutes = minutes;
			this.Date = DateTime.Now;
		}

		// ... Para utilizar en la carga del XML.
		public Exercise(int id, string name, short meters, short minutes, DateTime date)
		{
			this.Id = id;
			this.Name = name;
			this.Meters = meters;
			this.Minutes = minutes;
			this.Date = date;
		}

	}
}
