using System;
namespace Graficos
{
	public class Activity
	{

		private DateTime date;
		private int duration;
		private int distance;

		public int GetDuration()
		{
			return duration;
		}
		public int GetDistance()
		{
			return distance;
		}
		public DateTime GetDate()
		{
			return date;
		}

		public Activity(DateTime day, int durat, int distance)
		{
			this.duration = durat;
			this.date = day;
			this.distance = distance;

		}
	}
}

