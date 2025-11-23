using System;

public class LinkedList<T> where T : IComparable<T> {
	private Element<T>? _head;

	public LinkedList() {
		_head = null;
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

	public int Count() {
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

	public void PushFront(T data, bool toBack = false) {
		Element<T> newElement = new Element<T>(data);

		if (!toBack) {
			newElement.Next = _head;
			_head = newElement;
		} else {
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
	}

	public void PushBack(T data) {
		PushFront(data, true);
	}

	public void PushSorted(T val) {
		_head = PushSortedPriv(val, _head);
	}

	private Element<T> PushSortedPriv(T val, Element<T> list) {
		// insert to sorted list
		if (list == null) {
			// if the list is empty, create a new element
			return new Element<T>(val);
		} else if (list.Data.CompareTo(val) >= 0) {
			// if passed val is greater or equ, insert before current elem
			Element<T> newElem = new Element<T>(val);
			newElem.Next = list;
			return newElem;
		} else {
			// otherwise, continue traversing the list
			list.Next = PushSortedPriv(val, list.Next);
			return list;
		}
	}

	public void PopFront(bool toBack = false) {
		Element<T>? oldHead = _head;

		if (!toBack) {
			if (oldHead != null) {
				_head = oldHead.Next;
			}
		} else {
			if (oldHead == null) {
				return;
			}

			if (oldHead.Next == null) {
				_head = null;
			} else {
				Element<T>? current = oldHead;
				while (current.Next.Next != null) {
					current = current.Next;
				}
				current.Next = null;
			}
		}
	}

	public void PopBack() {
		PopFront(true);
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
