using System;
using System.Collections.Generic;
using System.Diagnostics;

public class HillClimb : Search, Search.IAlgorithm {
	public LinkedList<Coordinate> Run(int[,] grid, Coordinate start, Coordinate end, ref byte[,] gridSearchState) {
		Debug.WriteLine($"[INFO] Starting Hill Climbing from ({start.row}, {start.col}) to ({end.row}, {end.col})");

		LinkedList<Coordinate> openSet = new LinkedList<Coordinate>();
		LinkedList<Coordinate> closedSet = new LinkedList<Coordinate>();
		LinkedList<Coordinate> tempSet = new LinkedList<Coordinate>();
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
				Debug.WriteLine("[INFO] Hill Climbing found a path to the end");
				LinkedList<Coordinate> path = ReconstructPath(cameFrom, current);
				WalkPath(path, ref gridSearchState);
				return path;
			}

			closedSet.PushFront(current);
			gridSearchState[current.row, current.col] ^= CLOSED_FLAG;

			tempSet.Clear();

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

				if (!coordScore.ContainsKey(next)) {
					coordScore[next] = Heuristic(next, end);
					tempSet.PushBack(next);
					cameFrom[next] = current;
				}

				RunAnimationFrame(animationDelay);
			}

			tempSet.Sort((a, b) => coordScore[a] - coordScore[b]);
			foreach (Coordinate coord in tempSet) {
				if (!openSet.Contains(coord)) {
					openSet.PushBack(coord);
					gridSearchState[coord.row, coord.col] ^= OPEN_FLAG;
				}
			}

			gridSearchState[current.row, current.col] ^= CHECKING_FLAG;
			RunAnimationFrame(animationDelay);
		}

		Debug.WriteLine("[WARN] Hill Climbing did not find a path to the end");
		return new LinkedList<Coordinate>();
	}
}
