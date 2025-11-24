using System;

public partial class Search {
	/// <summary>
	/// North, East, South, West
	/// </summary>
	public int[][] neighbours = new int[4][] {
		new int[2] { -1, 0 },
		new int[2] { 0, 1 },
		new int[2] { 1, 0 },
		new int[2] { 0, -1 }
	};

	/// <summary>
	/// Calculate the Manhattan distance between two points
	/// </summary>
	public int Heuristic(int[] start, int[] end) {
		return Math.Abs(start[0] - end[0]) + Math.Abs(start[1] - end[1]);
	}
}
