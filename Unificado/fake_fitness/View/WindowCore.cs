using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using FakeFitness.Core;

namespace FakeFitness.View
{
	public partial class MainWindow
	{
		private List<Exercise> AllExercises = new List<Exercise>();
		private List<Exercise> DayExercises = new List<Exercise>();
		private List<Exercise> MonthExercises = new List<Exercise>();

		//private List<Exercise> AllMetrics = new List<Exercise>();
		//private List<Exercise> DayMetrics = new List<Exercise>();
		//private List<Exercise> MonthMetrics = new List<Exercise>();


		//-----------------------GENERALES/COMUNES---------------------

		// Cuando la aplicación se abre.
		private void OnInit()
		{
			XmlToExercisesList();
			RefreshView();
			CalendarDay();
			CalendarMonth();
		}

		// Cuando se cierra la aplicacion.
		private void OnClose()
		{
			ExercisesListToXml();
			Quit();
		}

		// Recarga la vista con los cambios sufridos.
		private void RefreshView()
		{
			CalendarMarkMonth();
			CalendarDay();
			CalendarMonth();
			UpdateExercisesTreeView();
			//UpdateMeticsTreeView();
		}

		private int GetIdByEditArgs(Gtk.EditedArgs e)
		{
			var RowPath = new Gtk.TreePath(e.Path);
			var ActiveRow = RowPath.Indices[0];
			return DayExercises[ActiveRow].Id;
		}
		//-----------------------GENERALES/COMUNES---------------------



		//--------------------------CALENDARIO-------------------------

		// Gestiona los cambios al clicar en un día concreto.
		private void CalendarDay()
		{
			// Recupera todos los ejercicios del día y los guarda en una lista de la clase.
			DayExercises =
				AllExercises.FindAll(ae =>
					ae.Date.Day == Calendar.Day
					&& ae.Date.Month == Calendar.Month+1
					&& ae.Date.Year == Calendar.Year);

			// Muestra por pantalla las ocurrencias.
			UpdateExercisesTreeView();
		}

		// Gestiona los cambios cuando se cambia de mes en el calendario.
		private void CalendarMonth()
		{
			// Recupera todos los ejercicios del mes y los guarda en una lista de la clase.
			MonthExercises =
				AllExercises.FindAll(ae =>
					ae.Date.Month == Calendar.Month + 1
					&& ae.Date.Year == Calendar.Year);

			// Borra todas las marcas
			for (uint i = 0; i <= 31; i++) { Calendar.UnmarkDay(i); }

			// Marcar las del nuevo mes
			CalendarMarkMonth();
		}

		// Marca los días con entrada de medidas o ejercicios.
		private void CalendarMarkMonth()
		{
			MonthExercises.ForEach(me => Calendar.MarkDay((uint)me.Date.Day));
			//MonthMetrics.ForEach(me => Calendar.MarkDay((uint)me.Date.Day));
		}

		//--------------------------CALENDARIO-------------------------



		//--------------------------EJERCICIOS-------------------------

		// ... Eventos.

		// Cuando se añade un ejercicio.
		private void ExerciseAdd()
		{
			var id = 0;
			var meters = (short)0;
			var minutes = (short)0;
			var name = ExerciseName.Text;

			foreach (var ae in AllExercises)
			{
				if (id <= ae.Id) { id = ae.Id + 1; }
			}

			// Comprueba que en la caja de texto de Metros solo haya digitos.
			if (ExerciseMeters.Text.All(char.IsDigit))
			{
				meters = Convert.ToInt16(ExerciseMeters.Text);
			}

			// Comprueba que en la caja de texto de Minutos solo haya digitos.
			if (ExerciseMinutes.Text.All(char.IsDigit))
			{
				minutes = Convert.ToInt16(ExerciseMinutes.Text);
			}

			AllExercises.Add(new Exercise(id, name, meters, minutes));
			RefreshView();
		}

		// Cuando se borra un ejercicio.
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

		// Cuando se edita el campo nombre.
		private void ExercisesNameEdit(object o, Gtk.EditedArgs e)
		{
			var exe = AllExercises.Find(ae => ae.Id == GetIdByEditArgs(e));
			exe.Name = e.NewText;
			RefreshView();
		}

		// Cuando se edita el campo metros.
		private void ExercisesMetersEdit(object o, Gtk.EditedArgs e)
		{
			var exe = AllExercises.Find( ae => ae.Id == GetIdByEditArgs(e) );
			exe.Meters = Convert.ToInt16(e.NewText);
			RefreshView();
		}

		//Cuando se edita el campo minutos.
		private void ExercisesMinutesEdit(object o, Gtk.EditedArgs e)
		{
			var exe = AllExercises.Find(ae => ae.Id == GetIdByEditArgs(e));
			exe.Minutes = Convert.ToInt16(e.NewText);
			RefreshView();
		}

		// ... Utilidades Listas y XML.

		// Carga la Lista en el Model del TreeView.
		private void UpdateExercisesTreeView()
		{
			// Load model as ListStore to be able to insert items.
			var model = new Gtk.ListStore(typeof(string), typeof(string), typeof(string), typeof(string));

			// Insert to the ListStore, all exercises on the list
			DayExercises.ForEach(te => model.AppendValues(
				te.Name, te.Meters.ToString(), te.Minutes.ToString(), te.Date.ToString()));

			// Actualiza la vista del arbol con la nueva info.
			ExercisesTreeview.Model = model;
		}

		// Cargar del XML a una Lista de ejercicios.
		private void XmlToExercisesList()
		{
			var CalendarDate = DateTime.Now;
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
					var name = rc.ElementAt(1).Value;
					var meters = Convert.ToInt16(rc.ElementAt(2).Value);
					var minutes = Convert.ToInt16(rc.ElementAt(3).Value);
					DateTime date = Convert.ToDateTime(rc.ElementAt(4).Value);

					AllExercises.Add(new Exercise(id, name, meters, minutes, date));
				}
			}
		}

		// Cuando se quiere guardar el contenido a xml.
		private void ExercisesListToXml()
		{
			var root = new XElement("Exercises");

			foreach (var ae in AllExercises)
			{
				var child = new XElement("Exercise");
				child.Add(new XAttribute("Id", ae.Id));
				child.Add(new XAttribute("Name", ae.Name));
				child.Add(new XAttribute("Meters", ae.Meters));
				child.Add(new XAttribute("Minutes", ae.Minutes));
				child.Add(new XAttribute("Date", ae.Date.ToString()));
				root.Add(child);
			}

			root.Save(Core.Settings.ExercisesXML);
		}

		//--------------------------EJERCICIOS-------------------------

	}
}
