using System;
using System.Collections.Generic;
using System.Diagnostics;

public class DepthFirst : Search, Search.IAlgorithm {
	public LinkedList<Coordinate> Run(int[,] grid, Coordinate start, Coordinate end, ref byte[,] gridSearchState) {
		Debug.WriteLine($"[INFO] Starting Depth-First from ({start.row}, {start.col}) to ({end.row}, {end.col})");

		LinkedList<Coordinate> openSet = new LinkedList<Coordinate>();
		LinkedList<Coordinate> closedSet = new LinkedList<Coordinate>();
		Dictionary<Coordinate, Coordinate> cameFrom = new Dictionary<Coordinate, Coordinate>();

		openSet.PushFront(start);
		gridSearchState[start.row, start.col] ^= OPEN_FLAG;

		while (openSet.Count > 0) {
			Coordinate current = openSet.PopFront();
			gridSearchState[current.row, current.col] ^= (OPEN_FLAG | CHECKING_FLAG);

			// reconstruct and exit if the end is found
			if (current.Equals(end)) {
				Debug.WriteLine("[INFO] Depth-First found a path to the end");
				LinkedList<Coordinate> path = ReconstructPath(cameFrom, current);
				WalkPath(path, ref gridSearchState);
				return path;
			}

			closedSet.PushFront(current);
			gridSearchState[current.row, current.col] ^= CLOSED_FLAG;

			RunAnimationFrame(100);

			// itterate over the neighbours
			for (int i = neighbours.Length - 1; i >= 0; i--) {
				Coordinate neighbour = neighbours[i];
				Coordinate next = new Coordinate {
					row = current.row + neighbour.row,
					col = current.col + neighbour.col
				};

				// skip if target cell is a wall, in open set, or in closed set
				if (!IsCellEmpty(grid, next) || openSet.Contains(next) || closedSet.Contains(next)) {
					continue;
				}

				openSet.PushFront(next);
				gridSearchState[next.row, next.col] ^= OPEN_FLAG;
				cameFrom[next] = current;

				RunAnimationFrame(100);
			}

			gridSearchState[current.row, current.col] ^= CHECKING_FLAG;
		}

		Debug.WriteLine("[INFO] Depth-First did not find a path to the end");
		return new LinkedList<Coordinate>();
	}
}
