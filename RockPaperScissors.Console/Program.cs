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
            WriteLine("=== Stein, Saks, Papir ===\n");
            WriteLine("Velg modus:");
            WriteLine("  1) Uendelig (spill til du avslutter)");
            WriteLine("  2) Best av 5 (først til 3 seire)");
            WriteLine("  3) Best av 7 (først til 4 seire)");
            WriteLine("  4) Avslutt\n");
            Write("Valg: ");

            switch (ReadLine()?.Trim())
            {
                case "1": return int.MaxValue;
                case "2": return 3;
                case "3": return 4;
                case "4": return null;
                default:
                    WriteWithColor("Ugyldig valg. Prøv igjen.", ConsoleColor.Yellow);
                    WriteLine("\nTrykk en tast for å fortsette...");
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
            WriteLine("\n  1) Stein\n  2) Saks\n  3) Papir\n  4) Avslutt\n");
            Write("Valg: ");

            var input = ReadLine()?.Trim();
            if (input == "4") return;

            if (!TryParseChoice(input, out var playerChoice))
            {
                WriteWithColor("Ugyldig valg — bruk 1, 2, 3 eller 4.", ConsoleColor.Yellow);
                WriteLine("\nTrykk en tast for å fortsette...");
                ReadKey(intercept: true);
                continue;
            }

            var round = session.PlayRound(playerChoice);
            ShowRoundResult(round);

            WriteLine("\nTrykk en tast for å fortsette...");
            ReadKey(intercept: true);
        }
    }

    private static bool TryParseChoice(string? input, out Choice choice)
    {
        switch (input)
        {
            case "1": choice = Choice.Rock; return true;
            case "2": choice = Choice.Scissors; return true;
            case "3": choice = Choice.Paper; return true;
            default: choice = default; return false;
        }
    }

    private static void ShowScoreboard(GameSession session, int targetWins)
    {
        var target = targetWins == int.MaxValue ? "uendelig" : $"først til {targetWins}";
        WriteLine($"=== Stein, Saks, Papir ({target}) ===");
        WriteLine($"Runder: {session.RoundsPlayed}   Du: {session.PlayerScore}   PC: {session.ComputerScore}   Uavgjort: {session.Ties}   Win rate: {session.WinRate:0.#}%");
    }

    private static void ShowRoundResult(Round round)
    {
        WriteLine($"\nDu valgte {ChoiceLabel(round.Player)}, PC valgte {ChoiceLabel(round.Computer)}.");

        switch (round.Outcome)
        {
            case Outcome.PlayerWin:
                WriteWithColor("Du vant!", ConsoleColor.Green);
                break;
            case Outcome.ComputerWin:
                WriteWithColor("Du tapte!", ConsoleColor.Red);
                break;
            case Outcome.Tie:
                WriteWithColor("Uavgjort!", ConsoleColor.Yellow);
                break;
        }
    }

    private static void ShowSummary(GameSession session, int targetWins)
    {
        Clear();
        WriteLine("=== Sluttsammendrag ===\n");

        if (session.RoundsPlayed == 0)
        {
            WriteLine("Ingen runder spilt.");
            return;
        }

        if (targetWins != int.MaxValue)
        {
            if (session.PlayerScore >= targetWins)
            {
                WriteWithColor($"Du vant matchen {session.PlayerScore}–{session.ComputerScore}!", ConsoleColor.Green);
            }
            else if (session.ComputerScore >= targetWins)
            {
                WriteWithColor($"PC vant matchen {session.ComputerScore}–{session.PlayerScore}.", ConsoleColor.Red);
            }
        }

        WriteLine();
        WriteLine($"Runder spilt: {session.RoundsPlayed}");
        WriteLine($"Seire:        {session.PlayerScore}");
        WriteLine($"Tap:          {session.ComputerScore}");
        WriteLine($"Uavgjort:     {session.Ties}");
        WriteLine($"Win rate:     {session.WinRate:0.#}%");
    }

    private static string ChoiceLabel(Choice choice) => choice switch
    {
        Choice.Rock => "stein",
        Choice.Scissors => "saks",
        Choice.Paper => "papir",
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
