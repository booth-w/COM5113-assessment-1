using System;
using System.Collections;
using System.Collections.Generic;

public class LinkedList<T> : IEnumerable<T> {
	private Element<T>? _head;

	public LinkedList() {
		_head = null;
	}

	public IEnumerator<T> GetEnumerator() {
		Element<T>? current = _head;

		while (current != null) {
			yield return current.Data;
			current = current.Next;
		}
	}

	IEnumerator IEnumerable.GetEnumerator() {
		return GetEnumerator();
	}

	public string PrintList() {
		Element<T>? current = _head;

		string toPrint = "";
		while (current != null) {
			toPrint += current.Data + ", ";
			current = current.Next;
		}

		// remove last ", "
		if (toPrint.Length >= 2) {
			toPrint = toPrint[..^2];
		}

		return toPrint;
	}

	public void Clear() {
		_head = null;
	}

	public int Count {
		get { return _Count(); }
	}

	private int _Count() {
		int count = 0;
		Element<T>? current = _head;

		while (current != null) {
			count++;
			current = current.Next;
		}

		return count;
	}

	public bool Contains(T data) {
		Element<T>? current = _head;

		while (current != null) {
			if (current.Data.Equals(data)) {
				return true;
			}
			current = current.Next;
		}

		return false;
	}

	public void Copy(Element<T>? newList) {
		// TODO
		if (_head == null) {
			return;
		}

		Element<T>? current = _head;
		while (current != null) {

		}
	}

	public bool GetFront(ref T data) {
		bool listNotEmpty = _head != null;
		if (listNotEmpty) {
			data = _head.Data;
		} else {
			Console.WriteLine("Cannot get front of an empty list");
		}

		return listNotEmpty;
	}

	/// <summary>
	/// Sort the linked list against an optional comparison function
	/// <returns>
	/// true if the list was modified (not already sorted)
	/// </returns>
	/// </summary>
	public bool Sort(Comparison<T>? comparison = null) {
		if (_head == null || _head.Next == null) {
			// list is empty or has one element so already sorted
			return false;
		}

		// if comparer is null, use default comparer
		comparison ??= Comparer<T>.Default.Compare;

		// bubble sort
		bool hasSwapped = false;
		bool swapped;
		do {
			swapped = false;
			Element<T>? current = _head;
			Element<T>? prev = null;

			while (current.Next != null) {
				if (comparison(current.Data, current.Next.Data) > 0) {
					// swap elements
					Element<T> next = current.Next;
					current.Next = next.Next;
					next.Next = current;

					if (prev == null) {
						_head = next;
					} else {
						prev.Next = next;
					}

					prev = next;
					swapped = true;
					hasSwapped = true;
				} else {
					prev = current;
					current = current.Next;
				}
			}
		} while (swapped);

		return hasSwapped;
	}

	public void PushFront(T data) {
		Element<T> newElement = new Element<T>(data);

		newElement.Next = _head;
		_head = newElement;
	}

	public void PushBack(T data) {
		Element<T> newElement = new Element<T>(data);

		if (_head == null) {
			_head = newElement;
		} else {
			Element<T>? current = _head;
			while (current.Next != null) {
				current = current.Next;
			}
			current.Next = newElement;
		}
	}

	public T PopFront() {
		Element<T>? oldHead = _head;

		if (oldHead != null) {
			_head = oldHead.Next;
			return oldHead.Data;
		}

		return default(T);
	}

	public T PopBack() {
		Element<T>? oldHead = _head;

		if (oldHead == null) {
			return default(T);
		}

		if (oldHead.Next == null) {
			_head = null;
			return oldHead.Data;
		} else {
			Element<T>? current = oldHead;
			while (current.Next.Next != null) {
				current = current.Next;
			}
			var toReturn = current.Next.Data;
			current.Next = null;
			return toReturn;
		}
	}

	public void PopFirst(T data) {
		if (_head == null) {
			return;
		}

		if (_head.Data.Equals(data)) {
			_head = _head.Next;
			return;
		}

		Element<T>? current = _head;
		while (current.Next != null) {
			if (current.Next.Data.Equals(data)) {
				current.Next = current.Next.Next;
				return;
			}
			current = current.Next;
		}
	}

	public void PopFirstRecursive(T val) {
		_head = PopFirstRecursivePriv(val, _head);
	}

	private Element<T> PopFirstRecursivePriv(T val, Element<T> list) {
		// remove all occurrences of val from list
		if (list == null) {
			// if list is empty, return null
			return null;
		} else if (list.Data.Equals(val)) {
			// if current elem is val, skip it
			return list.Next;
		} else {
			// otherwise, continue traversing the list
			list.Next = PopFirstRecursivePriv(val, list.Next);
			return list;
		}
	}

	public void PopAllOf(T val) {
		_head = PopAllOfPriv(val, _head);
	}

	private Element<T> PopAllOfPriv(T val, Element<T> list) {
		if (list == null) {
			// if list is empty, return null
			return null;
		} else if (list.Data.Equals(val)) {
			// if current elem is val, remove it
			return PopAllOfPriv(val, list.Next);
		} else {
			// otherwise, continue traversing the list
			list.Next = PopAllOfPriv(val, list.Next);
			return list;
		}
	}
}
