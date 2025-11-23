using System;
namespace Peeper.Utils;

public static class TerminalUI {
    public static void Clear() => Console.Write("\u001b[2J\u001b[H"); // ANSI clear + cursor home
    public static void WriteHeader(string headerText) => Console.WriteLine($"=== {headerText} ===");
    public static void WriteStat(string label, string value) => Console.WriteLine($"{label}: {value}");
    public static void WriteSpacer() => Console.WriteLine("");
}

