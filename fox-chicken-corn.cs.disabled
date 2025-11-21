using System;
using System.Linq;

public class FCC {
	private enum Item {
		Fox,
		Chicken,
		Corn
	}

	private LinkedList<Item> _northItems = new LinkedList<Item>();
	private LinkedList<Item> _southItems = new LinkedList<Item>();

	private enum Bank {
		North,
		South
	}
	private Bank _farmerPos;

	public FCC() {
		_northItems.PushFront(Item.Corn);
		_northItems.PushFront(Item.Chicken);
		_northItems.PushFront(Item.Fox);
		_farmerPos = Bank.North;
	}

	public void GameLoop() {
		while (!isWon() && !isLost()) {
			Console.Clear();
			DisplayInstructions();
			DisplayBanks();
			char choice = GetUserKey("Choose an item to take:\nf: fox\nc: chicken\no: corn\nn: nothing\n", new char[] { 'f', 'c', 'o', 'n' });
			Item? itemToMove = ChoiceToItem(choice);
			MoveItem(itemToMove);
		}
	}

	public void DisplayInstructions() {
		Console.WriteLine("The farmer can only carry one item with them on the boat at a time");
		Console.WriteLine("The fox cannot be left alone with the chicken");
		Console.WriteLine("The chicken cannot be left alone with the corn");
		Console.WriteLine("Bring all the items to the other side\n");
	}

	public void DisplayBanks() {
		const int padding = 12;
		Console.WriteLine($"{"North Bank: ", padding}{_northItems.PrintList()}");
		Console.WriteLine($"{"South Bank: ", padding}{_southItems.PrintList()}");
		Console.WriteLine($"{"Farmer: ", padding}{_farmerPos}\n");
	}

	private void MoveItem(Item? item) {
		if (_farmerPos == Bank.North) {
			if (item.HasValue && _northItems.Contains(item.Value)) {
				_northItems.PopFirst(item.Value);
				_southItems.PushFront(item.Value);
			}
			_farmerPos = Bank.South;
		} else {
			if (item.HasValue && _southItems.Contains(item.Value)) {
				_southItems.PopFirst(item.Value);
				_northItems.PushFront(item.Value);
			}
			_farmerPos = Bank.North;
		}
	}

	public bool isWon() {
		foreach (Item item in Enum.GetValues(typeof(Item))) {
			if (!_southItems.Contains(item)) {
				return false;
			}
		}
		return _farmerPos == Bank.South;
	}

	public bool isLost() {
		LinkedList<Item> bank = _farmerPos == Bank.North ? _southItems : _northItems;
		bool hasFox = bank.Contains(Item.Fox);
		bool hasChicken = bank.Contains(Item.Chicken);
		bool hasCorn = bank.Contains(Item.Corn);

		if (hasFox && hasChicken) {
			Console.WriteLine("\bThe fox has eaten the chicken");
			return true;
		}
		if (hasChicken && hasCorn) {
			Console.WriteLine("\bThe chicken has eaten the corn");
			return true;
		}
		return false;
	}

	public bool PlayAgain() {
		char choice = GetUserKey("Play again? (y/n)\n", new char[] { 'y', 'n' });
		return choice == 'y';
	}

	private char GetUserKey(string prompt, char[] validKeys) {
		Console.Write(prompt);

		// place cursor between brackets
		Console.Write("[ ]");
		Console.SetCursorPosition(1, Console.CursorTop);

		char input = Console.ReadKey(true).KeyChar;
		Console.SetCursorPosition(0, Console.CursorTop);

		// validate input
		if (validKeys.Contains(input)) {
			return input;
		} else {
			Console.WriteLine("\n\nInvalid input");
			return GetUserKey(prompt, validKeys);
		}
	}

	private Item? ChoiceToItem(char choice) {
		return choice switch {
			'f' => Item.Fox,
			'c' => Item.Chicken,
			'o' => Item.Corn,
			'n' => null,
			_ => null
		};
	}
}
