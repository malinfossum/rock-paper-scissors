# Rock, Paper, Scissors

Console game built in C# / .NET 10. Play against the computer in either endless mode or best-of-N matches.

## Stack

- C# / .NET 10
- NUnit for tests
- MVC-style solution: `Core` (rules), `Console` (IO), `Tests` (unit tests)

## Run

```powershell
dotnet run --project RockPaperScissors.Console
```

## Test

```powershell
dotnet test
```

## How best-of-N works

"Best of 5" means *first to 3 wins*. Ties don't count toward the round limit — they're replayed. Same idea for Best of 7 (first to 4).

## Project layout

```
RockPaperScissors.Core/      # game rules, enums, session state — no IO
RockPaperScissors.Console/   # menu, input, colored output
RockPaperScissors.Tests/     # NUnit tests for the core rules
```
