using System;
using System.Collections.Generic;

namespace proyectoDia
{
	public partial class MainWindow: Gtk.Window
	{
		private void Build()
		{
			this.vbMain = new Gtk.VBox(false, 5);

			this.BuildActions();
			this.BuildMenu();
			this.BuildTools();

			//Eventos
			this.DeleteEvent += (o, args) => this.OnClose();
			//this.Shown += (sender, e) => this.OnShow();

			//Declaración y posicionamiento
			this.WindowPosition = Gtk.WindowPosition.Center;
			this.Add(this.vbMain);

			this.SetGeometryHints(
				this, new Gdk.Geometry() { MinWidth = 320, MinHeight = 200}, Gdk.WindowHints.MinSize);
			this.SetGeometryHints(
				this, new Gdk.Geometry() { MaxWidth = 320, MaxHeight = 200 }, Gdk.WindowHints.MaxSize);
		}

		private void BuildActions()
		{
			this.actQuit = new Gtk.Action("Salir", "Salir", "Salir", Gtk.Stock.Quit);
			this.actQuit.Activated += (o, evt) => this.OnClose();

			this.actCreateMedidas = new Gtk.Action("CrearMedidas", "Enviar", "Crear Medidas", null);
			this.actCreateMedidas.Activated += (o, evt) => this.OnCreate();

		}

		private void BuildMenu()
		{
			//Declaramos Barra de menú
			var menuBar = new Gtk.MenuBar();

			//Declaramos los Items que formarán parte de la barra de menú
			var miFile = new Gtk.MenuItem("Archivo");
			var menuFile = new Gtk.Menu();		//Declarar Menu contextual desplegable para "Archivo"
			//Asignación de Menus como submenus de los Items
			miFile.Submenu = menuFile;          //Vinculamos menuFile como submenu de "Archivo"
			menuFile.Append(this.actQuit.CreateMenuItem());             //Añadimos como Item a menuFile la acción Quit
			//menuView.Append(miList);

			menuBar.Append(miFile);
			this.vbMain.PackStart(menuBar, false, false, 5);
		}

		private void BuildTools()
		{
			var hbBoxTree = new Gtk.HBox(false, 5);		//hbox treeview
			var hbBoxForm = new Gtk.HBox(false, 5);		//hbox para Campos
			var vbPeso = new Gtk.VBox(false, 5);
			var vbCadera = new Gtk.VBox(false, 5);
			var hbBox2 = new Gtk.HBox(false, 5);
			var vbBoxBut = new Gtk.VBox(false, 5);     //Instanciamos una sección VBox de GTK

			tree = new Gtk.TreeView();
			tree.ButtonPressEvent += new Gtk.ButtonPressEventHandler(OnRight());

			Gtk.TreeViewColumn pesoColumn = new Gtk.TreeViewColumn();
			pesoColumn.Title = "Peso";
			Gtk.TreeViewColumn caderaColumn = new Gtk.TreeViewColumn();
			caderaColumn.Title = "Cadera";
			Gtk.TreeViewColumn fechaColumn = new Gtk.TreeViewColumn();
			fechaColumn.Title = "Fecha";

			tree.AppendColumn(pesoColumn);
			tree.AppendColumn(caderaColumn);
			tree.AppendColumn(fechaColumn);

			modelMedidas = new Gtk.ListStore(typeof(string), typeof(string), typeof(string));

			tree.Model = modelMedidas;

			Gtk.CellRendererText pesoNameCell = new Gtk.CellRendererText();
			pesoColumn.PackStart(pesoNameCell, true);

			Gtk.CellRendererText caderaNameCell = new Gtk.CellRendererText();
			caderaColumn.PackStart(caderaNameCell, true);

			Gtk.CellRendererText fechaNameCell = new Gtk.CellRendererText();
			fechaColumn.PackStart(fechaNameCell, true);

			pesoColumn.AddAttribute(pesoNameCell, "text", 0);
			caderaColumn.AddAttribute(caderaNameCell, "text", 1);
			fechaColumn.AddAttribute(fechaNameCell, "text", 2);

			pesoNameCell.Editable = true;
			pesoNameCell.Edited += PesoNameCell_Edited;
			caderaNameCell.Editable = true;
			caderaNameCell.Edited += CaderaNameCell_Edited;

			vbPeso.Add(new Gtk.Label("Peso:"));
			entryPeso = new Gtk.Entry();
			vbPeso.Add(entryPeso);

			vbCadera.Add(new Gtk.Label("Cadera"));
			entryCadera = new Gtk.Entry();
			vbCadera.Add(entryCadera);

			hbBoxForm.PackStart(vbPeso, false, false, 5);
			hbBoxForm.PackStart(vbCadera, false, false, 5);

			var btNewMedidas = new Gtk.Button("Crear");
			btNewMedidas.Clicked += (sender, e) => this.OnCreate();

			// Packing everything
			scroll = new Gtk.ScrolledWindow();
			scroll.Add(tree);
			scroll.SetSizeRequest(200, 200);

			vbBoxBut.PackStart(btNewMedidas, false, false, 5);
			hbBox2.PackStart(vbBoxBut, false, false, 5);
			this.vbMain.PackStart(scroll, true, true, 5);
			this.vbMain.PackStart(hbBoxForm, true, true, 5);
			this.vbMain.PackStart(hbBox2, true, true, 5);

			OnInit();
		}

		private Gtk.Action actQuit;
		private Gtk.Action actCreateMedidas;

		private Gtk.VBox vbMain;
		//private Gtk.TextView edText;
		private Gtk.Entry entryPeso;
		private Gtk.Entry entryCadera;
		private Gtk.TreeView tree;
		private Gtk.ScrolledWindow scroll;

	}
}
