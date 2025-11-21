using System;
using System.Windows.Forms;

public partial class TerrainGrid : Form {
	public TerrainGrid() {
		InitializeComponent();

		string[] terrainData = System.IO.File.ReadAllLines("maps/test1Map.txt");

		TerrainGridControl grid = new TerrainGridControl(terrainData);
		grid.Dock = DockStyle.Fill;
		Controls.Add(grid);
		Controls.Add(new LoadMapButton());
	}
}
