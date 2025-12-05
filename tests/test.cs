using System;
using System.Diagnostics;

static partial class Test
{
	static public void Init()
	{
		Debug.Listeners.Clear();
		Debug.Listeners.Add(new Logging());
	}

	static public void Run()
	{
		PushFront();
		PushBack();
		PopFront();
		PopBack();
		PopFirst();
		PopFirstRecursive();
		PopAllOf();
		Count();
		Contains();
		Sort();
		Enqueue();
		Dequeue();
		QueueClear();
		IsCellEmpty();
		Debug.WriteLine("[INFO] tests complete");
	}

	static private void GenerateTest<T>(string name, Func<T> testMethod, T expected)
	{
		Debug.WriteLine($"[INFO] {name}");
		T output = testMethod();
		Debug.Assert(output.Equals(expected), $"{name}. found: {output}, expected: {expected}");
	}
}
