using System;
using System.Windows.Forms;

public partial class Form1 : Form {
	public Form1() {
		InitializeComponent();

		TerrainGridControl terrainGrid = new TerrainGridControl();
		terrainGrid.Dock = DockStyle.Fill;
		Controls.Add(terrainGrid);
	}
}
