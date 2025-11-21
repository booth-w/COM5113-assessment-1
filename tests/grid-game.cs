using System;
using System.Diagnostics;

partial class Test {
	static void CommandValidityChecker() {
		GenerateTest("command valid", () => {
			return GridGame.IsValidEditCommand("O 1 2");
		}, true);

		GenerateTest("command valid double digit", () => {
			return GridGame.IsValidEditCommand("O 10 20");
		}, true);

		GenerateTest("command invalid letter", () => {
			return GridGame.IsValidEditCommand("r 1 2");
		}, false);

		GenerateTest("command invalid multiple letters", () => {
			return GridGame.IsValidEditCommand("OE 1 2");
		}, false);

		GenerateTest("command invalid number", () => {
			return GridGame.IsValidEditCommand("O 12");
		}, false);

		GenerateTest("command empty", () => {
			return GridGame.IsValidEditCommand("");
		}, true);
	}
}
