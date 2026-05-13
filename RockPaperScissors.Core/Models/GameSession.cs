namespace RockPaperScissors.Core.Models;

public class GameSession
{
    private readonly Random _rng;
    private readonly List<Round> _history = new();

    public GameSession(Random? rng = null)
    {
        _rng = rng ?? new Random();
    }

    public int PlayerScore { get; private set; }
    public int ComputerScore { get; private set; }
    public int Ties { get; private set; }
    public IReadOnlyList<Round> History => _history;
    public int RoundsPlayed => _history.Count;

    public double WinRate => RoundsPlayed == 0
        ? 0.0
        : (double)PlayerScore / RoundsPlayed * 100.0;

    public Round PlayRound(Choice playerChoice)
    {
        var computerChoice = GameRules.RandomChoice(_rng);
        var outcome = GameRules.DetermineOutcome(playerChoice, computerChoice);

        switch (outcome)
        {
            case Outcome.PlayerWin: PlayerScore++; break;
            case Outcome.ComputerWin: ComputerScore++; break;
            case Outcome.Tie: Ties++; break;
        }

        var round = new Round(playerChoice, computerChoice, outcome);
        _history.Add(round);
        return round;
    }
}
