namespace Models;

/// <summary>
/// Téléporteur sans arme : déplace un vaisseau au hasard dans l'ordre de jeu
/// (éventuellement celui du joueur, de quoi surprendre un F-18).
/// </summary>
public class Tardis : Spaceship, IAbility
{
    public Tardis(Random? random = null)
        : base("Tardis", 1, 0, random)
    {
    }

    public void UseAbility(List<Spaceship> spaceships)
    {
        if (IsDestroyed || spaceships.Count < 2)
        {
            return;
        }

        int from = random.Next(spaceships.Count);
        Spaceship moved = spaceships[from];
        spaceships.RemoveAt(from);
        int to = random.Next(spaceships.Count + 1);
        spaceships.Insert(to, moved);

        Console.WriteLine($"  {Name} téléporte {moved.Name} de la position {from} à la position {to}.");
    }
}
