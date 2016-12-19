using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using FakeFitness.Core;

namespace FakeFitness.View
{
	public partial class MainWindow
	{
		
		//----------------------------GLOBALES-------------------------

		/// Lista de todos los ejercicios almacenados.
		private List<Exercise> AllExercises = new List<Exercise>();
		/// Lista de todos los ejercicios almacenados en un dia.
		private List<Exercise> DayExercises = new List<Exercise>();
		/// Lista de todos los ejercicios almacenados en un mes.
		private List<Exercise> MonthExercises = new List<Exercise>();

		/// Lista de todas las medidas almacenadas.
		private List<Measure> AllMeasures = new List<Measure>();
		/// Lista de todas las medidas almacenadas en un dia.
		private List<Measure> DayMeasures = new List<Measure>();
		/// Lista de todas las medidas almacenadas en un mes.
		private List<Measure> MonthMeasures = new List<Measure>();

		/// Array con los datos del número de ejercicios realizados.
		private int[] GraphExercisesNum = new int[31];
		/// Array con los datos del tiempo invertido en ejercicios.
		private int[] GraphExercisesTime = new int[31];
		/// Array con los datos de las distancias de los ejercicios.
		private int[] GraphExercisesDist = new int[31];
		/// Array con los datos de las medidas tomadas.
		private int[] GraphMeasuresSize = new int[31];
		/// Array con los datos de la talla personal.
		private int[] GraphMeasuresWeight = new int[31];

		//----------------------------GLOBALES-------------------------



		//-----------------------GENERALES/COMUNES---------------------

		/// <summary>
		/// Gestiona el inicio de la aplicación.
		/// </summary>
		/// <returns>void</returns>
		private void OnInit()
		{
			XmlToExercisesList();
			XmlToMeasuresList();
			RefreshView();
		}

		/// <summary>
		/// Gestiona el cierre de la aplicacion.
		/// </summary>
		/// <returns>void</returns>
		private void OnClose()
		{
			ExercisesListToXml();
			MeasuresListToXml();
			Quit();
		}

		/// <summary>
		/// Gestiona la modificacion de la vista de la aplicacion.
		/// </summary>
		/// <returns>void</returns>
		private void RefreshView()
		{
			CalendarMarkMonth();
			CalendarMonth();
			CalendarDay();
		}

		/// <summary>
		/// Obtiene el id del elemento que se edita en un TreeView.
		/// </summary>
		/// <param name=”e”>De tipo "Gtk.EditedArgs" es el elemento que se está editando.</param>
		/// <returns>El id (int) asociado al elemento.</returns>
		private int GetIdByEditArgs(Gtk.EditedArgs e)
		{
			var RowPath = new Gtk.TreePath(e.Path);
			var ActiveRow = RowPath.Indices[0];
			return DayExercises[ActiveRow].Id;
		}
		//-----------------------GENERALES/COMUNES---------------------


		//--------------------------CALENDARIO-------------------------

		/// <summary>
		/// Gestiona los cambios al clicar en un día concreto.
		/// Recupera todos los ejercicios del día y los guarda en <see cref="DayExercises"></see>.
		/// Recupera todas las medidas del día y las guarda en <see cref="DayMeasures"></see>.
		/// Las muestra en la lista de ejercicios y en la grafica.
		/// </summary>
		/// <returns>void</returns>
		private void CalendarDay()
		{

			DayExercises =
				AllExercises.FindAll(ae =>
					ae.Date.Day == Calendar.Day
					&& ae.Date.Month == Calendar.Month+1
					&& ae.Date.Year == Calendar.Year);

			DayMeasures =
				AllMeasures.FindAll(am =>
					am.Date.Day == Calendar.Day
					&& am.Date.Month == Calendar.Month + 1
					&& am.Date.Year == Calendar.Year);

			UpdateExercisesTreeView();
			UpdateMeasureData();
		}
		/// <summary>
		/// Gestiona los cambios cuando se cambia de mes en el calendario.
		/// Recupera todos los ejercicios del mes y los guarda en <see cref="MonthExercises"/>.
		/// Recupera todas las medidas del mes y las guarda en <see cref="MonthMesaures"/>.
		/// Elimina las marcas en los dias del calendario.
		/// Establece en negrita los días que tienen una o varias entradas de medidas o ejercicios.
		/// </summary>
		/// <returns>void</returns>
		private void CalendarMonth()
		{
			MonthExercises =
				AllExercises.FindAll(ae =>
					ae.Date.Month == Calendar.Month + 1
					&& ae.Date.Year == Calendar.Year);

			MonthMeasures =
				AllMeasures.FindAll(am =>
					am.Date.Month == Calendar.Month + 1
					&& am.Date.Year == Calendar.Year);

			CalendarCleanMonth();
			CalendarMarkMonth();
			MonthExerciseData();
			MonthMeasureData();
		}

		/// <summary>
		/// Eliminina las marcas que tienen los dias en el calendario
		/// </summary>
		/// <returns>void</returns>
		private void CalendarCleanMonth(){
			for (uint i = 0; i <= 31; i++){ 
				Calendar.UnmarkDay(i); 
			}
		}

		/// <summary>
		/// Establece en negrita los días que tienen una o varias entradas de medidas o ejercicios.
		/// </summary>
		/// <returns>void</returns>
		private void CalendarMarkMonth()
		{
			MonthExercises.ForEach(me => Calendar.MarkDay((uint)me.Date.Day));
			MonthMeasures.ForEach(me => Calendar.MarkDay((uint)me.Date.Day));
		}

		//--------------------------CALENDARIO-------------------------



		//--------------------------EJERCICIOS-------------------------

		// ... Eventos.

		/// <summary>
		/// Cuando se añade un ejercicio, se calcula su ID, se leen los campos <see cref="ExerciseDist"/> 
		/// y <see cref="ExerciseMins"/>.
		/// </summary>
		/// <returns>void</returns>
		private void ExerciseAdd()
		{
			var id = 0;
			var meters = (short)0;
			var minutes = (short)0;

			foreach (var ae in AllExercises)
			{
				if (id <= ae.Id) { id = ae.Id + 1; }
			}

			// Comprueba que en la caja de texto de Metros solo haya digitos.
			if (ExerciseDist.Text.All(char.IsDigit))
			{
				meters = Convert.ToInt16(ExerciseDist.Text);
			}

			// Comprueba que en la caja de texto de Minutos solo haya digitos.
			if (ExerciseMins.Text.All(char.IsDigit))
			{
				minutes = Convert.ToInt16(ExerciseMins.Text);
			}

			AllExercises.Add(new Exercise(id, meters, minutes));

			ExerciseMins.DeleteText(0, ExerciseMins.Text.Length);
			ExerciseDist.DeleteText(0, ExerciseDist.Text.Length);
			RefreshView();
		}

		/// <summary>
		/// Cuando se borra un ejercicio, en base a la fila activa del <see cref="ExercisesTreeview"/>.
		/// </summary>
		/// <param name="ActiveRow">Es la fila activa de la lista de ejercicios.</param>
		/// <returns>void</returns>
		private void ExerciseDelete(int ActiveRow)
		{
			// Obtiene el ejercicio en base a la fila seleccionada.
			var exe =
				AllExercises.Find(ae => 
					ae.Id == DayExercises[ActiveRow].Id);

			// Borrar el ejercicio seleccionado.
			AllExercises.Remove(exe);

			// Actualiza los cambios en la vista.
			RefreshView();
		}

		// ... Campos editables en el tree view.

		/// <summary>
		/// Edición de los Metros de un ejercicio (<see cref="Core.Exercise.dist"/>).
		/// </summary>
		/// <param name="ActiveRow">Es la fila activa de la lista de ejercicios.</param>
		/// <returns>void</returns>
		private void ExercisesDistEdit(object o, Gtk.EditedArgs e)
		{
			var exe = AllExercises.Find( ae => ae.Id == GetIdByEditArgs(e) );
			exe.Dist = Convert.ToInt16(e.NewText);
			RefreshView();
		}

		/// <summary>
		/// Edición de los Minutos de un ejercicio (<see cref="Core.Exercise.mins"/> ).
		/// </summary>
		/// <param name="o">Copia de <see cref="ExercisesTreeview"/>.</param>
		/// <param name="e">Elemento que está editando de la lista.</param>
		/// <returns>void</returns>
		private void ExercisesMinsEdit(object o, Gtk.EditedArgs e)
		{
			var exe = AllExercises.Find(ae => ae.Id == GetIdByEditArgs(e));
			exe.Mins = Convert.ToInt16(e.NewText);
			RefreshView();
		}

		// ... Utilidades Listas y XML.

		/// <summary>
		/// Carga la <see cref="AllExercises"/>  en el <see cref="ExercisesTreeview"/> .
		/// </summary>
		/// <returns>void</returns>
		private void UpdateExercisesTreeView()
		{
			// Crear una ListStore vacía.
			var model = new Gtk.ListStore(typeof(string), typeof(string), typeof(string));

			// Inserta a la ListStore todas las coincidencias.
			DayExercises.ForEach(te => model.AppendValues(
				te.Dist.ToString(), te.Mins.ToString(), te.Date.ToString()));

			// Actualiza la vista del arbol con la nueva info.
			ExercisesTreeview.Model = model;
		}

		/// <summary>
		/// Cargar del XML a <see cref="AllExercises"/> .
		/// </summary>
		/// <returns>void</returns>
		private void XmlToExercisesList()
		{
			if (File.Exists(Core.Settings.ExercisesXML))
			{
				// Carga en memoria el XML de ejercicios.
				var Root = XElement.Load(Core.Settings.ExercisesXML);

				// Extrae de queda ejercicio los atributos
				var RootChilds =
					from e
					in Root.Elements("Exercise")
					select e.Attributes();

				// Parsea los datos correctamente y los guarda en la lista.
				foreach (var rc in RootChilds)
				{
					var id = Convert.ToInt32(rc.ElementAt(0).Value);
					var dist = Convert.ToInt16(rc.ElementAt(1).Value);
					var mins = Convert.ToInt16(rc.ElementAt(2).Value);
					DateTime date = Convert.ToDateTime(rc.ElementAt(3).Value);

					AllExercises.Add(new Exercise(id, dist, mins, date));
				}
			}
		}

		/// <summary>
		/// Vuelca <see cref="AllExercises"/> al XML <see cref="Core.Settings.ExercisesXML"/>.
		/// </summary>
		/// <returns>void</returns>
		private void ExercisesListToXml()
		{
			var root = new XElement("Exercises");

			foreach (var ae in AllExercises)
			{
				var child = new XElement("Exercise");
				child.Add(new XAttribute("Id", ae.Id));
				child.Add(new XAttribute("Dist", ae.Dist));
				child.Add(new XAttribute("Mins", ae.Mins));
				child.Add(new XAttribute("Date", ae.Date.ToString()));
				root.Add(child);
			}

			root.Save(Core.Settings.ExercisesXML);
		}

		//--------------------------EJERCICIOS-------------------------


		//---------------------------MEDIDAS---------------------------

		// ... Eventos.

		/// <summary>
		/// Cuando se añade una medida, se calcula su ID, se leen los campos <see cref="MeasureWeight"/> 
		/// y <see cref="MeasureSize"/>.
		/// </summary>
		/// <returns>void</returns>
		private void MeasureAdd()
		{
			if (DayMeasures.Count() < 1)
			{
				var id = 0;
				var weight = (short)0;
				var size = (short)0;

				foreach (var am in AllMeasures)
				{
					if (id <= am.Id) { id = am.Id + 1; }
				}

				// Comprueba que en la caja de texto de Peso solo haya digitos.
				if (MeasureWeight.Text.All(char.IsDigit))
				{
					weight = Convert.ToInt16(MeasureWeight.Text);
				}

				// Comprueba que en la caja de texto de Talla solo haya digitos.
				if (MeasureSize.Text.All(char.IsDigit))
				{
					size = Convert.ToInt16(MeasureSize.Text);
				}
				var m = new Measure(id, weight, size);
				AllMeasures.Add(m);
			}

			else
			{
				var m = DayMeasures[0];
				// Comprueba que en la caja de texto de Peso solo haya digitos.
				if (MeasureWeight.Text.All(char.IsDigit))
				{
					m.Weight = Convert.ToInt16(MeasureWeight.Text);
				}

				// Comprueba que en la caja de texto de Talla solo haya digitos.
				if (MeasureSize.Text.All(char.IsDigit))
				{
					m.Size = Convert.ToInt16(MeasureSize.Text);
				}
			}

			UpdateMeasureData();
			RefreshView();
		}

		/// <summary>
		/// Actualiza los entrys de las medidas <see cref="MeasureWeight"/> y <see cref="MeasureSize"/>.
		/// </summary>
		/// <returns>void</returns>
		private void UpdateMeasureData()
		{
			if (DayMeasures.Count > 0)
			{
				var m = DayMeasures[0];
				MeasureWeight.DeleteText(0, MeasureWeight.Text.Length);
				MeasureWeight.InsertText(m.Weight.ToString());
				MeasureSize.DeleteText(0, MeasureSize.Text.Length);
				MeasureSize.InsertText(m.Size.ToString());
			}
			else
			{
				MeasureWeight.DeleteText(0, MeasureWeight.Text.Length);
				MeasureSize.DeleteText(0, MeasureSize.Text.Length);
			}
				
		}

		// ... Utilidades Listas y XML.

		/// <summary>
		/// Cargar del XML a <see cref="AllMeasures"/> .
		/// </summary>
		/// <returns>void</returns>
		private void XmlToMeasuresList()
		{
			if (File.Exists(Core.Settings.MeasuresXML))
			{
				// Carga en memoria el XML de ejercicios.
				var Root = XElement.Load(Core.Settings.MeasuresXML);

				// Extrae de queda ejercicio los atributos
				var RootChilds =
					from e
					in Root.Elements("Measure")
					select e.Attributes();

				// Parsea los datos correctamente y los guarda en la lista.
				foreach (var rc in RootChilds)
				{
					var id = Convert.ToInt32(rc.ElementAt(0).Value);
					var weight = Convert.ToInt16(rc.ElementAt(1).Value);
					var size = Convert.ToInt16(rc.ElementAt(2).Value);
					DateTime date = Convert.ToDateTime(rc.ElementAt(3).Value);

					AllMeasures.Add(new Measure(id, weight, size, date));
				}
			}
		}

		/// <summary>
		/// Volcar <see cref="AllMeasures"/> en el XML <see cref="Core.Settings.MeasuresXML"/>.
		/// </summary>
		/// <returns>void</returns>
		private void MeasuresListToXml()
		{
			var root = new XElement("Measures");

			foreach (var am in AllMeasures)
			{
				var child = new XElement("Measure");
				child.Add(new XAttribute("Id", am.Id));
				child.Add(new XAttribute("Weight", am.Weight));
				child.Add(new XAttribute("Size", am.Size));
				child.Add(new XAttribute("Date", am.Date.ToString()));
				root.Add(child);
			}

			root.Save(Core.Settings.MeasuresXML);
		}

		//---------------------------MEDIDAS---------------------------


		//---------------------------GRAFICO---------------------------

		private int[] CurrentGraphData = new int[31];

		/// <summary>
		/// Carga por separado los distintos datos de ejercicios para su graficado.
		/// </summary>
		/// <returns>void</returns>
		private void MonthExerciseData()
		{
			GraphExercisesNum = new int[31];
			GraphExercisesTime = new int[31];
			GraphExercisesDist = new int[31];

			if (MonthExercises.Count > 0)
			{
				foreach (var ae in MonthExercises)
				{
					GraphExercisesNum[ae.Date.Day - 1] =
						GraphExercisesNum[ae.Date.Day - 1] + 1;

					GraphExercisesTime[ae.Date.Day - 1] =
						GraphExercisesTime[ae.Date.Day - 1] + Convert.ToInt32(ae.Mins/60);

					GraphExercisesDist[ae.Date.Day - 1] =
						GraphExercisesDist[ae.Date.Day - 1] + Convert.ToInt32(ae.Dist/1000);
				}
			}

		}

		/// <summary>
		/// Carga por separado los distintos datos de medidas para su graficado.
		/// </summary>
		/// <returns>void</returns>
		private void MonthMeasureData()
		{
			GraphMeasuresSize = new int[31];
			GraphMeasuresWeight = new int[31];

			if (MonthMeasures.Count > 0)
			{
				foreach (var mm in MonthMeasures)
				{
					GraphMeasuresWeight[mm.Date.Day - 1] =
						GraphMeasuresWeight[mm.Date.Day - 1] + Convert.ToInt32(mm.Weight/4);

					GraphMeasuresSize[mm.Date.Day - 1] =
						GraphMeasuresSize[mm.Date.Day - 1] + Convert.ToInt32(mm.Size/4);
				}	
			}

		}

		/// <summary>
		/// Dibuja el grafico en base a <see cref="GraphExercisesDist"/>.
		/// </summary>
		/// <returns>void</returns>
		private void GraphicDist()
		{
			CurrentGraphData = GraphExercisesDist;
			RenderGraph("KiloMeters", 255, 30, 0);
		}

		/// <summary>
		/// Dibuja el grafico en base a <see cref="GraphExercisesTime"/>.
		/// </summary>
		/// <returns>void</returns>
		private void GraphicTime()
		{
			CurrentGraphData = GraphExercisesTime;
			RenderGraph("Hours", 0, 0, 255);
		}

		/// <summary>
		/// Dibuja el grafico en base a <see cref="GraphMeasuresWeight"/>. 
		/// </summary>
		/// <returns>void</returns>
		private void GraphicWeight()
		{
			CurrentGraphData = GraphMeasuresWeight;
			RenderGraph("Weight", 255, 0, 0);
		}

		/// <summary>
		/// Dibuja el grafico en base a <see cref="GraphMeasuresSize"/>.
		/// </summary>
		/// <returns>void</returns>
		private void GraphicSize()
		{
			CurrentGraphData = GraphMeasuresSize;
			RenderGraph("Size", 0, 120, 0);
		}

		/// <summary>
		/// Renderiza el grafico en base a los datos de <see cref="CurrentGraphData"/>.
		/// </summary>
		/// <returns>void</returns>
		private void RenderGraph(string SectionSTR, int R, int G, int B)
		{
			using (var canvas = Gdk.CairoHelper.Create(DrawingArea.GdkWindow))
			{
				DrawingArea.GdkWindow.Clear();


				// Axis.
				canvas.MoveTo(5, 15);
				canvas.ShowText(Encoding.UTF8.GetBytes(SectionSTR.ToCharArray()));
				canvas.LineWidth = 2;
				canvas.MoveTo(20, 30);
				canvas.LineTo(20, 90);
				canvas.LineTo(190, 90);
				canvas.MoveTo(200, 90);
				canvas.ShowText(Encoding.UTF8.GetBytes("D".ToCharArray()));

				canvas.Stroke();


				// Data
				canvas.LineWidth = 4;
				canvas.SetSourceRGBA(R, G, B, 255);
				canvas.MoveTo(20, 90);

				for (int i = 0; i < 31; i++)
				{
					canvas.LineTo(16 + 6 * i, 90 - CurrentGraphData[i]);
				}

				canvas.Stroke();


				// Clean
				canvas.GetTarget().Dispose();
			}
		}

		//---------------------------GRAFICO---------------------------

	}
}
