using System;
using System.Diagnostics;

static partial class Test {
	static public void Init() {
		Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
	}

	static public void Run() {
		PushFront();
		PushBack();
		PushSorted();
		PopFront();
		PopBack();
		PopFirst();
		PopFirstRecursive();
		PopAllOf();
		Count();
		Contains();
		Enqueue();
		Dequeue();
		QueueClear();
		CommandValidityChecker();
	}

	static private void GenerateTest<T>(string name, Func<T> testMethod, T expected) {
		Debug.WriteLine(name);
		T output = testMethod();
		Debug.Assert(output.Equals(expected), $"{name}. found: {output}, expected: {expected}");
	}
}
