using System;
using System.Collections.Generic;
using System.Diagnostics;

public static partial class Search {
	private struct AStarScore {
		public int cummulative;
		public int heuristic;
	}

	public static LinkedList<Coordinate> AStar(int[,] grid, Coordinate start, Coordinate end, ref byte[,] gridSearchState) {
		Debug.WriteLine($"[INFO] Starting A* from ({start.row}, {start.col}) to ({end.row}, {end.col})");

		LinkedList<Coordinate> openSet = new LinkedList<Coordinate>();
		LinkedList<Coordinate> closedSet = new LinkedList<Coordinate>();
		Dictionary<Coordinate, Coordinate> cameFrom = new Dictionary<Coordinate, Coordinate>();
		Dictionary<Coordinate, AStarScore> coordScore = new Dictionary<Coordinate, AStarScore>();

		openSet.PushFront(start);
		gridSearchState[start.row, start.col] ^= OPEN_FLAG;

		coordScore[start] = new AStarScore {
			cummulative = 0,
			heuristic = Heuristic(start, end)
		};

		while (openSet.Count > 0) {
			Coordinate current = openSet.PopFront();
			gridSearchState[current.row, current.col] ^= (OPEN_FLAG | CHECKING_FLAG);

			// reconstruct and exit if the end is found
			if (current.Equals(end)) {
				Debug.WriteLine("[INFO] A* found a path to the end");
				LinkedList<Coordinate> path = ReconstructPath(cameFrom, current);
				WalkPath(path, ref gridSearchState);
				return path;
			}

			closedSet.PushFront(current);
			gridSearchState[current.row, current.col] ^= CLOSED_FLAG;

			RunAnimationFrame(100);

			// itterate over the neighbours
			foreach (Coordinate neighbour in neighbours) {
				Coordinate next = new Coordinate {
					row = current.row + neighbour.row,
					col = current.col + neighbour.col
				};

				// skip if target cell is a wall or in closed set
				if (!IsCellEmpty(grid, next) || closedSet.Contains(next)) {
					continue;
				}

				// generate scores for neighbour and add to open set
				int nextCummulative = coordScore[current].cummulative + grid[next.row, next.col];
				int nextHeuristic = Heuristic(next, end);
				if (!coordScore.ContainsKey(next) || nextCummulative < coordScore[next].cummulative) {
					coordScore[next] = new AStarScore {
						cummulative = nextCummulative,
						heuristic = nextHeuristic
					};
					cameFrom[next] = current;

					if (!openSet.Contains(next)) {
						openSet.PushFront(next);
						gridSearchState[next.row, next.col] ^= OPEN_FLAG;
					}
				}

				// sort open set by lowest score
				openSet.Sort((Coordinate a, Coordinate b) => {
					AStarScore scoreA = coordScore[a];
					AStarScore scoreB = coordScore[b];

					int totalA = scoreA.cummulative + scoreA.heuristic;
					int totalB = scoreB.cummulative + scoreB.heuristic;

					return totalA.CompareTo(totalB);
				});

				RunAnimationFrame(100);
			}
			gridSearchState[current.row, current.col] ^= CHECKING_FLAG;
		}

		Debug.WriteLine("[WARN] A* could not find a path to the end");
		return new LinkedList<Coordinate>();
	}
}
