using System;
using System.Diagnostics;

public partial class Test {
	static void IsCellEmpty() {
		GenerateTest("cell is empty", () => {
			int[,] grid = {
				{ 1, 0, 0 },
				{ 1, 0, 1 },
				{ 2, 1, 1 }
			};
			return Search.IsCellEmpty(grid, new Search.Coordinate{ row = 0, col = 0 });
		}, true);

		GenerateTest("cell is blocked", () => {
			int[,] grid = {
				{ 1, 0, 0 },
				{ 1, 0, 1 },
				{ 2, 1, 1 }
			};
			return Search.IsCellEmpty(grid, new Search.Coordinate{ row = 0, col = 1 });
		}, false);

		GenerateTest("cell out of bounds", () => {
			int[,] grid = {
				{ 1, 0, 0 },
				{ 1, 0, 1 },
				{ 2, 1, 1 }
			};
			return Search.IsCellEmpty(grid, new Search.Coordinate{ row = -1, col = 0 });
		}, false);
	}
}
