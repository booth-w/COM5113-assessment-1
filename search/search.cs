using System;
using System.Collections.Generic;

public static partial class Search {
	public struct Coordinate {
		public int row;
		public int col;
	}

	/// <summary>
	/// North, East, South, West
	/// </summary>
	public static int[][] neighbours = new int[4][] {
		new int[2] { -1, 0 },
		new int[2] { 0, 1 },
		new int[2] { 1, 0 },
		new int[2] { 0, -1 }
	};

	/// <summary>
	/// Dictionary of available search algorithms' names and their corresponding functions
	/// </summary>
	public static Dictionary<string, Func<int[,], int[], int[], LinkedList<Coordinate>>> algorithms = new Dictionary<string, Func<int[,], int[], int[], LinkedList<Coordinate>>>() {
		{"A*", AStar},
		{"Dijkstras", Dijkstras},
		{"Breadth-First Search", BreadthFirst},
		{"Depth-First Search", DepthFirst},
		{"Best-First Search", BestFirst},
		{"Hill Climbing", HillClimb}
	};


	/// <summary>
	/// Calculate the Manhattan distance between two points
	/// </summary>
	public static int Heuristic(int[] start, int[] end) {
		return Math.Abs(start[0] - end[0]) + Math.Abs(start[1] - end[1]);
	}
}
