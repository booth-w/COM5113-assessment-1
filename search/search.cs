using System;
using System.Diagnostics;
using System.Collections.Generic;

public static partial class Search {
	public struct Coordinate {
		public int row;
		public int col;
	}

	private const byte PATH_FLAG = 0b1000;
	private const byte CHECKING_FLAG = 0b0100;
	private const byte OPEN_FLAG = 0b0010;
	private const byte CLOSED_FLAG = 0b0001;

	/// <summary>
	/// North, East, South, West
	/// </summary>
	public static Coordinate[] neighbours = new Coordinate[] {
		new Coordinate { row = -1, col = 0 },
		new Coordinate { row = 0, col = 1 },
		new Coordinate { row = 1, col = 0 },
		new Coordinate { row = 0, col = -1 }
	};

	public delegate LinkedList<Coordinate> SearchAlgorithm(
    int[,] grid,
    Coordinate start,
    Coordinate end,
		ref byte[,] gridSearchState
	);

	/// <summary>
	/// Dictionary of available search algorithms' names and their corresponding functions
	/// </summary>
	public static Dictionary<string, SearchAlgorithm> algorithms = new Dictionary<string, SearchAlgorithm>() {
		{"A*", AStar},
		{"Dijkstras", Dijkstras},
		{"Breadth-First Search", BreadthFirst},
		{"Depth-First Search", DepthFirst},
		{"Best-First Search", BestFirst},
		{"Hill Climbing", HillClimb}
	};

	/// <summary>
	/// Reconstruct the path from start to end using the cameFrom dictionary
	/// </summary>
	private static LinkedList<Coordinate> ReconstructPath(Dictionary<Coordinate, Coordinate> cameFrom, Coordinate current) {
		LinkedList<Coordinate> finalPath = new LinkedList<Coordinate>();
		finalPath.PushFront(current);

		while (cameFrom.ContainsKey(current)) {
			current = cameFrom[current];
			Debug.WriteLine($"[INFO] {current.row}, {current.col}");
			finalPath.PushFront(current);
		}

		return finalPath;
	}

	public static void WalkPath(LinkedList<Search.Coordinate> path, ref byte[,] gridSearchState) {
		while (path.Count > 0) {
			Search.Coordinate coord = path.PopFront();
			gridSearchState[coord.row, coord.col] ^= Search.PATH_FLAG;

			RunAnimationFrame(100);
		}
	}

	/// <summary>
	/// Calculate the Manhattan distance between two points
	/// </summary>
	public static int Heuristic(Coordinate start, Coordinate end) {
		return Math.Abs(start.row - end.row) + Math.Abs(start.col - end.col);
	}

	public static bool IsCellEmpty(int[,] grid, Coordinate cell) {
		return cell.row >= 0 && cell.row < grid.GetLength(0) && cell.col >= 0 && cell.col < grid.GetLength(1) && grid[cell.row, cell.col] != 0;
	}

	public static void RunAnimationFrame(int delay) {
		var parentForm = System.Windows.Forms.Application.OpenForms[0] as TerrainGridWindow;
		parentForm.grid.Invalidate();
		System.Windows.Forms.Application.DoEvents();
		System.Threading.Thread.Sleep(delay);

	}
}
