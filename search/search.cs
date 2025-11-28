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
	public static Coordinate[] neighbours = new Coordinate[] {
		new Coordinate { row = -1, col = 0 },
		new Coordinate { row = 0, col = 1 },
		new Coordinate { row = 1, col = 0 },
		new Coordinate { row = 0, col = -1 }
	};

	/// <summary>
	/// Dictionary of available search algorithms' names and their corresponding functions
	/// </summary>
	public static Dictionary<string, Func<int[,], Coordinate, Coordinate, LinkedList<Coordinate>>> algorithms = new Dictionary<string, Func<int[,], Coordinate, Coordinate, LinkedList<Coordinate>>>() {
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
	public static int Heuristic(Coordinate start, Coordinate end) {
		return Math.Abs(start.row - end.row) + Math.Abs(start.col - end.col);
	}
}
