using SpaceInvadersArmory;

namespace Models;

/// <summary>
/// Vaisseau générique. La classe est abstraite : on ne peut instancier que ses
/// classes filles (Dart, BWings, Rocinante, ViperMKII, F18, Tardis), mais on
/// les manipule toutes à travers le type Spaceship.
/// </summary>
public abstract class Spaceship : ISpaceship
{
    // Générateur aléatoire partagé avec les classes filles (injectable pour les tests)
    protected readonly Random random;

    public string Name { get; }
    public double MaxStructure { get; }
    public double MaxShield { get; }

    public double CurrentStructure { get; protected set; }
    public double CurrentShield { get; protected set; }

    public bool IsDestroyed => CurrentStructure <= 0;

    /// <summary>Faux par défaut : seul le ViperMKII appartient au joueur.</summary>
    public virtual bool BelongsToPlayer => false;

    private readonly List<Weapon> weapons = new List<Weapon>();
    public IReadOnlyList<Weapon> Weapons => weapons;
    public int MaxWeapons { get; } = 3;
    public double AverageDamages
    {
        get
        {
            double total = 0;
            foreach (Weapon weapon in weapons)
            {
                total += weapon.AverageDamage;
            }
            return total;
        }
    }

    /// <param name="random">Générateur aléatoire ; Random.Shared par défaut.</param>
    protected Spaceship(string name, double maxStructure, double maxShield, Random? random = null)
    {
        Name = name;
        MaxStructure = maxStructure;
        MaxShield = maxShield;
        CurrentStructure = maxStructure;
        CurrentShield = maxShield;
        this.random = random ?? Random.Shared;
    }

    public void AddWeapon(Weapon weapon)
    {
        if (weapons.Count >= MaxWeapons)
        {
            throw new InvalidOperationException($"Le vaisseau {Name} ne peut pas porter plus de {MaxWeapons} armes.");
        }
        weapons.Add(weapon);
    }

    public void RemoveWeapon(Weapon weapon)
    {
        weapons.Remove(weapon);
    }

    public void ClearWeapons()
    {
        weapons.Clear();
    }

    public void ReloadWeapons()
    {
        foreach (Weapon weapon in weapons)
        {
            weapon.DecrementReload();
        }
    }

    public void RepairShield(double repair)
    {
        if (repair < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(repair), "La réparation ne peut pas être négative.");
        }
        CurrentShield = Math.Min(MaxShield, CurrentShield + repair);
    }

    /// <summary>
    /// Le bouclier absorbe les dégâts en premier, le reste est retiré à la structure.
    /// Virtuelle : le Rocinante la redéfinit pour esquiver.
    /// </summary>
    public virtual void TakeDamages(double damages)
    {
        if (damages < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(damages), "Les dégâts ne peuvent pas être négatifs.");
        }
        double absorbedByShield = Math.Min(CurrentShield, damages);
        CurrentShield -= absorbedByShield;
        CurrentStructure = Math.Max(0, CurrentStructure - (damages - absorbedByShield));
    }

    /// <summary>
    /// Comportement par défaut : tire avec toutes les armes utilisables ce tour
    /// (voir CanFire) et inflige le total des dégâts à la cible.
    /// </summary>
    public virtual void ShootTarget(Spaceship target)
    {
        List<Weapon> usableWeapons = weapons.FindAll(CanFire);
        FireAt(target, usableWeapons);
    }

    /// <summary>
    /// Indique si une arme peut tirer ce tour. Par défaut, elle doit être rechargée ;
    /// les Dart et B-Wings l'assouplissent pour leur type d'arme de prédilection.
    /// </summary>
    protected virtual bool CanFire(Weapon weapon)
    {
        return weapon.IsReady;
    }

    /// <summary>
    /// Tire avec les armes données sur la cible, affiche le résultat et applique les dégâts.
    /// Factorise l'affichage commun à toutes les versions de ShootTarget.
    /// </summary>
    protected void FireAt(Spaceship target, IReadOnlyCollection<Weapon> weaponsToFire)
    {
        if (weapons.Count == 0)
        {
            return; // vaisseau désarmé (F-18, Tardis) : rien à faire
        }
        if (weaponsToFire.Count == 0)
        {
            Console.WriteLine($"  {Name} n'a aucune arme prête.");
            return;
        }

        double damages = 0;
        foreach (Weapon weapon in weaponsToFire)
        {
            // CanFire a déjà validé l'arme : on ignore ici son état de rechargement
            damages += weapon.Shoot(true);
        }

        string weaponNames = string.Join(", ", weaponsToFire.Select(weapon => weapon.Name));
        if (damages == 0)
        {
            Console.WriteLine($"  {Name} tire sur {target.Name} ({weaponNames}) et rate sa cible.");
            return;
        }

        Console.WriteLine($"  {Name} tire sur {target.Name} ({weaponNames}) : {damages:0.#} dégâts.");
        target.TakeDamages(damages);
        Console.WriteLine($"    -> {target}");
        if (target.IsDestroyed)
        {
            Console.WriteLine($"    *** {target.Name} est détruit ! ***");
        }
    }

    public void ViewWeapons()
    {
        foreach (Weapon weapon in weapons)
        {
            Console.WriteLine(weapon.ToString());
        }
    }

    public void ViewShip()
    {
        Console.WriteLine($"=== {Name} ===");
        Console.WriteLine($"Structure : {CurrentStructure} / {MaxStructure}");
        Console.WriteLine($"Bouclier  : {CurrentShield} / {MaxShield}");
        Console.WriteLine($"Armes ({weapons.Count} / {MaxWeapons}) :");
        ViewWeapons();
        Console.WriteLine($"Dégâts moyens : {AverageDamages}");
    }

    public override string ToString()
    {
        return $"{Name} [structure {CurrentStructure:0.#}/{MaxStructure}, bouclier {CurrentShield:0.#}/{MaxShield}]";
    }
}
