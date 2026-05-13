using System.Text;
using RockPaperScissors.Core.Models;
using static System.Console;

namespace RockPaperScissors.Console;

internal static class Program
{
    private static void Main()
    {
        OutputEncoding = Encoding.UTF8;

        var targetWins = SelectMode();
        if (targetWins is null) return;

        var session = new GameSession();
        PlayLoop(session, targetWins.Value);
        ShowSummary(session, targetWins.Value);
    }

    private static int? SelectMode()
    {
        while (true)
        {
            SafeClear();
            WriteLine("=== Rock, Paper, Scissors ===\n");
            WriteLine("Choose mode:");
            WriteLine("  1) Endless (play until you quit)");
            WriteLine("  2) Best of 5 (first to 3 wins)");
            WriteLine("  3) Best of 7 (first to 4 wins)");
            WriteLine("  4) Quit\n");
            Write("Choice: ");

            switch (ReadLine()?.Trim())
            {
                case "1": return int.MaxValue;
                case "2": return 3;
                case "3": return 4;
                case "4": return null;
                default:
                    WriteWithColor("Invalid choice. Try again.", ConsoleColor.Yellow);
                    WriteLine("\nPress any key to continue...");
                    ReadKey(intercept: true);
                    break;
            }
        }
    }

    private static void PlayLoop(GameSession session, int targetWins)
    {
        while (session.PlayerScore < targetWins && session.ComputerScore < targetWins)
        {
            SafeClear();
            ShowScoreboard(session, targetWins);
            WriteLine("\n  1) Rock\n  2) Paper\n  3) Scissors\n  4) Quit\n");
            Write("Choice: ");

            var input = ReadLine()?.Trim();
            if (input == "4") return;

            if (!TryParseChoice(input, out var playerChoice))
            {
                WriteWithColor("Invalid choice — use 1, 2, 3 or 4.", ConsoleColor.Yellow);
                WriteLine("\nPress any key to continue...");
                ReadKey(intercept: true);
                continue;
            }

            var round = session.PlayRound(playerChoice);
            ShowRoundResult(round);

            WriteLine("\nPress any key to continue...");
            ReadKey(intercept: true);
        }
    }

    private static bool TryParseChoice(string? input, out Choice choice)
    {
        switch (input)
        {
            case "1": choice = Choice.Rock; return true;
            case "2": choice = Choice.Paper; return true;
            case "3": choice = Choice.Scissors; return true;
            default: choice = default; return false;
        }
    }

    private static void ShowScoreboard(GameSession session, int targetWins)
    {
        var target = targetWins == int.MaxValue ? "endless" : $"first to {targetWins}";
        WriteLine($"=== Rock, Paper, Scissors ({target}) ===");
        WriteLine($"Rounds: {session.RoundsPlayed}   You: {session.PlayerScore}   PC: {session.ComputerScore}   Ties: {session.Ties}   Win rate: {session.WinRate:0.#}%");
    }

    private static void ShowRoundResult(Round round)
    {
        WriteLine($"\nYou chose {ChoiceLabel(round.Player)}, PC chose {ChoiceLabel(round.Computer)}.");

        switch (round.Outcome)
        {
            case Outcome.PlayerWin:
                WriteWithColor("You win!", ConsoleColor.Green);
                break;
            case Outcome.ComputerWin:
                WriteWithColor("You lose!", ConsoleColor.Red);
                break;
            case Outcome.Tie:
                WriteWithColor("Tie!", ConsoleColor.Yellow);
                break;
        }
    }

    private static void ShowSummary(GameSession session, int targetWins)
    {
        SafeClear();
        WriteLine("=== Final summary ===\n");

        if (session.RoundsPlayed == 0)
        {
            WriteLine("No rounds played.");
            return;
        }

        if (targetWins != int.MaxValue)
        {
            if (session.PlayerScore >= targetWins)
            {
                WriteWithColor($"You won the match {session.PlayerScore}–{session.ComputerScore}!", ConsoleColor.Green);
            }
            else if (session.ComputerScore >= targetWins)
            {
                WriteWithColor($"PC won the match {session.ComputerScore}–{session.PlayerScore}.", ConsoleColor.Red);
            }
        }

        WriteLine();
        WriteLine($"Rounds played: {session.RoundsPlayed}");
        WriteLine($"Wins:          {session.PlayerScore}");
        WriteLine($"Losses:        {session.ComputerScore}");
        WriteLine($"Ties:          {session.Ties}");
        WriteLine($"Win rate:      {session.WinRate:0.#}%");
    }

    private static string ChoiceLabel(Choice choice) => choice switch
    {
        Choice.Rock => "rock",
        Choice.Paper => "paper",
        Choice.Scissors => "scissors",
        _ => "?"
    };

    private static void SafeClear()
    {
        try { Clear(); }
        catch (IOException) { }
    }

    private static void WriteWithColor(string text, ConsoleColor color)
    {
        var original = ForegroundColor;
        try
        {
            ForegroundColor = color;
            WriteLine(text);
        }
        finally
        {
            ForegroundColor = original;
        }
    }
}
