namespace RockPaperScissors.Core.Models;

public static class GameRules
{
    public static Outcome DetermineOutcome(Choice player, Choice computer)
    {
        if (player == computer)
        {
            return Outcome.Tie;
        }

        bool playerWins = (player, computer) switch
        {
            (Choice.Rock, Choice.Scissors) => true,
            (Choice.Scissors, Choice.Paper) => true,
            (Choice.Paper, Choice.Rock) => true,
            _ => false
        };

        return playerWins ? Outcome.PlayerWin : Outcome.ComputerWin;
    }

    public static Choice RandomChoice(Random rng)
    {
        return (Choice)rng.Next(0, 3);
    }
}
