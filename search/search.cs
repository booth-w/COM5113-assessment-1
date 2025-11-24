using System;
using System.Collections.Generic;

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
	/// Dictionary of available search algorithms' names and their corresponding functions
	/// </summary>
	public var algorithms = new Dictionary<string, Func<int[,], int[], int[]>>() {
		{"A*", this.AStar},
		{"Dijkstra", this.Dijkstra},
		{"Breadth-First Search", this.BFS},
		{"Depth-First Search", this.DFS},
		{"Best-First Search", this.BestFirstSearch},
		{"Hill Climbing", this.HillClimb}
	};


	/// <summary>
	/// Calculate the Manhattan distance between two points
	/// </summary>
	public int Heuristic(int[] start, int[] end) {
		return Math.Abs(start[0] - end[0]) + Math.Abs(start[1] - end[1]);
	}
}
