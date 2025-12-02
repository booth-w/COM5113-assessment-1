using System;
using System.Diagnostics;
using System.Collections.Generic;

public static partial class Search {
	public static LinkedList<Coordinate> BreadthFirst(int[,] grid, Coordinate start, Coordinate end, ref byte[,] gridSearchState) {
		Debug.WriteLine($"[INFO] Starting Breadth-first from ({start.row}, {start.col}) to ({end.row}, {end.col})");
		LinkedList<Coordinate> openSet = new LinkedList<Coordinate>();
		LinkedList<Coordinate> closedSet = new LinkedList<Coordinate>();
		Dictionary<Coordinate, Coordinate> cameFrom = new Dictionary<Coordinate, Coordinate>();

		openSet.PushFront(start);

		while (openSet.Count > 0) {
			Coordinate current = openSet.PopFront();
			foreach (Coordinate neighbour in neighbours) {
				Coordinate next = new Coordinate {
					row = current.row + neighbour.row,
					col = current.col + neighbour.col
				};

				if (!IsCellEmpty(grid, next) || closedSet.Contains(next)) {
					continue;
				}

				openSet.PushFront(next);
				cameFrom[next] = current;
			}
			closedSet.PushFront(current);
		}

		return new LinkedList<Coordinate>();
	}
}
