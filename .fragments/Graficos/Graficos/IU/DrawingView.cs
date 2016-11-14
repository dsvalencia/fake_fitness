using System;

namespace Graficos
{
	public partial class Drawing : Gtk.Dialog
	{
		Diary diary;
		public Drawing(Gtk.Window parent)
		{
			this.TransientFor = parent;
			this.Title = "Drawing Fitness Demo";

			this.Build();
			this.ShowAll();

			//Sample Data to Testing
			diary = new Diary();
			diary.AddExercise(new Activity(new DateTime(2016, 4, 12), 123, 14));
			diary.AddExercise(new Activity(new DateTime(2016, 4, 12), 123, 140));
			diary.AddExercise(new Activity(new DateTime(2016, 4, 14), 123, 14));
			diary.AddExercise(new Activity(new DateTime(2016, 4, 2), 123, 14));
			diary.AddExercise(new Activity(new DateTime(2016, 4, 1), 123, 14));
			diary.AddMeasure(new BodyMeasures(new DateTime(2016, 4, 1), 34.12, 0));
			diary.AddMeasure(new BodyMeasures(new DateTime(2016, 4, 12), 33.1, 20));

			this.SetDiario(diary);
			this.SetMes();


		}

		private void Build()
		{
			this.BuildActions();
			var swScroll = new Gtk.ScrolledWindow();
			var hBox = new Gtk.HBox(false, 5);
			var vBox = new Gtk.VBox(false, 5);


			//var hBoxButton = new Gtk.HBox(false, 5); to eliminate
			// Drawing area
			this.drawingArea = new Gtk.DrawingArea();
			this.drawingArea.ExposeEvent += (o, args) => this.OnExposeDrawingArea();

			//Menu 
			var btPeso = new Gtk.Button(new Gtk.Label(this.actPeso.Label));
			var btAbdomen = new Gtk.Button(new Gtk.Label(this.actAbdomen.Label));
			var btEjerciciosTiempo = new Gtk.Button(new Gtk.Label(this.actEjerTime.Label));
			var btEjerciciosActividades = new Gtk.Button(new Gtk.Label(this.actEjerActivities.Label));
			var btEjerciciosDistancia = new Gtk.Button(new Gtk.Label(this.actEjerDistance.Label));

			btPeso.Clicked += (sender, e) => this.actPeso.Activate();
			btAbdomen.Clicked += (sender, e) => this.actAbdomen.Activate();
			btEjerciciosTiempo.Clicked += (sender, e) => this.actEjerTime.Activate();
			btEjerciciosDistancia.Clicked += (sender, e) => this.actEjerDistance.Activate();
			btEjerciciosActividades.Clicked += (sender, e) => this.actEjerActivities.Activate();


			// Layout
			swScroll.AddWithViewport(this.drawingArea);

			vBox.PackStart(btPeso, false, true, 2);
			vBox.PackStart(btAbdomen, false, true, 2);
			vBox.PackStart(btEjerciciosTiempo, false, true, 2);
			vBox.PackStart(btEjerciciosActividades, false, true, 2);
			vBox.PackStart(btEjerciciosDistancia, false, true, 2);
			hBox.PackStart(drawingArea, true, true, 10);
			hBox.PackStart(swScroll, true, true, 5);
			hBox.PackStart(vBox, false, true, 5);
			//this.VBox.PackStart( swScroll, true, true, 5 ); to eliminate
			this.VBox.PackStart(hBox, true, true, 5);
			this.AddButton(Gtk.Stock.Close, Gtk.ResponseType.Close);

			// Polish
			this.WindowPosition = Gtk.WindowPosition.CenterOnParent;


			this.Resize(400, 200);
			this.SetGeometryHints(
			this,
			new Gdk.Geometry()
			{
				MinWidth = 400,
				MinHeight = 200
			},
			Gdk.WindowHints.MinSize
		);
		}

		private void BuildActions()
		{
			actPeso = new Gtk.Action("Peso", "Peso", "Ver peso", null);
			actPeso.Activated += (obj, evt) => this.onPeso();

			actAbdomen = new Gtk.Action("Abdomen", "Circunferencia abdominal", "Ver circunferencia abdominal", null);
			actAbdomen.Activated += (obj, evt) => this.onAbdomen();

			actEjerTime = new Gtk.Action("ViewTimeExercise", "Tiempo Ejercicio", "Ver tiempo ejercicio", null);
			actEjerTime.Activated += (obj, evt) => this.onTimeActivities();


			actEjerActivities = new Gtk.Action("ViewEjerActivities", "Número de Actividades", "Ver numero ejercicios", null);
			actEjerActivities.Activated += (obj, evt) => this.onNumberActivities();


			actEjerDistance = new Gtk.Action("ViewDistanceExercise", "Distancia Ejercicio", "Ver distancia ejercicio", null);
			actEjerDistance.Activated += (obj, evt) => this.onDistanceActivities();
		}


		private Gtk.DrawingArea drawingArea;
		private Gtk.Action actEjerTime;
		private Gtk.Action actAbdomen;
		private Gtk.Action actPeso;
		private Gtk.Action actEjerActivities;
		private Gtk.Action actEjerDistance;




	}
}

