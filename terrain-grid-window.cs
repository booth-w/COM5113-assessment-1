using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

public partial class TerrainGridWindow : Form {
	public TerrainGridControl grid;

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

		SearchDropdown searchDropdown = new SearchDropdown();
		// centre the dropdown vertically
		searchDropdown.Location = new Point(120, (topPanel.Height - searchDropdown.Height) / 2);
		searchDropdown.Size = new Size(150, 30);
		topPanel.Controls.Add(searchDropdown);
	}

	/// <summary>
	/// A button to open a file dialog for loading from a map file
	/// </summary>
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

	/// <summary>
	/// A dropdown menu to select the search algorithm
	/// </summary>
	public class SearchDropdown : ComboBox {
		public SearchDropdown() {
			DropDownStyle = ComboBoxStyle.DropDownList;

			// populate the dropdown with the available search algorithms
			Search.algorithms.Keys.ToList().ForEach(algorithm => Items.Add(algorithm));

			// add onchange event listener
			SelectedIndexChanged += OnChange;
		}

		private void OnChange(object sender, EventArgs e) {
			Debug.WriteLine($"[INFO] Triggered dropdown onchange event");
			var searchAlgorithm = Search.algorithms[(string)SelectedItem];
			TerrainGridWindow parentWindow = (TerrainGridWindow)this.FindForm();
			parentWindow.grid.StartSearch(searchAlgorithm);
		}
	}
}
