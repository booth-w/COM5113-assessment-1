using System;

public class LinkedQueue<T> where T : IComparable<T> {
	private LinkedList<T> _list = new LinkedList<T>();

	public string PrintQueue() {
		return _list.PrintList();
	}

	public void Clear() {
		_list = new LinkedList<T>();
	}

	public void Enqueue(T data) {
		_list.PushBack(data);
	}

	public T Dequeue() {
		return _list.PopFront();
	}

	public int Length {
		get { return _Length(); }
	}

	private int _Length() {
		return _list.Count;
	}
}
