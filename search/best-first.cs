using System;
using System.Collections.Generic;
using System.Diagnostics;

public class BestFirst : Search, Search.IAlgorithm {
	public LinkedList<Coordinate> Run(int[,] grid, Coordinate start, Coordinate end, ref byte[,] gridSearchState) {
		Debug.WriteLine($"[INFO] Starting Best-First from ({start.row}, {start.col}) to ({end.row}, {end.col})");

		LinkedList<Coordinate> openSet = new LinkedList<Coordinate>();
		LinkedList<Coordinate> closedSet = new LinkedList<Coordinate>();
		Dictionary<Coordinate, Coordinate> cameFrom = new Dictionary<Coordinate, Coordinate>();
		Dictionary<Coordinate, int> coordScore = new Dictionary<Coordinate, int>();

		openSet.PushFront(start);
		coordScore[start] = Heuristic(start, end);
		gridSearchState[start.row, start.col] ^= OPEN_FLAG;

		while (openSet.Count > 0) {
			Coordinate current = openSet.PopFront();
			gridSearchState[current.row, current.col] ^= (OPEN_FLAG | CHECKING_FLAG);

			// reconstruct and exit if the end is found
			if (current.Equals(end)) {
				Debug.WriteLine("[INFO] Best-First found a path to the end");
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

				// skip if target cell is a wall or in closed set
				if (!IsCellEmpty(grid, next) || closedSet.Contains(next)) {
					continue;
				}

				bool toDraw = !openSet.Contains(next);

				// generate scores for neighbour and add to open set
				int nextHeuristic = Heuristic(next, end);
				if (!coordScore.ContainsKey(next) || nextHeuristic < coordScore[next]) {
					coordScore[next] = nextHeuristic;
					cameFrom[next] = current;

					if (!openSet.Contains(next)) {
						openSet.PushFront(next);
						gridSearchState[next.row, next.col] ^= OPEN_FLAG;
					}
				}

				// sort open set by lowest heuristic first
				openSet.Sort((a, b) => coordScore[a].CompareTo(coordScore[b]));

				if (toDraw) {
					RunAnimationFrame(100);
				}
			}

			gridSearchState[current.row, current.col] ^= CHECKING_FLAG;
		}

		Debug.WriteLine("[WARN] Best-First did not find a path to the end");
		return new LinkedList<Coordinate>();
	}
}
