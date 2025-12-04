using System;
using System.Collections.Generic;
using System.Diagnostics;

public class BestFirst : Search, Search.IAlgorithm {
	public LinkedList<Coordinate> Run(int[,] grid, Coordinate start, Coordinate end, ref byte[,] gridSearchState) {
		return new LinkedList<Coordinate>();
	}
}
