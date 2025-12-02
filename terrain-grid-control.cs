using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

public class TerrainGridControl : Control {
	/// <summary>
	/// Weights of the cells. 0 = infinite weight (wall)
	/// </summary>
	private int[,] terrainMap;

	/// <summary>
	/// Bit flag representation of the state of each cell for the animation
	/// 0 0 0 0 0
	/// | | | | |
	/// | | | | .__ walked so far in final animation (WALKED_FLAG)
	/// | | | .____ final path (PATH_FLAG)
	/// | | .______ in closed set (CLOSED_FLAG)
	/// | .________ in open set (OPEN_FLAG)
	/// .__________ currently checking (CHECKING_FLAG)
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
		if (!IsValidTerrainData(terrainData)) {
			throw new ArgumentException("Invalid terrain data");
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

	private bool IsValidTerrainData(string[] terrainData) {
		return true;
	}

	public void StartSearch(Search.SearchAlgorithm searchAlgorithm) {
		Debug.WriteLine($"[INFO] Selected search algorithm: {searchAlgorithm.Method.Name}");

		// exit if no terrain map loaded
		if (this.terrainMap == null) {
			Debug.WriteLine("[WARN] No terrain map loaded. Cannot start search");
			return;
		}

		Search.Coordinate startCoord = new Search.Coordinate { row = this.start.row, col = this.start.col };
		Search.Coordinate endCoord = new Search.Coordinate { row = this.end.row, col = this.end.col };
		LinkedList<Search.Coordinate> path = searchAlgorithm(this.terrainMap, startCoord, endCoord, ref this.gridSearchState);
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

		using (var pen = new Pen(Color.Black)) {
			for (int y = 0; y < rows; y++) {
				for (int x = 0; x < cols; x++) {
					int terrainValue = this.terrainMap[y, x];
					Color colour;

					if (!this.terrainColours.TryGetValue(terrainValue, out colour)) {
						colour = Color.Magenta;
					}

					// get the colour overlay from the search state
					byte searchState = this.gridSearchState[y, x];
					// Debug.WriteLine($"[INFO] Cell ({x}, {y}) search state: {Convert.ToString(searchState, 2).PadLeft(5, '0')}");
					// most significant bit has highest priority
					foreach (var state in this.searchStateColours.Keys.OrderByDescending(k => k)) {
						if ((searchState & state) != 0) {
							colour = this.searchStateColours[state];
							break;
						}
					}

					int px = x * cellSize;
					int py = y * cellSize;
					var rect = new Rectangle(px, py, cellSize, cellSize);

					using (var brush = new SolidBrush(colour)) {
						e.Graphics.FillRectangle(brush, rect);
					}
					e.Graphics.DrawRectangle(pen, rect);
				}
			}
		}

		// start and end markers
		using (var font = new Font("Arial", cellSize / 2)) {
			using (var brush = new SolidBrush(Color.Red)) {
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
