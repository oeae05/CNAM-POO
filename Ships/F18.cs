namespace Models;

/// <summary>
/// Kamikaze sans arme : s'il est au contact du vaisseau du joueur dans l'ordre de jeu
/// (juste avant ou juste après lui), il explose et lui inflige 10 points de dégâts.
/// </summary>
public class F18 : Spaceship, IAbility
{
    public const double ExplosionDamages = 10;

    public F18(Random? random = null)
        : base("F-18", 15, 0, random)
    {
    }

    public void UseAbility(List<Spaceship> spaceships)
    {
        if (IsDestroyed)
        {
            return;
        }

        int position = spaceships.IndexOf(this);
        Spaceship? player = FindPlayerAt(spaceships, position - 1) ?? FindPlayerAt(spaceships, position + 1);
        if (player == null)
        {
            return;
        }

        Console.WriteLine($"  {Name} percute {player.Name} : \"Salut les mecs c'est moi !\"");
        player.TakeDamages(ExplosionDamages);
        Console.WriteLine($"    -> {player}");
        CurrentStructure = 0; // le F-18 est détruit par son explosion
        Console.WriteLine($"    *** {Name} est détruit ! ***");
    }

    /// <summary>Renvoie le vaisseau du joueur s'il se trouve à cette position, sinon null.</summary>
    private static Spaceship? FindPlayerAt(List<Spaceship> spaceships, int position)
    {
        if (position < 0 || position >= spaceships.Count)
        {
            return null;
        }
        Spaceship ship = spaceships[position];
        return ship.BelongsToPlayer && !ship.IsDestroyed ? ship : null;
    }
}
