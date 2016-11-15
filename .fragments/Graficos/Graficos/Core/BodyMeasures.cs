using System;
namespace Graficos
{
	public class BodyMeasures
	{
		private DateTime date;
		public DateTime GetDate() { return date; }

		private double weight;
		public double GetWeight() { return weight; }

		private int abdominalCircunference;
		public int GetAbdominalCircunference() { return abdominalCircunference; }


		public BodyMeasures(DateTime date, double weight, int abdominalCircunference)
		{
			this.abdominalCircunference = abdominalCircunference;
			this.weight = weight;
			this.date = date;
		}
		public BodyMeasures(DateTime date, int abdominalCircunference, double weight)
		{
			this.weight = weight;
			this.abdominalCircunference = abdominalCircunference;
			this.date = date;
		}
	}
}
