using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

public partial class TerrainGridWindow : Form
{
	public TerrainGridControl grid;

	private bool ignoreDropdownOnChange = false;
	public string? loadedFilePath = null;

	public TerrainGridWindow()
	{
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

		RunAllButton runAllButton = new RunAllButton(grid);
		runAllButton.Location = new Point(280, 10);
		runAllButton.Size = new Size(100, 30);
		topPanel.Controls.Add(runAllButton);
	}

	/// <summary>
	/// A button to open a file dialog for loading from a map file
	/// </summary>
	public class LoadMapButton : Button
	{
		public LoadMapButton(TerrainGridControl grid)
		{
			Text = "Load Map";

			Click += (sender, e) =>
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Title = "Select Map File";
				openFileDialog.Filter = "Map Files (*.txt)|*.txt|All Files (*.*)|*.*";

				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					string filePath = openFileDialog.FileName;
					string[] terrainData = System.IO.File.ReadAllLines(filePath);
					TerrainGridWindow parentWindow = (TerrainGridWindow)this.FindForm();
					string fileName = System.IO.Path.GetFileName(filePath).Split('.')[0];
					parentWindow.loadedFilePath = fileName;
					grid.LoadTerrainData(terrainData);
				}
			};
		}
	}

	/// <summary>
	/// A dropdown menu to select the search algorithm
	/// </summary>
	public class SearchDropdown : ComboBox
	{
		public SearchDropdown()
		{
			DropDownStyle = ComboBoxStyle.DropDownList;

			// populate the dropdown with the available search algorithms
			Search.algorithms.Keys.ToList().ForEach(algorithm => Items.Add(algorithm));

			// add onchange event listener
			SelectedIndexChanged += OnChange;
		}

		private void OnChange(object sender, EventArgs e)
		{
			TerrainGridWindow parentWindow = (TerrainGridWindow)this.FindForm();
			if (parentWindow.ignoreDropdownOnChange)
			{
				Debug.WriteLine($"[WARN] Ignoring dropdown onchange event");
				return;
			}

			Debug.WriteLine($"[INFO] Triggered dropdown onchange event");

			Search.animationDelay = 100;
			var searchAlgorithm = Search.algorithms[(string)SelectedItem];
			parentWindow.grid.StartSearch(searchAlgorithm);
		}
	}

	/// <summary>
	/// A button that runs all search algorithms against all maps
	/// </summary>
	public class RunAllButton : Button
	{
		public RunAllButton(TerrainGridControl grid)
		{
			Text = "Run All";

			Click += (sender, e) =>
			{
				TerrainGridWindow parentWindow = (TerrainGridWindow)this.FindForm();
				foreach (var mapFile in System.IO.Directory.GetFiles("maps", "*.txt"))
				{
					string[] terrainData = System.IO.File.ReadAllLines(mapFile);
					string fileName = System.IO.Path.GetFileName(mapFile).Split('.')[0];
					parentWindow.loadedFilePath = fileName;
					grid.LoadTerrainData(terrainData);

					foreach (var searchAlgorithm in Search.algorithms)
					{
						Debug.WriteLine($"[INFO] Running {searchAlgorithm.Key} on {mapFile}");

						// change the selected item in the dropdown without triggering onchange event
						parentWindow.ignoreDropdownOnChange = true;
						var dropdown = parentWindow.Controls.OfType<Panel>().First().Controls.OfType<SearchDropdown>().First();
						dropdown.SelectedItem = searchAlgorithm.Key;
						parentWindow.ignoreDropdownOnChange = false;

						Search.animationDelay = 20;
						grid.StartSearch(searchAlgorithm.Value);
					}
				}

				// alert done
				MessageBox.Show("Completed running all search algorithms on all maps", "Run All Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

			};
		}
	}
}
