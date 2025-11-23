using System;
using System.Drawing;
using System.Windows.Forms;

public partial class TerrainGridWindow : Form {
	private TerrainGridControl grid;

	public TerrainGridWindow() {
		InitializeComponent();

		grid = new TerrainGridControl();
		grid.Dock = DockStyle.Fill;
		Controls.Add(grid);

		Panel topPanel = new Panel();
		topPanel.Dock = DockStyle.Top;
		topPanel.Height = 50;
		Controls.Add(topPanel);

		LoadMapButton loadMapButton = new LoadMapButton(grid);
		loadMapButton.Location = new Point(10, 10);
		loadMapButton.Size = new Size(100, 30);
		topPanel.Controls.Add(loadMapButton);
	}

	public class LoadMapButton : Button {
		public LoadMapButton(TerrainGridControl grid) {
			Text = "Load Map";

			Click += (sender, e) => {
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Title = "Select Map File";
				openFileDialog.Filter = "Map Files (*.txt)|*.txt|All Files (*.*)|*.*";

				if (openFileDialog.ShowDialog() == DialogResult.OK) {
					string filePath = openFileDialog.FileName;
					string[] terrainData = System.IO.File.ReadAllLines(filePath);
					grid.LoadTerrainData(terrainData);
				}
			};
		}
	}
}
