using System;
using System.Collections.Generic;

namespace Graficos
{
	public class DataGraphicsCore
	{

		//Clase para generar datos de las gráficas 


		private int[] dataArray ;

		private List<Activity> activities;
		private List<BodyMeasures> measures;
		Diary diario;
		private int ActualMonth;
		public DataGraphicsCore(Diary diario)
		{
			dataArray = new int[31];
			activities = new List<Activity>();
			measures = new List<BodyMeasures>();
			this.diario = diario;
		}


		public void InitialiteDataArray()
		{
			for (int i = 0; i < 31; i++)
			{
				dataArray[i] = 0;
			}
		}

		public void SetMes(DateTime actualDatetime)
		{
			ActualMonth = actualDatetime.Month;
		}

		public int[] analisePetition(String Type, int parameter)
		{
			if (Type == null)
				throw new ArgumentNullException(nameof(Type));
			InitialiteDataArray();
			switch (Type)
			{
				case "Activity":
					{

						activities = diario.GetActivities();
						switch (parameter)
						{
							case 0:
								AnaliseDataTimeActivities();
								break;
							case 1:
								AnaliseDataNumberActivities();
								break;
							case 2:
								AnaliseDataDistanceActivities();
								break;

						}
						break;
					}
				case "BodyMeasures":
					{
						measures = diario.GetMeasures();
						switch (parameter)
						{
							case 0:
								AnaliseDataWeight();
								break;
							case 1:
								AnaliseDataCircunferenceAb();
								break;

						}
						break;
					}
				default:
					break;
			}

			return dataArray;

		}

		private void AnaliseDataTimeActivities()
		{
			InitialiteDataArray();
			foreach (Activity i in this.activities)
			{

				if (i.GetDate().Month == ActualMonth)
				{
					dataArray[i.GetDate().Day- 1] = dataArray[i.GetDate().Day- 1] + i.GetDuration();
				}

			}

		}

		private void AnaliseDataNumberActivities()
		{
			InitialiteDataArray();
			foreach (Activity i in this.activities)
			{

				if (i.GetDate().Month == ActualMonth)
				{
					dataArray[i.GetDate().Day- 1] = dataArray[i.GetDate().Day- 1] + 1;
				}

			}
		}

		private void AnaliseDataDistanceActivities()
		{
			InitialiteDataArray();
			foreach (Activity i in this.activities)
			{

				if (i.GetDate().Month == ActualMonth)
				{
					dataArray[i.GetDate().Day- 1] = dataArray[i.GetDate().Day- 1] + i.GetDistance();
				}

			}
		}

		private void AnaliseDataWeight()
		{
			InitialiteDataArray();
			foreach (BodyMeasures i in this.measures)
			{

				if (i.GetDate().Month == ActualMonth)
				{
					dataArray[i.GetDate().Day-1] = Convert.ToInt32(i.GetWeight());
				}

			}

		}
		private void AnaliseDataCircunferenceAb()
		{
			InitialiteDataArray();
			foreach (BodyMeasures i in this.measures)
			{

				if (i.GetDate().Month == ActualMonth)
				{
					dataArray[i.GetDate().Day- 1]= i.GetAbdominalCircunference();

				}

			}
		
		}



	}

}
