using SpaceInvadersArmory;

namespace Models;

public class Player
{
    private readonly string FirstName;
    private readonly string LastName;
    private string Alias;
    public Spaceship BattleShip { get; }

    /// <param name="armory">Armurerie fournissant les armes du vaisseau par défaut.</param>
    /// <param name="random">Générateur aléatoire du vaisseau (graine fixe pour les tests).</param>
    public Player(string firstName, string lastName, string alias, Armory armory, Random? random = null)
    {
        FirstName = firstName;
        LastName = lastName;
        Alias = alias;
        // Spaceship est abstraite : le vaisseau par défaut est un ViperMKII
        BattleShip = new ViperMKII(armory, random);
    }

    public override string ToString()
    {
        return $"{Alias} ({FirstName} {LastName})";
    }

    public override bool Equals(object? obj)
    {
        return obj is Player other && Alias == other.Alias;
    }

    public override int GetHashCode()
    {
        return Alias.GetHashCode();
    }
}
