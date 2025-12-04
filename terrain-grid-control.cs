using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Forms;

public class TerrainGridControl : Control {
	/// <summary>
	/// Weights of the cells. 0 = infinite weight (wall)
	/// </summary>
	private int[,] terrainMap;

	/// <summary>
	/// Bit flag representation of the state of each cell for the animation
	/// 0 0 0 0
	/// | | | |
	/// | | | .__ in closed set (CLOSED_FLAG)
	/// | | .____ in open set (OPEN_FLAG)
	/// | .______ currently checking (CHECKING_FLAG)
	/// .________ final path (PATH_FLAG)
	/// </summary>
	private byte[,] gridSearchState;

	private int rows;
	private int cols;

	private (int row, int col) start;
	private (int row, int col) end;

	/// <summary>
	/// 	<list>
	/// 		<item>0: Black</item>
	/// 		<item>1: White</item>
	/// 		<item>2: Green</item>
	/// 		<item>3: LightBlue</item>
	///			<item>Other: Magenta</item>
	/// 	</list>
	/// </summary>
	private readonly Dictionary<int, Color> terrainColours;

	/// <summary>
	/// 	<list>
	/// 		<item>0b1000: CornflowerBlue (final path)</item>
	/// 		<item>0b0100: Orange (currently checking)</item>
	/// 		<item>0b0010: Khaki (in open set)</item>
	/// 		<item>0b0001: LightGray (in closed set)</item>
	/// 	</list>
	/// </summary>
	private readonly Dictionary<byte, Color> searchStateColours ;

	/// <summary>
	/// Init the terrain and the terrain colours
	/// </summary>
	public TerrainGridControl() {
		this.terrainColours = new Dictionary<int, Color> {
			{0, Color.Black},
			{1, Color.White},
			{2, Color.Green},
			{3, Color.LightBlue},
		};

		this.searchStateColours = new Dictionary<byte, Color> {
			{0b1000, Color.CornflowerBlue},
			{0b0100, Color.Orange},
			{0b0010, Color.Khaki},
			{0b0001, Color.LightGray}
		};
	}

	/// <summary>
	/// Takes the terrain data string array,
	/// checks if it is valid,
	/// parses it into the terrain map,
	/// then triggers a redraw
	/// </summary>
	/// <param name="terrainData">Array of strings representing the terrain data</param>
	public void LoadTerrainData(string[] terrainData) {
		// strip empty lines and trim whitespace
		terrainData = terrainData
			.Where(line => !string.IsNullOrWhiteSpace(line))
			.Select(line => line.Trim())
			.ToArray();

		if (!IsValidTerrainData(terrainData)) {
			Debug.WriteLine("[ERROR] Invalid terrain data");
			MessageBox.Show("Invalid terrain data provided.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return;
		}

		this.rows = int.Parse(terrainData[0].Split(' ')[0]);
		this.cols = int.Parse(terrainData[0].Split(' ')[1]);

		this.start.row = int.Parse(terrainData[1].Split(' ')[0]);
		this.start.col = int.Parse(terrainData[1].Split(' ')[1]);

		this.end.row = int.Parse(terrainData[2].Split(' ')[0]);
		this.end.col = int.Parse(terrainData[2].Split(' ')[1]);

		this.terrainMap = new int[rows, cols];
		this.gridSearchState = new byte[rows, cols];

		for (int row = 0; row < rows; row++) {
			string[] terrainRow = terrainData[row + 3].Split(' ');
			for (int col = 0; col < cols; col++) {
				this.terrainMap[row, col] = int.Parse(terrainRow[col]);
			}
		}

		this.Invalidate();
	}

	/// <summary>
	/// Validate the terrain data format
	/// such that it meets the expected structure of
	/// rows cols
	/// start_row start_col possitioned in the grid and not a wall
	/// end_row end_col possitioned in the grid and not a wall
	/// grid of ints 0 to 3 of size rows x cols
	public bool IsValidTerrainData(string[] terrainData) {
		if (terrainData.Length < 4) {
			Debug.WriteLine("[ERROR] Terrain data has less than 4 lines");
			return false;
		}

		// validate first 3 lines
		for (int row = 0; row < 3; row++) {
			if (!Regex.IsMatch(terrainData[row], @"^\d+\s+\d+$")) {
				Debug.WriteLine($"[ERROR] Terrain data line {row} is not in the expected format. Found: '{terrainData[row]}'");
				return false;
			}
		}

		// validate number of rows
		int expectedRows = int.Parse(terrainData[0].Split(' ')[0]);
		int expectedCols = int.Parse(terrainData[0].Split(' ')[1]);
		if (terrainData.Length != expectedRows + 3) {
			Debug.WriteLine($"[ERROR] Terrain data does not have the expected number of rows. Found: {terrainData.Length - 3}, Expected: {expectedRows}");
			return false;
		}

		// validate terrain grid
		for (int row = 0; row < expectedRows; row++) {
			string[] terrainRow = terrainData[row + 3].Split(' ');

			// validate number of cols
			if (terrainRow.Length != expectedCols) {
				Debug.WriteLine($"[ERROR] Terrain data row {row + 3} does not have the expected number of columns. Found: {terrainRow.Length}, Expected: {expectedCols}");
				return false;
			}

			// validate cell values
			foreach (string cell in terrainRow) {
				if (!Regex.IsMatch(cell, @"^[0-3]$")) {
					Debug.WriteLine($"[ERROR] Terrain data row {row + 3} has an invalid cell value: '{cell}'");
					return false;
				}
			}
		}

		int startRow = int.Parse(terrainData[1].Split(' ')[0]);
		int startCol = int.Parse(terrainData[1].Split(' ')[1]);
		int endRow = int.Parse(terrainData[2].Split(' ')[0]);
		int endCol = int.Parse(terrainData[2].Split(' ')[1]);

		// check start pos is in the grid
		if (startRow < 0 || startRow >= expectedRows || startCol < 0 || startCol >= expectedCols) {
			Debug.WriteLine($"[ERROR] Start position ({startRow}, {startCol}) is out of bounds");
			return false;
		}

		// check end pos is in the grid
		if (endRow < 0 || endRow >= expectedRows || endCol < 0 || endCol >= expectedCols) {
			Debug.WriteLine($"[ERROR] End position ({endRow}, {endCol}) is out of bounds");
			return false;
		}

		// check start pos is not a wall
		if (terrainData[startRow + 3].Split(' ')[startCol] == "0") {
			Debug.WriteLine($"[ERROR] Start position ({startRow}, {startCol}) is a wall");
			return false;
		}

		// check end pos is not a wall
		if (terrainData[endRow + 3].Split(' ')[endCol] == "0") {
			Debug.WriteLine($"[ERROR] End position ({endRow}, {endCol}) is a wall");
			return false;
		}

		Debug.WriteLine("[INFO] Terrain data is valid");
		return true;
	}

	public void StartSearch(Search.IAlgorithm searchAlgorithm) {
		Debug.WriteLine($"[INFO] Selected search algorithm: {searchAlgorithm.GetType().Name}");

		// exit if no terrain map loaded
		if (this.terrainMap == null) {
			Debug.WriteLine("[WARN] No terrain map loaded. Cannot start search");
			return;
		}

		Search.Coordinate startCoord = new Search.Coordinate { row = this.start.row, col = this.start.col };
		Search.Coordinate endCoord = new Search.Coordinate { row = this.end.row, col = this.end.col };
		LinkedList<Search.Coordinate> path = searchAlgorithm.Run(this.terrainMap, startCoord, endCoord, ref this.gridSearchState);
	}


	/// <summary>
	/// Display the terrain map
	/// </summary>
	protected override void OnPaint(PaintEventArgs e) {
		base.OnPaint(e);

		if (this.terrainMap == null) {
			return;
		}

		int rows = this.terrainMap.GetLength(0);
		int cols = this.terrainMap.GetLength(1);
		if (rows == 0 || cols == 0) {
			return;
		}

		int width = ClientSize.Width;
		int height = ClientSize.Height;
		if (width <= 0 || height <= 0) {
			return;
		}

		int cellWidth = width / cols;
		int cellHeight = height / rows;
		int cellSize = Math.Min(cellWidth, cellHeight);
		if (cellSize <= 0) {
			return;
		}

		using (Pen pen = new Pen(Color.Black)) {
			for (int y = 0; y < rows; y++) {
				for (int x = 0; x < cols; x++) {
					int terrainValue = this.terrainMap[y, x];
					Color colour;
					Color stateColour = Color.Transparent;

					if (!this.terrainColours.TryGetValue(terrainValue, out colour)) {
						colour = Color.Magenta;
					}

					// get the colour overlay from the search state
					byte searchState = this.gridSearchState[y, x];

					// most significant bit has highest priority
					foreach (byte state in this.searchStateColours.Keys.OrderByDescending(k => k)) {
						if ((searchState & state) != 0) {
							stateColour = this.searchStateColours[state];
							break;
						}
					}

					int px = x * cellSize;
					int py = y * cellSize;
					Rectangle rect = new Rectangle(px, py, cellSize, cellSize);

					int posOffset = cellSize / 8;
					int sizeOffset = cellSize / 4;
					Rectangle stateRect = new Rectangle(px + posOffset, py + posOffset, cellSize - sizeOffset, cellSize - sizeOffset);

					// fill the base terrain colour
					using (SolidBrush brush = new SolidBrush(colour)) {
						e.Graphics.FillRectangle(brush, rect);
					}

					// overlay the search state colour
					if (stateColour != Color.Transparent) {
						using (Pen statePen = new Pen(stateColour, sizeOffset)) {
							e.Graphics.DrawRectangle(statePen, stateRect);
						}
					}

					// cell border
					e.Graphics.DrawRectangle(pen, rect);
				}
			}
		}

		// start and end markers
		using (Font font = new Font("Arial", cellSize / 2)) {
			using (SolidBrush brush = new SolidBrush(Color.Red)) {
				// start
				int startX = this.start.col * cellSize + cellSize / 4;
				int startY = this.start.row * cellSize + cellSize / 8;
				e.Graphics.DrawString("S", font, brush, startX, startY);

				// end
				int endX = this.end.col * cellSize + cellSize / 4;
				int endY = this.end.row * cellSize + cellSize / 8;
				e.Graphics.DrawString("E", font, brush, endX, endY);
			}
		}
	}
}
