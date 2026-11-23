namespace TP1.Tests;

public class SpaceInvadersTests
{
    // Garde-fou : un test ne doit jamais boucler indéfiniment
    private const int MaxRounds = 1000;

    [Fact]
    public void Init_CreatesOneEnemyOfEachType()
    {
        SpaceInvaders game = new SpaceInvaders(new Random(42));

        Assert.Equal(5, game.Enemies.Count);
        Assert.Equal(5, game.Enemies.Select(enemy => enemy.GetType()).Distinct().Count());
        Assert.True(game.PlayerShip.BelongsToPlayer);
    }

    [Fact]
    public void Game_Ends()
    {
        SpaceInvaders game = new SpaceInvaders(new Random(42));

        PlayUntilOver(game);

        Assert.True(game.IsOver);
    }

    [Fact]
    public void Game_SameSeed_SameOutcome()
    {
        SpaceInvaders first = new SpaceInvaders(new Random(42));
        SpaceInvaders second = new SpaceInvaders(new Random(42));

        PlayUntilOver(first);
        PlayUntilOver(second);

        Assert.Equal(first.PlayerShip.CurrentStructure, second.PlayerShip.CurrentStructure);
        Assert.Equal(
            first.Enemies.Select(enemy => enemy.CurrentStructure),
            second.Enemies.Select(enemy => enemy.CurrentStructure));
    }

    private static void PlayUntilOver(SpaceInvaders game)
    {
        for (int round = 0; round < MaxRounds && !game.IsOver; round++)
        {
            game.PlayRound();
        }
    }
}
