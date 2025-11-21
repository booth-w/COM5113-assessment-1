using System;
using System.Diagnostics;

static partial class Test {
	static void Enqueue() {
		GenerateTest("enqueue to empty", () => {
			LinkedQueue<int> queue = new LinkedQueue<int>();
			queue.Enqueue(1);
			return queue.PrintQueue();
		}, "1");

		GenerateTest("enqueue to non-empty", () => {
			LinkedQueue<int> queue = new LinkedQueue<int>();
			queue.Enqueue(1);
			queue.Enqueue(2);
			return queue.PrintQueue();
		}, "1, 2");
	}

	static void Dequeue() {
		GenerateTest("dequeue from empty", () => {
			LinkedQueue<int> queue = new LinkedQueue<int>();
			queue.Dequeue();
			return queue.PrintQueue();
		}, "");

		GenerateTest("dequeue from single element", () => {
			LinkedQueue<int> queue = new LinkedQueue<int>();
			queue.Enqueue(1);
			queue.Dequeue();
			return queue.PrintQueue();
		}, "");

		GenerateTest("dequeue from multiple elements", () => {
			LinkedQueue<int> queue = new LinkedQueue<int>();
			queue.Enqueue(1);
			queue.Enqueue(2);
			queue.Enqueue(3);
			queue.Dequeue();
			return queue.PrintQueue();
		}, "2, 3");
	}

	static void QueueClear() {
		GenerateTest("clear empty queue", () => {
			LinkedQueue<int> queue = new LinkedQueue<int>();
			queue.Clear();
			return queue.PrintQueue();
		}, "");

		GenerateTest("clear non-empty queue", () => {
			LinkedQueue<int> queue = new LinkedQueue<int>();
			queue.Enqueue(1);
			queue.Enqueue(2);
			queue.Clear();
			return queue.PrintQueue();
		}, "");
	}
}
