using System;
using System.Windows.Forms;

public partial class TerrainGrid : Form {
	public TerrainGrid() {
		InitializeComponent();

		TerrainGridControl grid = new TerrainGridControl();
		grid.Dock = DockStyle.Fill;
		Controls.Add(grid);
	}
}
