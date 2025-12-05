using System;
using System.Collections.Generic;
using System.Diagnostics;

public class Dijkstras : Search, Search.IAlgorithm
{
	public LinkedList<Coordinate> Run(int[,] grid, Coordinate start, Coordinate end, ref byte[,] gridSearchState)
	{
		Debug.WriteLine($"[INFO] Starting Dijkstras from ({start.row}, {start.col}) to ({end.row}, {end.col})");

		LinkedList<Coordinate> openSet = new LinkedList<Coordinate>();
		LinkedList<Coordinate> closedSet = new LinkedList<Coordinate>();
		Dictionary<Coordinate, Coordinate> cameFrom = new Dictionary<Coordinate, Coordinate>();
		Dictionary<Coordinate, int> coordScore = new Dictionary<Coordinate, int>();

		openSet.PushFront(start);
		gridSearchState[start.row, start.col] ^= OPEN_FLAG;

		coordScore[start] = 0;

		while (openSet.Count > 0)
		{
			Coordinate current = openSet.PopFront();
			gridSearchState[current.row, current.col] ^= (OPEN_FLAG | CHECKING_FLAG);

			// reconstruct and exit if the end is found
			if (current.Equals(end))
			{
				Debug.WriteLine("[INFO] Dijkstras found a path to the end");
				LinkedList<Coordinate> path = ReconstructPath(cameFrom, current);
				WalkPath(path, ref gridSearchState);
				return path;
			}

			closedSet.PushFront(current);
			gridSearchState[current.row, current.col] ^= CLOSED_FLAG;

			RunAnimationFrame(animationDelay);

			// itterate over the neighbours
			foreach (Coordinate neighbour in neighbours)
			{
				Coordinate next = new Coordinate
				{
					row = current.row + neighbour.row,
					col = current.col + neighbour.col
				};

				// skip if target cell is a wall or in closed set
				if (!IsCellEmpty(grid, next) || closedSet.Contains(next))
				{
					continue;
				}

				bool toDraw = !openSet.Contains(next);

				// generate scores for neighbour and add to open set
				int nextCummulative = coordScore[current] + grid[next.row, next.col];
				if (!coordScore.ContainsKey(next) || nextCummulative < coordScore[next])
				{
					coordScore[next] = nextCummulative;
					cameFrom[next] = current;

					if (!openSet.Contains(next))
					{
						openSet.PushFront(next);
						gridSearchState[next.row, next.col] ^= OPEN_FLAG;
					}
				}

				// sort open set by lowest score
				openSet.Sort((Coordinate a, Coordinate b) =>
				{
					int scoreA = coordScore[a];
					int scoreB = coordScore[b];

					return scoreA.CompareTo(scoreB);
				});

				if (toDraw)
				{
					RunAnimationFrame(animationDelay);
				}
			}
			gridSearchState[current.row, current.col] ^= CHECKING_FLAG;
		}

		Debug.WriteLine("[WARN] Dijkstras could not find a path to the end");
		return new LinkedList<Coordinate>();
	}
}
