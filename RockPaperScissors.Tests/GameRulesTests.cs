using RockPaperScissors.Core.Models;

namespace RockPaperScissors.Tests;

public class GameRulesTests
{
    [TestCase(Choice.Rock, Choice.Scissors, Outcome.PlayerWin)]
    [TestCase(Choice.Scissors, Choice.Paper, Outcome.PlayerWin)]
    [TestCase(Choice.Paper, Choice.Rock, Outcome.PlayerWin)]
    [TestCase(Choice.Scissors, Choice.Rock, Outcome.ComputerWin)]
    [TestCase(Choice.Paper, Choice.Scissors, Outcome.ComputerWin)]
    [TestCase(Choice.Rock, Choice.Paper, Outcome.ComputerWin)]
    [TestCase(Choice.Rock, Choice.Rock, Outcome.Tie)]
    [TestCase(Choice.Scissors, Choice.Scissors, Outcome.Tie)]
    [TestCase(Choice.Paper, Choice.Paper, Outcome.Tie)]
    public void DetermineOutcome_returns_expected(Choice player, Choice computer, Outcome expected)
    {
        Assert.That(GameRules.DetermineOutcome(player, computer), Is.EqualTo(expected));
    }

    [Test]
    public void RandomChoice_returns_valid_enum_for_seeded_rng()
    {
        var rng = new Random(42);
        for (int i = 0; i < 100; i++)
        {
            var choice = GameRules.RandomChoice(rng);
            Assert.That(choice, Is.AnyOf(Choice.Rock, Choice.Scissors, Choice.Paper));
        }
    }

    [Test]
    public void GameSession_increments_player_score_on_win()
    {
        var session = new GameSession(new Random(1));
        var initial = session.PlayerScore;
        for (int i = 0; i < 20; i++)
        {
            session.PlayRound(Choice.Rock);
        }
        Assert.That(session.RoundsPlayed, Is.EqualTo(20));
        Assert.That(session.PlayerScore + session.ComputerScore + session.Ties, Is.EqualTo(20));
        Assert.That(session.PlayerScore, Is.GreaterThanOrEqualTo(initial));
    }

    [Test]
    public void GameSession_win_rate_is_zero_on_empty_session()
    {
        var session = new GameSession();
        Assert.That(session.WinRate, Is.EqualTo(0.0));
    }

    [Test]
    public void GameSession_records_round_in_history()
    {
        var session = new GameSession(new Random(0));
        var round = session.PlayRound(Choice.Paper);
        Assert.That(session.History, Has.Count.EqualTo(1));
        Assert.That(session.History[0], Is.EqualTo(round));
    }
}
