using System;
using System.Linq;
using System.Text.RegularExpressions;

public static class GridGame {
	static bool gameOver;
	static bool isEditing;

	struct Grid {
		public char[,] grid;
		public int rows;
		public int cols;
		public char gridChar;
	}

	struct Player {
		public int x;
		public int y;
		public char playerChar;
	}

	public static void Run() {
		(Grid grid, Player player) = Init();

		while (!gameOver) {
			(grid, player) = GameLoop(grid, player);
		}

		// exit with a new line
		Console.WriteLine();
	}

	static (Grid, Player) Init() {
		Grid grid = new Grid();
		grid.rows = 15;
		grid.cols = 15;
		grid.grid = new char[grid.rows, grid.cols];
		grid.gridChar = '.';

		Player player = new Player();
		player.x = grid.rows / 2;
		player.y = grid.cols / 2;
		player.playerChar = 'P';

		gameOver = false;
		isEditing = false;

		// fill grid with gridChar
		for (int row = 0; row < grid.rows; row++) {
			for (int col = 0; col < grid.cols; col++) {
				grid.grid[row, col] = grid.gridChar;
			}
		}

		return (grid, player);
	}

	static (Grid, Player) GameLoop(Grid grid, Player player) {
		Console.Clear();
		PrintRoom(grid, player);

		(grid, player) = GetInput(grid, player);
		return (grid, player);
	}

	static void PrintRoom(Grid grid, Player player) {
		int rows = grid.grid.GetLength(0);
		int cols = grid.grid.GetLength(1);

		for (int row = 0; row < rows; row++) {
			for (int col = 0; col < cols; col++) {
				if (player.x == col && player.y == row && !isEditing) {
					Console.Write(player.playerChar + " ");
					continue;
				}
				Console.Write(grid.grid[row, col] + " ");
			}
			Console.WriteLine();
		}
	}

	static (Grid, Player) EditGrid(Grid grid, Player player) {
		bool isValid;
		do {
			Console.Clear();
			PrintRoom(grid, player);
			Console.WriteLine("[command] [x] [y]\n0, 0, bottom left\no: Toggle obsticle\ns: Starting position\ne: Exit position\nLeave blank to cancel");
			string input = Console.ReadLine();
			isValid = IsValidEditCommand(input);
		} while (!isValid);

		return (grid, player);
	}

	static char GetUserKey(string prompt, char[] validKeys) {
		Console.Write(prompt);

		// place cursor between brackets
		Console.Write("[ ]");
		Console.SetCursorPosition(1, Console.CursorTop);

		char input = Console.ReadKey(true).KeyChar;

		// validate input
		if (validKeys.Contains(input)) {
			return input;
		} else {
			Console.WriteLine("\n\nInvalid input");
			return GetUserKey(prompt, validKeys);
		}
	}

	static (Grid, Player) GetInput(Grid grid, Player player) {
		char input;
		if (!isEditing)  {
			input = GetUserKey("Move cursor: wasd\nEdit the grid: e\nQuit: q\n", new char[] {'w', 'a', 's', 'd', 'e', 'q'});
		} else {
			input = GetUserKey("Move player: wasd\nEnter Command: /\nQuit editing: q\n", new char[] {'w', 'a', 's', 'd', '/', 'q'});
		}
		int newX = player.x;
		int newY = player.y;

		switch (input) {
			case 'q':
				if (!isEditing) {
					gameOver = true;
				} else {
					isEditing = false;
				}
				break;
			case 'e':
				isEditing = !isEditing;
				break;
			case '/':
				EditGrid(grid, player);
				break;
			case 'w':
				newY--;
				break;
			case 'a':
				newX--;
				break;
			case 's':
				newY++;
				break;
			case 'd':
				newX++;
				break;
		}

		if (newX >= 0 && newX < grid.rows && newY >= 0 && newY < grid.cols) {
			// move player
			grid.grid[player.y, player.x] = grid.gridChar;
			player.x = newX;
			player.y = newY;
			grid.grid[player.y, player.x] = player.playerChar;
		}

		return (grid, player);
	}

	/// <summary>
	/// Validates a grid edit command against <c>^([OoSsEe]\s\d+\s\d+)?$</c>
	/// such that the first char is either O, S, or E, then two integers, or the entire string is empty
	/// </summary>
	public static bool IsValidEditCommand(string command) {
		string regex = "^([OoSsEe]\\s\\d+\\s\\d+)?$";
		return Regex.IsMatch(command, regex);
	}
}
