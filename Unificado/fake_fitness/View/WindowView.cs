using System;
using FakeFitness.Core;

namespace FakeFitness.View
{
	public partial class MainWindow : Gtk.Window
	{
		// Caja principal.
		private Gtk.VBox MainBox;
		
		// Variables GTK vista calendario.
		private Gtk.Calendar Calendar;

		// Variables GTK vista ejerecicicios.
		private Gtk.Entry ExerciseDist;
		private Gtk.Entry ExerciseMins;
		private Gtk.TreeView ExercisesTreeview;

		// Variables GTK vista ejerecicicios.
		private Gtk.Entry MeasureWeight;
		private Gtk.Entry MeasureSize;

		
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

		private void Build()
		{

			// ----------------------TAMANHO-----------------------

			SetGeometryHints(
				this, new Gdk.Geometry() { MinWidth = 400, MinHeight = 400 }, Gdk.WindowHints.MinSize);

			// ----------------------EVENTOS-----------------------

			DeleteEvent += (o, args) => OnClose();

			// ----------------------VISTAS------------------------

			MainBox = new Gtk.VBox(false, 5);

			MainBox.Add( new Gtk.HSeparator() );
			MainBox.Add( CalendarView() );
			MainBox.Add( new Gtk.HSeparator() );
			MainBox.Add( MeasureView() );
			MainBox.Add( new Gtk.HSeparator() );
			MainBox.Add( ExercisesListView() );
			MainBox.Add( new Gtk.HSeparator() );
			MainBox.Add( ExercisesAddView() );

			this.Add(MainBox);
		}

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

			// Fill vboxs.
			ViewBox.Add(WeightLabel);
			ViewBox.Add(MeasureWeight);
			ViewBox.Add(SizeLabel);
			ViewBox.Add(MeasureSize);

			// Compose main vBox.
			ViewBox.Add(SaveButton);

			// Add main box to the window
			return ViewBox;
		}
	}
}
