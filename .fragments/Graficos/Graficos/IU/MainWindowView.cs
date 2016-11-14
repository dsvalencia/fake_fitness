using System;

namespace Graficos {
	public partial class MainWindow: Gtk.Window {
		public MainWindow()
			:base( Gtk.WindowType.Toplevel )
		{
			this.Build();
		}

		private void Build() {
			this.vbMain = new Gtk.VBox( false, 5 );

			this.BuildActions();
			this.BuildTools();

			// Events
			this.DeleteEvent += (o, args) => this.OnClose();
			this.Shown += (sender, e) => this.OnShow();

			// Polish
			this.WindowPosition = Gtk.WindowPosition.Center;
			this.Add( this.vbMain );
			this.Resize( 640, 480 );

			this.SetGeometryHints(
				this,
				new Gdk.Geometry() {
					MinWidth = 320,
					MinHeight = 200
				},
				Gdk.WindowHints.MinSize
			); 
		}

		private void BuildActions() {


			this.actViewDrawing = new Gtk.Action( "ViewNotebook", "Drawing demo", "View drawing demo", null );
			this.actViewDrawing.Activated += (obj, evt) => this.OnViewDrawing();
		}



		private void BuildTools() {
			var hbBox = new Gtk.HBox( false, 5 );
			var vbBox = new Gtk.VBox( false, 5 );

			// Widgets
			this.edText = new Gtk.TextView();
			this.edText.Editable = false;

		
			var btDrawing = new Gtk.Button( new Gtk.Label( this.actViewDrawing.Label ) );
			btDrawing.Clicked += (sender, e) => this.actViewDrawing.Activate();



			vbBox.PackStart( btDrawing, false, false, 5 );
			hbBox.PackStart( this.edText, true, true, 5 );
			hbBox.PackStart( vbBox, false, false, 5 );
			this.vbMain.PackStart( hbBox, true, true, 5 );
		}

	
		private Gtk.Action actViewDrawing;

		private Gtk.VBox vbMain;
		private Gtk.TextView edText;
	}
}
