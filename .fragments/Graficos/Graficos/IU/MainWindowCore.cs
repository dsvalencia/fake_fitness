using System;

namespace Graficos {
	public partial class MainWindow {
		private void OnClose() {
			Gtk.Application.Quit();
		}

		private void OnShow()
		{
			this.edText.Buffer.Text = @"";
		}

		private void OnViewDrawing() {
			var dlg = new Drawing( this );

			dlg.Run();
			dlg.Destroy();
		}
	}
}

