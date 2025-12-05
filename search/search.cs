using System;
using System.Diagnostics;
using System.Collections.Generic;

public abstract class Search
{
	public interface IAlgorithm
	{
		LinkedList<Coordinate> Run(
			int[,] grid,
			Coordinate start,
			Coordinate end,
			ref byte[,] gridSearchState
		);
	}

	public struct Coordinate
	{
		public int row;
		public int col;
	}

	protected const byte PATH_FLAG = 0b1000;
	protected const byte CHECKING_FLAG = 0b0100;
	protected const byte OPEN_FLAG = 0b0010;
	protected const byte CLOSED_FLAG = 0b0001;

	public static int animationDelay = 100;

	/// <summary>
	/// North, East, South, West
	/// </summary>
	public static Coordinate[] neighbours = new Coordinate[]
	{
		new Coordinate { row = -1, col = 0 },
		new Coordinate { row = 0, col = 1 },
		new Coordinate { row = 1, col = 0 },
		new Coordinate { row = 0, col = -1 }
	};

	/// <summary>
	/// Dictionary of available search algorithms' names and their corresponding functions
	/// </summary>
	public static Dictionary<string, IAlgorithm> algorithms = new Dictionary<string, IAlgorithm>()
	{
		{"A*", new AStar()},
		{"Dijkstras", new Dijkstras()},
		{"Breadth-First Search", new BreadthFirst()},
		{"Depth-First Search", new DepthFirst()},
		{"Best-First Search", new BestFirst()},
		{"Hill Climbing", new HillClimb()}
	};

	/// <summary>
	/// Reconstruct the path from start to end using the cameFrom dictionary
	/// </summary>
	protected static LinkedList<Coordinate> ReconstructPath(Dictionary<Coordinate, Coordinate> cameFrom, Coordinate current)
	{
		LinkedList<Coordinate> finalPath = new LinkedList<Coordinate>();
		finalPath.PushFront(current);

		while (cameFrom.ContainsKey(current))
		{
			current = cameFrom[current];
			Debug.WriteLine($"[INFO] {current.row}, {current.col}");
			finalPath.PushFront(current);
		}

		return finalPath;
	}

	protected static void WalkPath(LinkedList<Search.Coordinate> path, ref byte[,] gridSearchState)
	{
		foreach (Coordinate step in path)
		{
			gridSearchState[step.row, step.col] ^= PATH_FLAG;
			RunAnimationFrame(100);
		}
	}

	/// <summary>
	/// Calculate the Manhattan distance between two points
	/// </summary>
	protected static int Heuristic(Coordinate start, Coordinate end)
	{
		return Math.Abs(start.row - end.row) + Math.Abs(start.col - end.col);
	}

	public static bool IsCellEmpty(int[,] grid, Coordinate cell)
	{
		return cell.row >= 0 && cell.row < grid.GetLength(0) && cell.col >= 0 && cell.col < grid.GetLength(1) && grid[cell.row, cell.col] != 0;
	}

	public static void RunAnimationFrame(int delay)
	{
		TerrainGridWindow parentForm = System.Windows.Forms.Application.OpenForms[0] as TerrainGridWindow;
		parentForm.grid.Invalidate();
		System.Windows.Forms.Application.DoEvents();
		System.Threading.Thread.Sleep(delay);

	}
}
