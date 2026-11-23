using Models;
using SpaceInvadersArmory;

namespace TP1;

public class SpaceInvaders
{
    // Réparation maximale du bouclier en début de tour
    private const double ShieldRepairPerRound = 2;

    private readonly List<Player> players = new List<Player>();
    private readonly List<Spaceship> enemies = new List<Spaceship>();
    private readonly Armory armory;
    private readonly Random random;
    private int roundNumber;

    public IReadOnlyList<Spaceship> Enemies => enemies;

    /// <summary>Vaisseau du joueur qui combat (le premier joueur).</summary>
    public Spaceship PlayerShip => players[0].BattleShip;

    /// <summary>La partie s'arrête quand le joueur est détruit ou qu'il ne reste plus d'ennemi.</summary>
    public bool IsOver => PlayerShip.IsDestroyed || enemies.All(enemy => enemy.IsDestroyed);

    /// <param name="random">Générateur aléatoire de toute la partie ;
    /// un new Random(graine) rend la partie reproductible.</param>
    public SpaceInvaders(Random? random = null)
    {
        this.random = random ?? Random.Shared;
        armory = new Armory(this.random);
        Init();
    }

    static void Main(string[] args)
    {
        SpaceInvaders game = new SpaceInvaders();
        game.ShowIntro();

        while (!game.IsOver)
        {
            game.PlayRound();
        }

        game.ShowResult();
    }

    private void Init()
    {
        players.Add(new Player("bertin", "oce", "oce", armory, random));
        players.Add(new Player("toussaint", "jeans matth", "jm", armory, random));
        players.Add(new Player("chapuis", "maxime", "max", armory, random));

        // Un ennemi de chaque type
        enemies.Add(new Dart(armory, random));
        enemies.Add(new BWings(armory, random));
        enemies.Add(new Rocinante(armory, random));
        enemies.Add(new F18(random));
        enemies.Add(new Tardis(random));
    }

    private void ShowIntro()
    {
        Console.WriteLine("Joueurs :");
        foreach (Player player in players)
        {
            Console.WriteLine($"  - {player}");
        }
        Console.WriteLine();

        armory.ViewArmory();
        Console.WriteLine();

        PlayerShip.ViewShip();
        Console.WriteLine();

        Console.WriteLine("Ennemis :");
        foreach (Spaceship enemy in enemies)
        {
            Console.WriteLine($"  - {enemy}");
        }
    }

    /// <summary>Joue un tour complet : préparation, aptitudes spéciales, puis tirs.</summary>
    public void PlayRound()
    {
        roundNumber++;
        Console.WriteLine();
        Console.WriteLine($"========== Tour {roundNumber} ==========");

        List<Spaceship> order = BuildPlayOrder();
        Console.WriteLine($"Ordre de jeu : {string.Join(" > ", order.Select(ship => ship.Name))}");

        PrepareShips(order);
        UseAbilities(order);
        ShootAll(order);
    }

    /// <summary>
    /// Ordre de jeu : les ennemis non détruits, avec le vaisseau du joueur
    /// inséré à une position aléatoire (entre 0 et le nombre d'ennemis en vie).
    /// </summary>
    private List<Spaceship> BuildPlayOrder()
    {
        List<Spaceship> order = enemies.Where(enemy => !enemy.IsDestroyed).ToList();
        order.Insert(random.Next(order.Count + 1), PlayerShip);
        return order;
    }

    /// <summary>Chaque vaisseau recharge ses armes et répare jusqu'à 2 points de bouclier.</summary>
    private static void PrepareShips(List<Spaceship> order)
    {
        foreach (Spaceship ship in order)
        {
            ship.ReloadWeapons();
            if (ship.CurrentShield < ship.MaxShield)
            {
                ship.RepairShield(ShieldRepairPerRound);
            }
        }
    }

    /// <summary>
    /// Les vaisseaux dotés d'une aptitude spéciale l'utilisent.
    /// On parcourt une copie : une aptitude (Tardis) peut réordonner la liste.
    /// </summary>
    private static void UseAbilities(List<Spaceship> order)
    {
        foreach (Spaceship ship in order.ToList())
        {
            if (ship is IAbility ability)
            {
                ability.UseAbility(order);
            }
        }
    }

    /// <summary>
    /// Chaque vaisseau encore en vie tire dans l'ordre : les ennemis sur le joueur,
    /// le joueur sur un ennemi non détruit choisi au hasard.
    /// </summary>
    private void ShootAll(List<Spaceship> order)
    {
        foreach (Spaceship ship in order)
        {
            if (IsOver)
            {
                return;
            }
            if (ship.IsDestroyed)
            {
                continue;
            }

            Spaceship target = ship.BelongsToPlayer ? PickRandomEnemy() : PlayerShip;
            ship.ShootTarget(target);
        }
    }

    private Spaceship PickRandomEnemy()
    {
        List<Spaceship> aliveEnemies = enemies.Where(enemy => !enemy.IsDestroyed).ToList();
        return aliveEnemies[random.Next(aliveEnemies.Count)];
    }

    private void ShowResult()
    {
        Console.WriteLine();
        Console.WriteLine($"========== Fin de partie après {roundNumber} tour(s) ==========");
        Console.WriteLine(PlayerShip.IsDestroyed
            ? "Le ViperMKII a été détruit. Les envahisseurs ont gagné..."
            : "Tous les ennemis sont détruits. Victoire !");
    }
}
