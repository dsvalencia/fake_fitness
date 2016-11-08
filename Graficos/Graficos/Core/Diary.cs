using System;
using System.Collections.Generic;

namespace Graficos
{
	public class Diary
	{
		List<BodyMeasures> measures;
		List<Activity> exercise;

		public Diary()
		{
			this.measures = new List<BodyMeasures>();
			this.exercise = new List<Activity>();
	
		}

		public List<Activity> GetActivities()
		{
			return this.exercise;
		}
		public List<BodyMeasures> GetMeasures()
		{
			return this.measures;
		}
		public void AddMeasure(BodyMeasures measure)
		{
			this.measures.Add(measure);
			return;
		}
		public void AddExercise(Activity activity){
			this.exercise.Add(activity);
			return;
		}
		public Boolean DeleteExercise(Activity activity)
		{
			return this.exercise.Remove(activity);
		}
		public Boolean DeleteMeasure(BodyMeasures measure)
		{
			return this.measures.Remove(measure);
		}

		public override string ToString()
		{
			return string.Format("[Diary]"+ this.exercise.Count+", "+ this.measures.Count);
		}
	}
}
