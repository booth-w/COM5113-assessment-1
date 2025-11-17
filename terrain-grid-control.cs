using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

public class TerrainGridControl : Control {
	private int[,] terrainMap;
	private readonly Dictionary<int, Color> terrainColours;

	/// <summary>
	/// Init the terrain and the terrain colours
	/// 	<list>
	/// 		<item>0: Black</item>
	/// 		<item>1: White</item>
	/// 		<item>2: Green</item>
	/// 		<item>3: Light Blue</item>
	/// 	</list>
	/// </summary>
	public TerrainGridControl() {
		terrainColours = new Dictionary<int, Color> {
			{0, Color.Black},
			{1, Color.White},
			{2, Color.Green},
			{3, Color.LightBlue},
		};

		terrainMap = new int[,] {
			{0, 1, 2, 3},
			{1, 2, 3, 0},
			{2, 3, 0, 1},
			{3, 0, 1, 2}
		};
	}

	/// <summary>
	/// Display the terrain map
	/// </summary>
	protected override void OnPaint(PaintEventArgs e) {
		base.OnPaint(e);

		if (terrainMap == null) {
			return;
		}

		int rows = terrainMap.GetLength(0);
		int cols = terrainMap.GetLength(1);
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
					int terrainValue = terrainMap[y, x];
					Color color;

					if (!terrainColours.TryGetValue(terrainValue, out color)) {
						color = Color.Magenta;
					}

					int px = x * cellSize;
					int py = y * cellSize;
					var rect = new Rectangle(px, py, cellSize, cellSize);

					using (var brush = new SolidBrush(color)) {
						e.Graphics.FillRectangle(brush, rect);
					}
					e.Graphics.DrawRectangle(pen, rect);
				}
			}
		}
	}
}
