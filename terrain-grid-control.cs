using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

public class TerrainGridControl : Control {
	private int[,] terrainMap;
	private int rows;
	private int cols;

	private (int row, int col) start;
	private (int row, int col) end;

	/// <summary>
	/// 	<list>
	/// 		<item>0: Black</item>
	/// 		<item>1: White</item>
	/// 		<item>2: Green</item>
	/// 		<item>3: Light Blue</item>
	///			<item>Other: Magenta</item>
	/// 	</list>
	/// </summary>
	private readonly Dictionary<int, Color> terrainColours;

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
	}
}
