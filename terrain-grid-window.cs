using System;
using System.Drawing;
using System.Windows.Forms;

public partial class TerrainGridWindow : Form {
	public TerrainGridWindow() {
		InitializeComponent();

		string[] terrainData = System.IO.File.ReadAllLines("maps/test1Map.txt");

		Panel topPanel = new Panel();
		topPanel.Dock = DockStyle.Top;
		topPanel.Height = 50;
		Controls.Add(topPanel);

		LoadMapButton loadMapButton = new LoadMapButton();
		loadMapButton.Location = new Point(10, 10);
		loadMapButton.Size = new Size(100, 30);
		topPanel.Controls.Add(loadMapButton);

		TerrainGridControl grid = new TerrainGridControl(terrainData);
		grid.Dock = DockStyle.Fill;
		Controls.Add(grid);
	}

	public class LoadMapButton : Button {
		public LoadMapButton() {
			Text = "Load Map";

			Click += (sender, e) => {
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Title = "Select Map File";
				openFileDialog.Filter = "Map Files (*.txt)|*.txt|All Files (*.*)|*.*";

				if (openFileDialog.ShowDialog() == DialogResult.OK) {
					string filePath = openFileDialog.FileName;
					Console.WriteLine(filePath);
				}
			};
		}
	}
}
