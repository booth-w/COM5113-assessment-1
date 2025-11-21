using System;
using System.Diagnostics;

static partial class Test {
	static void PushFront() {
		GenerateTest("push front to empty", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushFront(1);
			return list.PrintList();
		}, "1");

		GenerateTest("push front to non-empty", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushFront(1);
			list.PushFront(2);
			return list.PrintList();
		}, "2, 1");
	}

	static void PushBack() {
		GenerateTest("push back to empty", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushBack(1);
			return list.PrintList();
		}, "1");

		GenerateTest("push back to non-empty", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushBack(1);
			list.PushBack(2);
			return list.PrintList();
		}, "1, 2");
	}

	static void PushSorted() {
		GenerateTest("push sorted to empty", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushSorted(2);
			return list.PrintList();
		}, "2");

		GenerateTest("push sorted to front", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushSorted(2);
			list.PushSorted(1);
			return list.PrintList();
		}, "1, 2");

		GenerateTest("push sorted to back", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushSorted(1);
			list.PushSorted(2);
			list.PushSorted(4);
			return list.PrintList();
		}, "1, 2, 4");

		GenerateTest("push sorted to middle", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushSorted(1);
			list.PushSorted(2);
			list.PushSorted(4);
			list.PushSorted(3);
			return list.PrintList();
		}, "1, 2, 3, 4");
	}

	static void PopFront() {
		GenerateTest("pop front from empty", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PopFront();
			return list.PrintList();
		}, "");

		GenerateTest("pop front from non-empty", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushFront(1);
			list.PushFront(2);
			list.PopFront();
			return list.PrintList();
		}, "1");

		GenerateTest("pop front last element", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushFront(1);
			list.PopFront();
			return list.PrintList();
		}, "");
	}

	static void PopBack() {
		GenerateTest("pop back from empty", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PopBack();
			return list.PrintList();
		}, "");

		GenerateTest("pop back from non-empty", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushBack(1);
			list.PushBack(2);
			list.PopBack();
			return list.PrintList();
			}, "1");

		GenerateTest("pop back last element", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushBack(1);
			list.PopBack();
			return list.PrintList();
		}, "");
	}

	static void PopFirst() {
		GenerateTest("pop first from empty", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PopFirst(1);
			return list.PrintList();
		}, "");

		GenerateTest("pop first does not exist", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushFront(1);
			list.PushFront(2);
			list.PushFront(3);
			list.PopFirst(4);
			return list.PrintList();
		}, "3, 2, 1");

		GenerateTest("pop first only element", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushFront(1);
			list.PopFirst(1);
			return list.PrintList();
		}, "");

		GenerateTest("pop first middle", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushFront(1);
			list.PushFront(2);
			list.PushFront(3);
			list.PopFirst(2);
			return list.PrintList();
		}, "3, 1");

		GenerateTest("pop first head", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushFront(1);
			list.PushFront(2);
			list.PushFront(3);
			list.PopFirst(3);
			return list.PrintList();
		}, "2, 1");

		GenerateTest("pop first tail", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushFront(1);
			list.PushFront(2);
			list.PushFront(3);
			list.PopFirst(1);
			return list.PrintList();
		}, "3, 2");

		GenerateTest("pop first multiple", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushFront(2);
			list.PushFront(1);
			list.PushFront(2);
			list.PushFront(3);
			list.PopFirst(2);
			return list.PrintList();
		}, "3, 1, 2");

		GenerateTest("pop first last", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushFront(1);
			list.PopFirst(1);
			return list.PrintList();
		}, "");
	}

	static void PopFirstRecursive() {
		GenerateTest("pop first recursive from empty", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PopFirst(1);
			return list.PrintList();
		}, "");

		GenerateTest("pop first recursive does not exist", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushFront(1);
			list.PushFront(2);
			list.PushFront(3);
			list.PopFirst(4);
			return list.PrintList();
		}, "3, 2, 1");

		GenerateTest("pop first recursive only element", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushFront(1);
			list.PopFirst(1);
			return list.PrintList();
		}, "");

		GenerateTest("pop first recursive middle", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushFront(1);
			list.PushFront(2);
			list.PushFront(3);
			list.PopFirst(2);
			return list.PrintList();
		}, "3, 1");

		GenerateTest("pop first recursive head", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushFront(1);
			list.PushFront(2);
			list.PushFront(3);
			list.PopFirst(3);
			return list.PrintList();
		}, "2, 1");

		GenerateTest("pop first recursive tail", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushFront(1);
			list.PushFront(2);
			list.PushFront(3);
			list.PopFirst(1);
			return list.PrintList();
		}, "3, 2");

		GenerateTest("pop first recursive multiple", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushFront(2);
			list.PushFront(1);
			list.PushFront(2);
			list.PushFront(3);
			list.PopFirst(2);
			return list.PrintList();
		}, "3, 1, 2");

		GenerateTest("pop first recursive last", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushFront(1);
			list.PopFirst(1);
			return list.PrintList();
		}, "");
	}

	static void PopAllOf() {
		GenerateTest("pop all of from empty", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PopAllOf(1);
			return list.PrintList();
		}, "");

		GenerateTest("pop all of none exist", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushBack(1);
			list.PushBack(2);
			list.PushBack(3);
			list.PopAllOf(4);
			return list.PrintList();
		}, "1, 2, 3");

		GenerateTest("pop all of some exist", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushBack(3);
			list.PushBack(2);
			list.PushBack(4);
			list.PushBack(2);
			list.PopAllOf(2);
			return list.PrintList();
		}, "3, 4");

		GenerateTest("pop all of all exist", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushBack(2);
			list.PushBack(2);
			list.PushBack(2);
			list.PopAllOf(2);
			return list.PrintList();
		}, "");
	}

	static void Count() {
		GenerateTest("count 0", () => {
			LinkedList<int> list = new LinkedList<int>();
			return list.Count();
		}, 0);

		GenerateTest("count 1", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushFront(1);
			return list.Count();
		}, 1);

		GenerateTest("count 3", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushFront(1);
			list.PushFront(2);
			list.PushFront(3);
			return list.Count();
		}, 3);
	}

	static void Contains() {
		GenerateTest("contains in empty", () => {
			LinkedList<int> list = new LinkedList<int>();
			return list.Contains(1);
		}, false);

		GenerateTest("contains existing", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushFront(1);
			list.PushFront(2);
			list.PushFront(3);
			return list.Contains(2);
		}, true);

		GenerateTest("contains non-existing", () => {
			LinkedList<int> list = new LinkedList<int>();
			list.PushFront(1);
			list.PushFront(2);
			list.PushFront(3);
			return list.Contains(4);
		}, false);
	}
}
