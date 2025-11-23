using System;

public class Element<T> {
	private Element<T>? _next;
	public Element<T>? Next {
		get { return _next; }
		set { _next = value; }
	}

	public Element(T data) {
		_data = data;
	}

	private T _data;
	public T Data {
		get { return _data; }
	}
}
