using System;
using FakeFitness.Core;

namespace FakeFitness.View
{
	public partial class MainWindow : Gtk.Window
	{
		// Variables GTK vista calendario.
		private Gtk.Calendar Calendar;

		// Variables GTK vista ejerecicicios.
		private Gtk.Entry ExerciseName;
		private Gtk.Entry ExerciseLoad;
		private Gtk.Entry ExerciseMinutes;
		private Gtk.TreeView ExercisesTreeview;

		// Variables GTK vista ejerecicicios.
		private Gtk.Entry MeasureName;
		private Gtk.Entry MeasureValue;
		private Gtk.TreeView MeasuresTreeview;
		
		public MainWindow() : base(Gtk.WindowType.Toplevel)
		{
			Build();
			OnInit();
		}

		// Cerrar la aplicacion.
		private void Quit() => Gtk.Application.Quit();

		// Controla cuando se hace click derecho en un ejercicio.
		[GLib.ConnectBeforeAttribute]
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

		// Controla cuando se hace click derecho en una medida.
		[GLib.ConnectBeforeAttribute]
		protected void OnRightMeasure(object sender, Gtk.ButtonPressEventArgs e)
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

		private void Build()
		{

			// ----------------------TAMANHO-----------------------

			//SetGeometryHints(
			//	this, new Gdk.Geometry() { MinWidth = 300, MinHeight = 300 }, Gdk.WindowHints.MinSize);

			// ----------------------EVENTOS-----------------------

			DeleteEvent += (o, args) => OnClose();

			// ----------------------VISTAS------------------------

			var MainBox = new Gtk.VBox(false, 5);
			var DataBox = new Gtk.HBox(false, 5);
			var LeftBox = new Gtk.VBox(false, 5);
			var RightBox = new Gtk.VBox(false, 5);

			LeftBox.Add( MeasuresListView() );
			LeftBox.Add( MeasuresAddView() );

			RightBox.Add( ExercisesListView() );
			RightBox.Add( ExercisesAddView() );

			DataBox.Add(new Gtk.VSeparator());
			DataBox.Add( LeftBox );
			DataBox.Add( new Gtk.VSeparator() );
			DataBox.Add( RightBox) ;
			DataBox.Add(new Gtk.VSeparator());

			MainBox.Add( new Gtk.HSeparator() );
			MainBox.Add( CalendarView() );
			MainBox.Add(new Gtk.HSeparator());
			MainBox.Add( DataBox );
			MainBox.Add(new Gtk.HSeparator());

			this.Add(MainBox);
		}

		private Gtk.VBox CalendarView()
		{
			var ViewBox = new Gtk.VBox(false, 5);

			// Labels.
			var SectionTitle = new Gtk.Label("<b>CALENDARIO</b>");
			SectionTitle.UseMarkup = true;

			// Calendar
			Calendar = new Gtk.Calendar();

			// Calendar events.
			Calendar.DaySelected += (o,args) => CalendarDay();
			Calendar.MonthChanged += (o, args) => CalendarMonth();

			ViewBox.Add(SectionTitle);
			ViewBox.Add(Calendar);

			return ViewBox;
		}

		private Gtk.VBox ExercisesListView()
		{
			var ViewBox = new Gtk.VBox(false, 5);

			// ------------------INICIALIZACION---------------------

			// Labels.
			var SectionTitle = new Gtk.Label("<b>EXERCISES</b>");
			SectionTitle.UseMarkup = true;

			// Tree.
			ExercisesTreeview = new Gtk.TreeView();

			// Tree model.
			ExercisesTreeview.Model =
				new Gtk.ListStore(typeof(string), typeof(string), typeof(string), typeof(string));

			// Tree events
			ExercisesTreeview.ButtonPressEvent += new Gtk.ButtonPressEventHandler(OnRightExercise);

			// Tree columns.
			var NameRender = new Gtk.CellRendererText();
			NameRender.Editable = true;
			NameRender.Edited += ExercisesNameEdit;

			var LoadRender = new Gtk.CellRendererText();
			LoadRender.Editable = true;
			LoadRender.Edited += ExercisesLoadEdit;

			var MinutesRender = new Gtk.CellRendererText();
			MinutesRender.Editable = true;
			MinutesRender.Edited += ExercisesMinutesEdit;

			// Boxes Creation.
			var TreeScroll = new Gtk.ScrolledWindow();
			var TreeSpace = new Gtk.HBox(false, 5);

			// -------------------COMPOSICION------------------------

			// Tree Columns Append.
			ExercisesTreeview.AppendColumn("Name", NameRender, "text", 0);
			ExercisesTreeview.AppendColumn("Load", LoadRender, "text", 1);
			ExercisesTreeview.AppendColumn("Minutes", MinutesRender, "text", 2);
			ExercisesTreeview.AppendColumn("Date", new Gtk.CellRendererText(), "text", 3);

			// Fill Boxes.
			TreeScroll.Add(ExercisesTreeview);
			TreeSpace.Add(TreeScroll);

			ViewBox.Add(SectionTitle);
			ViewBox.Add(TreeSpace);

			return ViewBox;
		}

		private Gtk.VBox ExercisesAddView()
		{
			var ViewBox = new Gtk.VBox(false, 5);

			// ------------------INICIALIZACION---------------------

			// Entrys.
			ExerciseName = new Gtk.Entry("");
			ExerciseLoad = new Gtk.Entry("");
			ExerciseMinutes = new Gtk.Entry("");

			// Entrys Properties.
			ExerciseName.Alignment = 0.5f;
			ExerciseLoad.Alignment = 0.5f;
			ExerciseMinutes.Alignment = 0.5f;

			// Labels.
			var NameLabel = new Gtk.Label("Name:");
			var LoadLabel = new Gtk.Label("Load:");
			var MinutesLabel = new Gtk.Label("Minutes:");

			// Buttons.
			var SaveButton = new Gtk.Button("ADD");
			SaveButton.Clicked += (o, args) => ExerciseAdd();

			// -------------------COMPOSICION------------------------

			// Fill vboxs.
			ViewBox.Add(NameLabel);
			ViewBox.Add(ExerciseName);
			ViewBox.Add(LoadLabel);
			ViewBox.Add(ExerciseLoad);
			ViewBox.Add(MinutesLabel);
			ViewBox.Add(ExerciseMinutes);

			// Compose main vBox.
			ViewBox.Add(SaveButton);

			// Add main box to the window
			return ViewBox;
		}

		private Gtk.VBox MeasuresListView()
		{
			var ViewBox = new Gtk.VBox(false, 5);

			// ------------------INICIALIZACION---------------------

			// Labels.
			var SectionTitle = new Gtk.Label("<b>MEASURES</b>");
			SectionTitle.UseMarkup = true;

			// Tree.
			MeasuresTreeview = new Gtk.TreeView();

			// Tree model.
			MeasuresTreeview.Model =
				new Gtk.ListStore(typeof(string), typeof(string), typeof(string));

			// Tree events
			MeasuresTreeview.ButtonPressEvent += new Gtk.ButtonPressEventHandler(OnRightMeasure);

			// Tree columns.
			var NameRender = new Gtk.CellRendererText();
			NameRender.Editable = true;
			NameRender.Edited += MeasureNameEdit;

			var ValueRender = new Gtk.CellRendererText();
			ValueRender.Editable = true;
			ValueRender.Edited += MeasureValueEdit;

			// Boxes Creation.
			var TreeScroll = new Gtk.ScrolledWindow();
			var TreeSpace = new Gtk.HBox(false, 5);

			// -------------------COMPOSICION------------------------

			// Tree Columns Append.
			MeasuresTreeview.AppendColumn("Name", NameRender, "text", 0);
			MeasuresTreeview.AppendColumn("Value", ValueRender, "text", 1);
			MeasuresTreeview.AppendColumn("Date", new Gtk.CellRendererText(), "text", 2);

			// Fill Boxes.
			TreeScroll.Add(MeasuresTreeview);
			TreeSpace.Add(TreeScroll);

			ViewBox.Add(SectionTitle);
			ViewBox.Add(TreeSpace);

			return ViewBox;
		}

		private Gtk.VBox MeasuresAddView()
		{
			var ViewBox = new Gtk.VBox(false, 5);

			// ------------------INICIALIZACION---------------------

			// Entrys.
			MeasureName = new Gtk.Entry("");
			MeasureValue = new Gtk.Entry("");

			// Entrys Properties.
			MeasureName.Alignment = 0.5f;
			MeasureValue.Alignment = 0.5f;

			// Labels.
			var NameLabel = new Gtk.Label("Name:");
			var ValueLabel = new Gtk.Label("Value:");

			// Buttons.
			var SaveButton = new Gtk.Button("ADD");
			SaveButton.Clicked += (o, args) => MeasureAdd();

			// -------------------COMPOSICION------------------------

			// Fill vboxs.
			ViewBox.Add(NameLabel);
			ViewBox.Add(MeasureName);
			ViewBox.Add(ValueLabel);
			ViewBox.Add(MeasureValue);

			// Compose main vBox.
			ViewBox.Add(SaveButton);

			// Add main box to the window
			return ViewBox;
		}
	}
}
