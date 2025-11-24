using System;

public partial class Search {
	/// <summary>
	/// Calculate the Manhattan distance between two points
	/// </summary>
	public int Heuristic(int[] start, int[] end) {
		return Math.Abs(start[0] - end[0]) + Math.Abs(start[1] - end[1]);
	}
}
