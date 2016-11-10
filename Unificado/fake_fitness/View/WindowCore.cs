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

		private List<Measure> AllMeasures = new List<Measure>();
		private List<Measure> DayMeasures = new List<Measure>();
		private List<Measure> MonthMeasures = new List<Measure>();


		//-----------------------GENERALES/COMUNES---------------------

		// Cuando la aplicación se abre.
		private void OnInit()
		{
			XmlToExercisesList();
			XmlToMeasuresList();
			RefreshView();
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
			CalendarMonth();
			CalendarDay();
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

			// Recupera todas las medidas del día y las guarda en una lista de la clase.
			DayMeasures =
				AllMeasures.FindAll(am =>
					am.Date.Day == Calendar.Day
					&& am.Date.Month == Calendar.Month + 1
					&& am.Date.Year == Calendar.Year);

			// Muestra por pantalla las ocurrencias.
			UpdateExercisesTreeView();
			UpdateMeasuresTreeView();
		}

		// Gestiona los cambios cuando se cambia de mes en el calendario.
		private void CalendarMonth()
		{
			// Recupera todos los ejercicios del mes y los guarda en una lista de la clase.
			MonthExercises =
				AllExercises.FindAll(ae =>
					ae.Date.Month == Calendar.Month + 1
					&& ae.Date.Year == Calendar.Year);

			// Recupera todas las medidas del mes y las guarda en una lista de la clase.
			MonthMeasures =
				AllMeasures.FindAll(am =>
					am.Date.Month == Calendar.Month + 1
					&& am.Date.Year == Calendar.Year);

			// Borra todas las marcas
			for (uint i = 0; i <= 31; i++) { Calendar.UnmarkDay(i); }

			// Marcar las del nuevo mes
			CalendarMarkMonth();
		}

		// Marca los días con entrada de medidas o ejercicios.
		private void CalendarMarkMonth()
		{
			MonthExercises.ForEach(me => Calendar.MarkDay((uint)me.Date.Day));
			MonthMeasures.ForEach(me => Calendar.MarkDay((uint)me.Date.Day));
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
			if (ExerciseLoad.Text.All(char.IsDigit))
			{
				meters = Convert.ToInt16(ExerciseLoad.Text);
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
		private void ExercisesLoadEdit(object o, Gtk.EditedArgs e)
		{
			var exe = AllExercises.Find( ae => ae.Id == GetIdByEditArgs(e) );
			exe.Load = Convert.ToInt16(e.NewText);
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
			// Crear una ListStore vacía.
			var model = new Gtk.ListStore(typeof(string), typeof(string), typeof(string), typeof(string));

			// Inserta a la ListStore todas las coincidencias.
			DayExercises.ForEach(te => model.AppendValues(
				te.Name, te.Load.ToString(), te.Minutes.ToString(), te.Date.ToString()));

			// Actualiza la vista del arbol con la nueva info.
			ExercisesTreeview.Model = model;
		}

		// Cargar del XML a una Lista de ejercicios.
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
				child.Add(new XAttribute("Load", ae.Load));
				child.Add(new XAttribute("Minutes", ae.Minutes));
				child.Add(new XAttribute("Date", ae.Date.ToString()));
				root.Add(child);
			}

			root.Save(Core.Settings.ExercisesXML);
		}

		//--------------------------EJERCICIOS-------------------------


		//---------------------------MEDIDAS---------------------------

		// ... Eventos.

		// Cuando se añade un ejercicio.
		private void MeasureAdd()
		{
			var id = 0;
			var val = (short)0;
			var name = MeasureName.Text;

			foreach (var am in AllMeasures)
			{
				if (id <= am.Id) { id = am.Id + 1; }
			}

			// Comprueba que en la caja de texto de Value solo haya digitos.
			if (MeasureValue.Text.All(char.IsDigit))
			{
				val = Convert.ToInt16(MeasureValue.Text);
			}

			AllMeasures.Add(new Measure(id, name, val));
			RefreshView();
		}

		// Cuando se borra una medida.
		private void MeasureDelete(int ActiveRow)
		{
			// Obtiene la medida en base a la fila seleccionada.
			var measure =
				AllMeasures.Find(am =>
					am.Id == DayMeasures[ActiveRow].Id);

			// Borrar la medida seleccionada.
			AllMeasures.Remove(measure);

			// Actualiza los cambios en la vista.
			RefreshView();
		}

		// ... Campos editables en el tree view.

		// Cuando se edita el campo nombre.
		private void MeasureNameEdit(object o, Gtk.EditedArgs e)
		{
			var measure = AllMeasures.Find(am => am.Id == GetIdByEditArgs(e));
			measure.Name = e.NewText;
			RefreshView();
		}

		// Cuando se edita el campo valor.
		private void MeasureValueEdit(object o, Gtk.EditedArgs e)
		{
			var measure = AllMeasures.Find(am => am.Id == GetIdByEditArgs(e));
			measure.Val = Convert.ToInt16(e.NewText);
			RefreshView();
		}

		// ... Utilidades Listas y XML.

		// Carga la Lista en el Model del TreeView.
		private void UpdateMeasuresTreeView()
		{
			// Crear una ListStore vacía.
			var model = new Gtk.ListStore(typeof(string), typeof(string), typeof(string));

			// Inserta a la ListStore todas las coincidencias
			DayMeasures.ForEach(tm => model.AppendValues(
				tm.Name, tm.Val.ToString(), tm.Date.ToString()));

			// Actualiza la vista del arbol con la nueva info.
			MeasuresTreeview.Model = model;
		}

		// Cargar del XML a una Lista de ejercicios.
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
					var name = rc.ElementAt(1).Value;
					var val = Convert.ToInt16(rc.ElementAt(2).Value);
					DateTime date = Convert.ToDateTime(rc.ElementAt(3).Value);

					AllMeasures.Add(new Measure(id, name, val, date));
				}
			}
		}

		// Cuando se quiere guardar el contenido a xml.
		private void MeasuresListToXml()
		{
			var root = new XElement("Measures");

			foreach (var am in AllMeasures)
			{
				var child = new XElement("Measure");
				child.Add(new XAttribute("Id", am.Id));
				child.Add(new XAttribute("Name", am.Name));
				child.Add(new XAttribute("Value", am.Val));
				child.Add(new XAttribute("Date", am.Date.ToString()));
				root.Add(child);
			}

			root.Save(Core.Settings.MeasuresXML);
		}

		//---------------------------MEDIDAS---------------------------

	}
}
