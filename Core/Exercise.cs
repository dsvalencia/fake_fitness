using System;

namespace FakeFitness.Core
{
	public class Exercise
	{
		// VARS ...
		private int id;
		private short dist;
		private short mins;
		private DateTime date;

		// PROPS ...
		public int Id
		{
			get { return id; }
			set { this.id = value; }
		}

		public short Dist
		{
			get { return dist; }
			set { this.dist = value; }
		}

		public short Mins
		{
			get { return mins; }
			set { this.mins = value; }
		}

		public DateTime Date
		{
			get { return date; }
			set { this.date = value; }
		}


		// CONSTRUCTOR ...

		// ... Para creación inicial.
		public Exercise(int id, short dist, short mins)
		{
			this.Id = id;
			this.Dist = dist;
			this.Mins = mins;
			this.Date = DateTime.Now;
		}

		// ... Para utilizar en la carga del XML.
		public Exercise(int id, short dist, short mins, DateTime date)
		{
			this.Id = id;
			this.Dist = dist;
			this.Mins = mins;
			this.Date = date;
		}

	}
}
