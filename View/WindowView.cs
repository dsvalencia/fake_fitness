using System;
using FakeFitness.Core;

namespace FakeFitness.View
{
	public partial class MainWindow : Gtk.Window
	{
		/// Variable GTK para la vista calendario.
		private Gtk.Calendar Calendar;

		/// Variable GTK para la vista grafico.
		private Gtk.DrawingArea DrawingArea;

		/// Variable GTK para el textarea de la distancia de ejerecicios.
		private Gtk.Entry ExerciseDist;
		/// Variables GTK para el textarea de los minutos de ejercicios.
		private Gtk.Entry ExerciseMins;
		/// Variables GTK para el listado de ejrecicios.
		private Gtk.TreeView ExercisesTreeview;

		/// Variables GTK para el textarea del peso.
		private Gtk.Entry MeasureWeight;

		/// Variables GTK para el textarea de la talla.
		private Gtk.Entry MeasureSize;

		/// <summary>
		/// Constructor de la GUI.
		/// </summary>
		public MainWindow() : base(Gtk.WindowType.Toplevel)
		{
			this.Title = "••• DIA :: FakeFitness •••";
			Build();
			OnInit();
		}

		/// <summary>
		/// Cerrar la aplicacion.
		/// </summary>
		/// <returns>Void</returns>
		private void Quit() => Gtk.Application.Quit();

		
		[GLib.ConnectBeforeAttribute]
		/// <summary>
		/// Controla cuando se hace click derecho en un ejercicio.
		/// </summary>
		/// <returns>Void</returns>
		protected void OnRightExercise(object sender, Gtk.ButtonPressEventArgs e)
		{
			if (e.Event.Button == 3) // Botón derecho.
			{
				Gtk.TreePath RowPath;
				if (ExercisesTreeview.GetPathAtPos(
					Convert.ToInt32(e.Event.X),
					Convert.ToInt32(e.Event.Y),
					out RowPath))
				{
					int ActiveRow = RowPath.Indices[0];
					Gtk.Menu m = new Gtk.Menu();
					Gtk.MenuItem DeleteExercise = new Gtk.MenuItem("DELETE!");
					DeleteExercise.ButtonPressEvent += (o, a) => ExerciseDelete(ActiveRow);
					m.Add(DeleteExercise);
					m.ShowAll();
					m.Popup();

				}
			}
		}
		/// <summary>
		/// Construye la vista sobre la que se basa la aplicación
		/// </summary>
		/// <returns>Void</returns>
		private void Build()
		{

			// ----------------------TAMANHO-----------------------

			//SetGeometryHints(
			//	this, new Gdk.Geometry() { MinWidth = 400, MinHeight = 400 }, Gdk.WindowHints.MinSize);

			// ----------------------EVENTOS-----------------------

			DeleteEvent += (o, args) => OnClose();

			// ----------------------VISTAS------------------------

			var Layout = new Gtk.HBox(false, 5);
			var MainBox = new Gtk.VBox(false, 5);

			MainBox.Add( new Gtk.HSeparator() );
			MainBox.Add( CalendarView() );
			MainBox.Add( new Gtk.HSeparator() );
			MainBox.Add( MeasureView() );
			MainBox.Add( new Gtk.HSeparator() );
			MainBox.Add( ExercisesListView() );
			MainBox.Add( new Gtk.HSeparator() );
			MainBox.Add( ExercisesAddView() );
			MainBox.Add( new Gtk.HSeparator() );
			MainBox.Add( GraphicView() );
			MainBox.Add( new Gtk.HSeparator() );

			Layout.Add( new Gtk.VSeparator() );
			Layout.Add( MainBox );
			Layout.Add( new Gtk.VSeparator() );

			this.Add(Layout);
		}
		/// <summary>
		/// Genera la vista de <see cref="Calendar"></see> de la aplicación 
		/// </summary>
		/// <returns>La vista en que se muestra el <see cref="Calendar"></see></returns>
		private Gtk.VBox CalendarView()
		{
			var ViewBox = new Gtk.VBox(false, 5);

			// Calendar
			Calendar = new Gtk.Calendar();

			// Calendar events.
			Calendar.DaySelected += (o,args) => CalendarDay();
			Calendar.MonthChanged += (o, args) => CalendarMonth();

			ViewBox.Add(Calendar);

			return ViewBox;
		}
		/// <summary>
		/// Calcula la vista que mostrará la lista de ejercicios (<see cref="ExerciseTreeView"></see>). 
		/// </summary>
		/// <returns>La vista en la que se muestra una lista de los ejercicios (<see cref="ExerciseTreeView"></see>).</returns>
		private Gtk.VBox ExercisesListView()
		{
			var ViewBox = new Gtk.VBox(false, 5);

			// ------------------INICIALIZACION---------------------

			// Tree.
			ExercisesTreeview = new Gtk.TreeView();

			// Tree model.
			ExercisesTreeview.Model =
				new Gtk.ListStore(typeof(string), typeof(string), typeof(string));

			// Tree events
			ExercisesTreeview.ButtonPressEvent += new Gtk.ButtonPressEventHandler(OnRightExercise);

			// Tree columns.
			var DistRender = new Gtk.CellRendererText();
			DistRender.Editable = true;
			DistRender.Edited += ExercisesDistEdit;

			var MinsRender = new Gtk.CellRendererText();
			MinsRender.Editable = true;
			MinsRender.Edited += ExercisesMinsEdit;

			// Boxes Creation.
			var TreeScroll = new Gtk.ScrolledWindow();
			var TreeSpace = new Gtk.HBox(false, 5);

			// -------------------COMPOSICION------------------------

			// Tree Columns Append.
			ExercisesTreeview.AppendColumn("Dist", DistRender, "text", 0);
			ExercisesTreeview.AppendColumn("Mins", MinsRender, "text", 1);
			ExercisesTreeview.AppendColumn("Date", new Gtk.CellRendererText(), "text", 2);

			// Fill Boxes.
			TreeScroll.Add(ExercisesTreeview);
			TreeSpace.Add(TreeScroll);

			ViewBox.Add(TreeSpace);

			return ViewBox;
		}
		/// <summary>
		/// Genera la vista que añade ejercicios nuevos que cotendrán los parametros: <see cref="ExerciseDist"></see> y <see cref="ExerciseMin"></see> 
		/// </summary>
		/// <returns>Muestra la vista que permite añadir un ejercicio</returns>
		private Gtk.HBox ExercisesAddView()
		{
			var ViewBox = new Gtk.HBox(false, 5);

			// ------------------INICIALIZACION---------------------

			// Entrys.
			ExerciseDist = new Gtk.Entry("");
			ExerciseMins = new Gtk.Entry("");

			// Entrys Properties.
			ExerciseDist.Alignment = 0.5f;
			ExerciseMins.Alignment = 0.5f;

			// Labels.
			var DistLabel = new Gtk.Label("Dist:");
			var MinsLabel = new Gtk.Label("Mins:");

			// Buttons.
			var SaveButton = new Gtk.Button("SAVE");
			SaveButton.Clicked += (o, args) => ExerciseAdd();

			// -------------------COMPOSICION------------------------

			// Fill vboxs.
			ViewBox.Add(DistLabel);
			ViewBox.Add(ExerciseDist);
			ViewBox.Add(MinsLabel);
			ViewBox.Add(ExerciseMins);

			// Compose main vBox.
			ViewBox.Add(SaveButton);

			// Add main box to the window
			return ViewBox;
		}
		/// <summary>
		/// Genera la vista que añade medidas nuevas que cotendrán los parametros: <see cref="MesureWeight"></see> y <see cref="MesureSize"></see> 
		/// </summary>
		/// <returns>Muestra la vista que permite añadir una medida</returns>
		private Gtk.HBox MeasureView()
		{
			var ViewBox = new Gtk.HBox(false, 5);

			// ------------------INICIALIZACION---------------------

			// Entrys.
			MeasureWeight = new Gtk.Entry("");
			MeasureSize = new Gtk.Entry("");

			// Entrys Properties.
			MeasureWeight.Alignment = 0.5f;
			MeasureSize.Alignment = 0.5f;

			// Labels.
			var WeightLabel = new Gtk.Label("Weight:");
			var SizeLabel = new Gtk.Label("Size:");

			// Buttons.
			var SaveButton = new Gtk.Button("SAVE");
			SaveButton.Clicked += (o, args) => MeasureAdd();

			// -------------------COMPOSICION------------------------

			// Compose main Box.
			ViewBox.Add(WeightLabel);
			ViewBox.Add(MeasureWeight);
			ViewBox.Add(SizeLabel);
			ViewBox.Add(MeasureSize);
			ViewBox.Add(SaveButton);

			// Add main box to the window.
			return ViewBox;
		}
		/// <summary>
		/// Genera la vista que muestra el gráfico en la aplicación actualizando el <see cref="DrawingArea"></see>
		/// </summary>
		/// <returns>La vista que contiene el gráfico a mostrar</returns>
		private Gtk.HBox GraphicView()
		{
			var RightBox = new Gtk.VBox(false, 5);
			var ViewBox = new Gtk.HBox(false, 5);
			var Scrollable = new Gtk.ScrolledWindow();

			// ------------------INICIALIZACION---------------------

			var WeightButton = new Gtk.Button("WEIGHT");
			WeightButton.Clicked += (o, args) => GraphicWeight();

			var SizeButton = new Gtk.Button("SIZE");
			SizeButton.Clicked += (o, args) => GraphicSize();

			var TimeButton = new Gtk.Button("TIME");
			TimeButton.Clicked += (o, args) => GraphicTime();

			var DistButton = new Gtk.Button("DIST");
			DistButton.Clicked += (o, args) => GraphicDist();


			// -------------------COMPOSICION------------------------

			DrawingArea = new Gtk.DrawingArea();
			Scrollable.AddWithViewport( DrawingArea );

			// Fill Boxs.
			RightBox.Add(WeightButton);
			RightBox.Add(SizeButton);
			RightBox.Add(TimeButton);
			RightBox.Add(DistButton);

			// Compose main Box.
			ViewBox.Add(Scrollable);
			ViewBox.Add(RightBox);

			// Add main box to the window.
			return ViewBox;
		}
	}
}
