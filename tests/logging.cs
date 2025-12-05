using System;
using System.Collections.Generic;
using System.Diagnostics;

public class Logging : TraceListener
{
	private void Colourise(string message)
	{
		Dictionary<string, ConsoleColor> levelColours = new Dictionary<string, ConsoleColor>()
		{
			{"[WARN]", ConsoleColor.Yellow},
			{"[ERROR]", ConsoleColor.Red}
		};

		foreach (var colour in levelColours)
		{
			if (message.StartsWith(colour.Key))
			{
				Console.ForegroundColor = colour.Value;
				return;
			}
		}
	}

	public override void Write(string message)
	{
		Colourise(message);
		Console.Write($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}");
		Console.ResetColor();
	}

	public override void WriteLine(string message)
	{
		Colourise(message);
		Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}");
		Console.ResetColor();
	}

	public override void Fail(string message)
	{
		Colourise("[ERROR]");
		Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [ERROR] {message}");
		Console.ResetColor();
	}
}
