using System;
using System.Text;

namespace Graficos
{
	public partial class Drawing
	{
		private int[] dataArray = new int[31];
		GraphicsCore core;

	
		public void SetDiario(Diary diary)
		{
			 core= new GraphicsCore(diary);
		}
		public void SetMes()
		{
			core.SetMes(new DateTime(2016, 4, 1));		
		}
		private String action = "default";

		private void OnExposeDrawingArea()
		{
			switch (action)
			{

				case "default":
					{
						using (var canvas = Gdk.CairoHelper.Create(this.drawingArea.GdkWindow))
						{
							this.drawingArea.GdkWindow.Clear();
							// Axis
							canvas.MoveTo(0, 10);
							canvas.ShowText(Encoding.UTF8.GetBytes("Peso".ToCharArray()));
							canvas.LineWidth = 4;
							canvas.MoveTo(40, 10);
							canvas.LineTo(40, 550);
							canvas.LineTo(450, 550);
							canvas.MoveTo(460, 550);
							canvas.ShowText(Encoding.UTF8.GetBytes("Mes".ToCharArray()));

							//anotations axis x
							for (int i = 425, j = 7; i > 25; i -= 50, j--)
							{
								canvas.MoveTo(i, 560);
								canvas.ShowText(Encoding.UTF8.GetBytes("|".ToCharArray()));
								canvas.MoveTo(i - 15, 570);
							}

							//anotations axis y 
							for (int i = 525, j = 1; i > 25; i -= 50, j++)
							{
								canvas.MoveTo(35, i);
								canvas.ShowText(Encoding.UTF8.GetBytes("-".ToCharArray()));
								canvas.MoveTo(20, i);
								canvas.ShowText(Encoding.UTF8.GetBytes((j * 6).ToString().ToCharArray()));
							}


							canvas.Stroke();

							// Data
							canvas.LineWidth = 3;
							canvas.SetSourceRGBA(255, 0, 0, 255);
							canvas.MoveTo(40, 550);
							for (int i = 0; i < 31; i++)
							{
								if (dataArray[i] != 0)
								{
									canvas.LineTo(75 + 50 * i, 550 - dataArray[i]);
								}
								else {
									int previus = i;
									while (dataArray[previus] == 0 && previus > 0) { previus--; }
									canvas.LineTo(75 + 14 * i, 550 - dataArray[previus]);
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
							canvas.MoveTo(40, 10);
							canvas.LineTo(40, 550);
							canvas.LineTo(450, 550);
							canvas.MoveTo(460, 550);
							canvas.ShowText(Encoding.UTF8.GetBytes("Mes".ToCharArray()));

							//anotations axis x
							for (int i = 425, j = 7; i > 25; i -= 50, j--)
							{
								canvas.MoveTo(i, 560);
								canvas.ShowText(Encoding.UTF8.GetBytes("|".ToCharArray()));
								canvas.MoveTo(i - 15, 570);
							}

							//anotations axis y 
							for (int i = 525, j = 1; i > 25; i -= 50, j++)
							{
								canvas.MoveTo(35, i);
								canvas.ShowText(Encoding.UTF8.GetBytes("-".ToCharArray()));
								canvas.MoveTo(20, i);
								canvas.ShowText(Encoding.UTF8.GetBytes((j * 6).ToString().ToCharArray()));
							}


							canvas.Stroke();

							// Data
							canvas.LineWidth = 3;
							canvas.SetSourceRGBA(255, 0, 0, 255);
							canvas.MoveTo(40, 550);
							for (int i = 0; i < 31; i++)
							{
								if (dataArray[i] != 0)
								{
									canvas.LineTo(75 + 50 * i, 550 - dataArray[i]);
								}
								else {
									int previus = i;
									while (dataArray[previus] == 0 && previus > 0) { previus--; }
									canvas.LineTo(75 + 14 * i, 550 - dataArray[previus]);
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
							canvas.ShowText(Encoding.UTF8.GetBytes("Tiempo Ejercicio".ToCharArray()));
							canvas.LineWidth = 4;
							canvas.MoveTo(40, 10);
							canvas.LineTo(40, 550);
							canvas.LineTo(450, 550);
							canvas.MoveTo(460, 550);
							canvas.ShowText(Encoding.UTF8.GetBytes("Mes".ToCharArray()));

							//anotations axis x
							for (int i = 425, j = 7; i > 25; i -= 50, j--)
							{
								canvas.MoveTo(i, 560);
								canvas.ShowText(Encoding.UTF8.GetBytes("|".ToCharArray()));
								canvas.MoveTo(i - 15, 570);
							}

							//anotations axis y 
							for (int i = 525, j = 1; i > 25; i -= 50, j++)
							{
								canvas.MoveTo(35, i);
								canvas.ShowText(Encoding.UTF8.GetBytes("-".ToCharArray()));
								canvas.MoveTo(20, i);
								canvas.ShowText(Encoding.UTF8.GetBytes((j * 6).ToString().ToCharArray()));
							}


							canvas.Stroke();

					// Data
					canvas.LineWidth = 3;
					canvas.SetSourceRGBA(255, 200, 0, 255);
					canvas.MoveTo(40, 550);
							// Data
							canvas.LineWidth = 3;
							canvas.SetSourceRGBA(255, 0, 0, 255);
							canvas.MoveTo(40, 550);
							for (int i = 0; i < 31; i++)
							{
								canvas.LineTo(75 + 14 * i, 550 - dataArray[i]);
								canvas.ShowText(Encoding.UTF8.GetBytes(dataArray[i].ToString().ToCharArray()));

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
							canvas.MoveTo(40, 10);
							canvas.LineTo(40, 550);
							canvas.LineTo(450, 550);
							canvas.MoveTo(460, 550);
							canvas.ShowText(Encoding.UTF8.GetBytes("Mes".ToCharArray()));

							//anotations axis x
							for (int i = 425, j = 7; i > 25; i -= 50, j--)
							{
								canvas.MoveTo(i, 560);
								canvas.ShowText(Encoding.UTF8.GetBytes("|".ToCharArray()));
								canvas.MoveTo(i - 15, 570);
							}

							//anotations axis y 
							for (int i = 525, j = 1; i > 25; i -= 50, j++)
							{
								canvas.MoveTo(35, i);
								canvas.ShowText(Encoding.UTF8.GetBytes("-".ToCharArray()));
								canvas.MoveTo(20, i);
								canvas.ShowText(Encoding.UTF8.GetBytes((j * 6).ToString().ToCharArray()));
							}


							canvas.Stroke();

							// Data
							canvas.LineWidth = 3;
							canvas.SetSourceRGBA(255, 200, 0, 255);
							canvas.MoveTo(40, 550);
							for (int i = 0; i < 31; i++)
							{
								Console.WriteLine(i);
								Console.WriteLine(dataArray[i]);
								canvas.LineTo(75 + 14 * i, 550 - dataArray[i]*30);
								canvas.ShowText(Encoding.UTF8.GetBytes(dataArray[i].ToString().ToCharArray()));

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
							canvas.ShowText(Encoding.UTF8.GetBytes("Distancia Ejercicio".ToCharArray()));
							canvas.LineWidth = 4;
							canvas.MoveTo(40, 10);
							canvas.LineTo(40, 550);
							canvas.LineTo(450, 550);
							canvas.MoveTo(460, 550);
							canvas.ShowText(Encoding.UTF8.GetBytes("Mes".ToCharArray()));

							//anotations axis x
							for (int i = 425, j = 7; i > 25; i -= 50, j--)
							{
								canvas.MoveTo(i, 560);
								canvas.ShowText(Encoding.UTF8.GetBytes("|".ToCharArray()));
								canvas.MoveTo(i - 15, 570);
							}

							//anotations axis y 
							for (int i = 525, j = 1; i > 25; i -= 50, j++)
							{
								canvas.MoveTo(35, i);
								canvas.ShowText(Encoding.UTF8.GetBytes("-".ToCharArray()));
								canvas.MoveTo(20, i);
								canvas.ShowText(Encoding.UTF8.GetBytes((j * 6).ToString().ToCharArray()));
							}


							canvas.Stroke();

							// Data
							canvas.LineWidth = 3;
							canvas.SetSourceRGBA(255, 200, 0, 255);
							canvas.MoveTo(40, 550);
							for (int i = 0; i < 31; i++)
							{
								canvas.LineTo(75 + 14 * i, 550 - dataArray[i]);
								canvas.ShowText(Encoding.UTF8.GetBytes(dataArray[i].ToString().ToCharArray()));

							}
							canvas.Stroke();

							// Clean
							canvas.GetTarget().Dispose();
						}
						break;
					}
		}
			}
		void onTimeActivities()
		{
			dataArray = core.analisePetition("Activity", 0);
			action = "TimeActivities";
			OnExposeDrawingArea();
			this.Resize(800, 700);
		}

		void onAbdomen()
		{	dataArray = core.analisePetition("BodyMeasures", 1);
			action = "abCirc";
			OnExposeDrawingArea();
			this.Resize(800, 700);
		}



		void onNumberActivities()
		{
			dataArray = core.analisePetition("Activity", 1);
			action = "NumberActivities";
			OnExposeDrawingArea();
			this.Resize(800, 700);
		}

		void onDistanceActivities()
		{
			dataArray = core.analisePetition("Activity", 2);
			action = "DistanceActivities";
			OnExposeDrawingArea();
			this.Resize(800, 700);
		}		void onPeso()
		{
			dataArray = core.analisePetition("BodyMeasures", 0);
			action = "default";
			OnExposeDrawingArea();
			this.Resize(800, 700);
		}

	
		}
	}


