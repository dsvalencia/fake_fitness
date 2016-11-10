using System;
using System.Text;

namespace Graficos
{
	public partial class Drawing
	{
		private int[] dataArray = new int[31]; //Array de datos a mostrar en gráficas
		DataGraphicsCore core;

		//Función para cargar nuevas Listas de actividades y medidas
		public void SetDiario(Diary diary)
		{
			 core= new DataGraphicsCore(diary);
		}
		//Función para cambiar mes activo en los gráficos 
		public void SetMes()
		{
			core.SetMes(new DateTime(2016, 4, 1));		
		}
		private String action = "weight"; //acción a ejecutar, por defecto Peso 

		private void OnExposeDrawingArea()
		{
			//Cambio de Gráficos

			switch (action) //Switch decisión tipo de datos a mostrar
			{

				case "weight":
					{
						using (var canvas = Gdk.CairoHelper.Create(this.drawingArea.GdkWindow))
						{
							this.drawingArea.GdkWindow.Clear();
							// Axis
							canvas.MoveTo(0, 10);
							canvas.ShowText(Encoding.UTF8.GetBytes("Peso".ToCharArray()));
							canvas.LineWidth = 4;
							canvas.MoveTo(10, 10);
							canvas.LineTo(10, 120);
							canvas.LineTo(197, 120);
							canvas.MoveTo(199, 120);
							canvas.ShowText(Encoding.UTF8.GetBytes("Mes".ToCharArray()));

							canvas.Stroke();

							// Data
							canvas.LineWidth = 3;
							canvas.SetSourceRGBA(255, 0, 0, 255);
							canvas.MoveTo(10, 120);
							for (int i = 0; i < 31; i++)
							{
								if (dataArray[i] != 0)
								{
									canvas.LineTo(16 + 6 * i, 120 - dataArray[i]);
								}
								else {
									int previus = i;
									while (dataArray[previus] == 0 && previus > 0) { previus--; }
									canvas.LineTo(16 + 6* i, 120 - dataArray[previus]);
								}


							}
							canvas.Stroke();

							// Clear	

							canvas.GetTarget().Dispose();
						}

							break;
						}
		case "abCirc":
					{
						using (var canvas = Gdk.CairoHelper.Create(this.drawingArea.GdkWindow))
						{
							this.drawingArea.GdkWindow.Clear();
							// Axis
							canvas.MoveTo(0, 10);
							canvas.ShowText(Encoding.UTF8.GetBytes("Circunferencia Abdominal".ToCharArray()));
							canvas.LineWidth = 4;
							canvas.MoveTo(10, 10);
							canvas.LineTo(10, 120);
							canvas.LineTo(197, 120);
							canvas.MoveTo(199, 120);
							canvas.ShowText(Encoding.UTF8.GetBytes("Mes".ToCharArray()));



							canvas.Stroke();

							// Data
							canvas.LineWidth = 3;
							canvas.SetSourceRGBA(0, 120, 0, 255);
							canvas.MoveTo(10, 120);
							for (int i = 0; i < 31; i++)
							{

								if (dataArray[i] != 0)
								{
									canvas.LineTo(16 + 6 * i, 120 - dataArray[i]);
								}
								else {
									int previus = i;
									while (dataArray[previus] == 0 && previus > 0) { previus--; }
									canvas.LineTo(16 + 6 * i, 120 - dataArray[previus]);
								}
							}
							canvas.Stroke();

							// Clear	

							canvas.GetTarget().Dispose();
						}

						break;
					}


				case "TimeActivities":
					{
				using (var canvas = Gdk.CairoHelper.Create(this.drawingArea.GdkWindow))
				{

							this.drawingArea.GdkWindow.Clear();


							// Axis
							canvas.MoveTo(0, 10);
							canvas.ShowText(Encoding.UTF8.GetBytes("Tiempo Actividades".ToCharArray()));
							canvas.LineWidth = 4;
							canvas.MoveTo(10, 10);
							canvas.LineTo(10, 120);
							canvas.LineTo(197, 120);
							canvas.MoveTo(199, 120);
							canvas.ShowText(Encoding.UTF8.GetBytes("Mes".ToCharArray()));




							canvas.Stroke();

						
							// Data
							canvas.LineWidth = 3;
							canvas.SetSourceRGBA(255, 200, 0, 255);
							canvas.MoveTo(10, 120);
							for (int i = 0; i < 31; i++)
							{
								canvas.LineTo(16 + 6 * i, 120 - dataArray[i]/5);

							}
					canvas.Stroke();

					// Clean
					canvas.GetTarget().Dispose();
				}
				break;
			}

			case "NumberActivities":
					{
						using (var canvas = Gdk.CairoHelper.Create(this.drawingArea.GdkWindow))
						{

							this.drawingArea.GdkWindow.Clear();


							// Axis
							canvas.MoveTo(0, 10);
							canvas.ShowText(Encoding.UTF8.GetBytes("Numero Actividades".ToCharArray()));
							canvas.LineWidth = 4;
							canvas.MoveTo(10, 10);
							canvas.LineTo(10, 120);
							canvas.LineTo(197, 120);
							canvas.MoveTo(199, 120);
							canvas.ShowText(Encoding.UTF8.GetBytes("Mes".ToCharArray()));



							canvas.Stroke();

							// Data
							canvas.LineWidth = 3;
							canvas.SetSourceRGBA(0, 0, 255, 255);
							canvas.MoveTo(10, 120);
							for (int i = 0; i < 31; i++)
							{
								Console.WriteLine(i);
								Console.WriteLine(dataArray[i]);
								canvas.LineTo(16 + 6 * i, 120 - dataArray[i]*6);

							}
							canvas.Stroke();

							// Clean
							canvas.GetTarget().Dispose();
						}
						break;
					}
					case "DistanceActivities":
					{
						using (var canvas = Gdk.CairoHelper.Create(this.drawingArea.GdkWindow))
						{

							this.drawingArea.GdkWindow.Clear();


							// Axis
							canvas.MoveTo(0, 10);
							canvas.ShowText(Encoding.UTF8.GetBytes("Distancia Actividades".ToCharArray()));
							canvas.LineWidth = 4;
							canvas.MoveTo(10, 10);
							canvas.LineTo(10, 120);
							canvas.LineTo(197, 120);
							canvas.MoveTo(199, 120);
							canvas.ShowText(Encoding.UTF8.GetBytes("Mes".ToCharArray()));



							canvas.Stroke();

							// Data
							canvas.LineWidth = 3;
							canvas.SetSourceRGBA(255,30,0,255);
							canvas.MoveTo(10, 120);
							for (int i = 0; i < 31; i++)
							{
								canvas.LineTo(16 + 6 * i, 120 - dataArray[i]/5);

							}
							canvas.Stroke();

							// Clean
							canvas.GetTarget().Dispose();
						}
						break;
					}
				}
			}

		//Accion Botón Tiempo de Actividades
		void onTimeActivities()
		{
			dataArray = core.analisePetition("Activity", 0);
			action = "TimeActivities";
			OnExposeDrawingArea();
		}

		//Accion Botón Circunferencia Abdominal
		void onAbdomen()
		{	dataArray = core.analisePetition("BodyMeasures", 1);
			action = "abCirc";
			OnExposeDrawingArea();
		}

		//Accion Botón Número Actividades

		void onNumberActivities()
		{
			dataArray = core.analisePetition("Activity", 1);
			action = "NumberActivities";
			OnExposeDrawingArea();
		}

		//Accion Botón Distancia Actividades

		void onDistanceActivities()
		{
			dataArray = core.analisePetition("Activity", 2);
			action = "DistanceActivities";
			OnExposeDrawingArea();
		}		

		//Accion Botón Peso

		void onPeso()
		{
			dataArray = core.analisePetition("BodyMeasures", 0);
			action = "weight";
			OnExposeDrawingArea();
		}

	
		}
	}


