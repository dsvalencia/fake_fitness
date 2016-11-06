using System;
using FakeFitness.Core;

namespace FakeFitness.View
{
	public partial class MainWindow : Gtk.Window
	{
		private Gtk.Calendar Calendar;
		private Gtk.TreeView ExercisesTreeview;

		private Gtk.Entry ExerciseName;
		private Gtk.Entry ExerciseMeters;
		private Gtk.Entry ExerciseMinutes;
		
		public MainWindow() : base(Gtk.WindowType.Toplevel)
		{
			Build();
			OnInit();
		}

		// Cerrar la aplicacion.
		private void Quit() => Gtk.Application.Quit();

		// Controla cuando se hace doble-click en un ejercicio.
		[GLib.ConnectBeforeAttribute]
		protected void OnRight(object sender, Gtk.ButtonPressEventArgs e)
		{
			if (e.Event.Button == 3) // Botón derecho.
			{
				Gtk.TreePath RowPath;
				if (ExercisesTreeview.GetPathAtPos(Convert.ToInt32(e.Event.X),
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

			MainBox.Add( new Gtk.HSeparator() );
			MainBox.Add( CalendarView() );
			MainBox.Add( ExercisesListView() );
			MainBox.Add( ExercisesAddView() );

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
			ExercisesTreeview.ButtonPressEvent += new Gtk.ButtonPressEventHandler(OnRight);

			// Tree columns.
			var NameRender = new Gtk.CellRendererText();
			NameRender.Editable = true;
			NameRender.Edited += ExercisesNameEdit;

			var MetersRender = new Gtk.CellRendererText();
			MetersRender.Editable = true;
			MetersRender.Edited += ExercisesMetersEdit;

			var MinutesRender = new Gtk.CellRendererText();
			MinutesRender.Editable = true;
			MinutesRender.Edited += ExercisesMinutesEdit;

			var DateRender = new Gtk.CellRendererText();

			// Boxes Creation.
			var TreeScroll = new Gtk.ScrolledWindow();
			var TreeSpace = new Gtk.HBox(false, 5);

			// -------------------COMPOSICION------------------------

			// Tree Columns Append.
			ExercisesTreeview.AppendColumn("Name", NameRender, "text", 0);
			ExercisesTreeview.AppendColumn("Meters", MetersRender, "text", 1);
			ExercisesTreeview.AppendColumn("Minutes", MinutesRender, "text", 2);
			ExercisesTreeview.AppendColumn("Date", DateRender, "text", 3);

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
			ExerciseMeters = new Gtk.Entry("");
			ExerciseMinutes = new Gtk.Entry("");

			// Entrys Properties.
			ExerciseName.Alignment = 0.5f;
			ExerciseMeters.Alignment = 0.5f;
			ExerciseMinutes.Alignment = 0.5f;

			// Labels.
			var NameLabel = new Gtk.Label("Name:");
			var MetersLabel = new Gtk.Label("Meters:");
			var MinutesLabel = new Gtk.Label("Minutes:");

			// Buttons.
			var SaveButton = new Gtk.Button("ADD");
			SaveButton.Clicked += (o, args) => ExerciseAdd();

			// HBoxs.
			var NewItemBlock = new Gtk.HBox(false, 5);

			// VBoxs.
			var Labels = new Gtk.VBox(false, 5);
			var Entrys = new Gtk.VBox(false, 5);

			// -------------------COMPOSICION------------------------

			// Fill vboxs.
			Labels.Add(NameLabel);
			Labels.Add(MetersLabel);
			Labels.Add(MinutesLabel);

			Entrys.Add(ExerciseName);
			Entrys.Add(ExerciseMeters);
			Entrys.Add(ExerciseMinutes);

			// Fill hboxs.
			NewItemBlock.Add(Labels);
			NewItemBlock.Add(Entrys);

			// Compose main vBox.
			ViewBox.Add(NewItemBlock);
			ViewBox.Add(SaveButton);

			// Add main box to the window
			return ViewBox;
		}
	}
}
