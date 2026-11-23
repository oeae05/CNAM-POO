namespace SpaceInvadersArmory;

public class Armory
{
    private readonly List<Weapon> weapons = new List<Weapon>();
    private readonly Random? random;

    public IReadOnlyList<Weapon> Weapons => weapons;

    /// <param name="random">Générateur transmis aux armes (graine fixe pour les tests).</param>
    public Armory(Random? random = null)
    {
        this.random = random;
        Init();
    }

    private void Init()
    {
        weapons.Add(new Weapon("Laser", 2, 3, EWeaponType.Direct, 2, random));
        weapons.Add(new Weapon("Hammer", 1, 8, EWeaponType.Explosive, 2, random));
        weapons.Add(new Weapon("Torpille", 3, 3, EWeaponType.Guided, 2, random));
        weapons.Add(new Weapon("Mitrailleuse", 6, 8, EWeaponType.Direct, 1, random));
        weapons.Add(new Weapon("EMG", 1, 7, EWeaponType.Explosive, 2, random));
        weapons.Add(new Weapon("Missile", 4, 100, EWeaponType.Guided, 4, random));
    }

    public void AddWeapon(Weapon weapon)
    {
        weapons.Add(weapon);
    }

    public bool RemoveWeapon(Weapon weapon)
    {
        return weapons.Remove(weapon);
    }

    public bool Contains(Weapon weapon)
    {
        return weapons.Contains(weapon);
    }

    /// <summary>
    /// Renvoie une copie de l'arme de l'armurerie portant ce nom.
    /// Chaque vaisseau reçoit ainsi sa propre instance (et son propre compteur de rechargement).
    /// </summary>
    /// <exception cref="ArgumentException">Aucune arme de ce nom dans l'armurerie.</exception>
    public Weapon CreateWeapon(string name)
    {
        Weapon? model = weapons.Find(weapon => weapon.Name == name);
        if (model == null)
        {
            throw new ArgumentException($"L'arme {name} n'existe pas dans l'armurerie.", nameof(name));
        }
        return new Weapon(model);
    }

    public void ViewArmory()
    {
        Console.WriteLine($"Armurerie ({weapons.Count} armes) :");
        foreach (Weapon weapon in weapons)
        {
            Console.WriteLine($"  - {weapon}");
        }
    }
}
