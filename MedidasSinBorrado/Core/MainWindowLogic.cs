using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

namespace proyectoDia
{
	public partial class MainWindow
	{
		public MainWindow() : base(Gtk.WindowType.Toplevel)
		{
			this.Build();
		}

		private void OnInit()
		{
			ImportXml();
			ListToModel();
		}

		private void OnClose()
		{
			ExportXml();
			Gtk.Application.Quit();
		}


		private void OnCreate()
		{
			short peso = Convert.ToInt16(this.entryPeso.Text);
			short cadera = Convert.ToInt16(this.entryCadera.Text);
			Medida me = new Medida(peso, cadera);
			ListMedidas.Add(me);
			modelMedidas.AppendValues(me.Peso.ToString(), me.Cadera.ToString(), me.Fecha.ToString());
		}



		private void ImportXml() 
		{	//XML TO LIST
			ListMedidas = new List<Medida>();
			var root = XElement.Load("medidas.xml");
			var childs = from i in root.Elements("Medida") select i.Attributes();

			foreach (var elem in childs)
			{
				var peso = Convert.ToInt16(elem.ElementAt(0).Value);
				var cadera = Convert.ToInt16(elem.ElementAt(1).Value);
				var fecha = Convert.ToDateTime(elem.ElementAt(2).Value);
				ListMedidas.Add(new Medida(peso,cadera,fecha));
			}
		}

		public void ExportXml()
		{
			var xml = new XElement("Medidas",
					from me in ListMedidas
						select new XElement("Medida",
							new XAttribute("Peso", me.Peso),
							new XAttribute("Cadera", me.Cadera),
							new XAttribute("Fecha", me.Fecha)
					));
			

			xml.Save("medidas.xml");
		}


		private void ListToModel()
		{
			modelMedidas.Clear();
			foreach (var i in ListMedidas)
			{
				modelMedidas.AppendValues(i.Peso.ToString(), i.Cadera.ToString(), i.Fecha.ToString());
			}
		}

		private void PesoNameCell_Edited(object o, Gtk.EditedArgs e)
		{
			var dir = new Gtk.TreePath(e.Path);
			var index = dir.Indices[0];
			ListMedidas[index].Peso = Convert.ToInt16(e.NewText);
			ListToModel();
		}

		private void CaderaNameCell_Edited(object o, Gtk.EditedArgs e)
		{
			var dir = new Gtk.TreePath(e.Path);
			var index = dir.Indices[0];
			ListMedidas[index].Cadera = Convert.ToInt16(e.NewText);
			ListToModel();
		}

		private void OnRight(object o, Gtk.ButtonPressEventArgs e)
		{
			if (e.Event.Button == 3) //Identificador de botón derecho
			{
				
			}
		}

		private Gtk.ListStore modelMedidas;
		private List<Medida> ListMedidas;

	}
}


