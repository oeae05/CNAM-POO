namespace SpaceInvadersArmory;

public class Weapon : IWeapon
{
    // Générateur aléatoire utilisé pour les tirs (injectable pour les tests)
    private readonly Random random;

    public string Name { get; }
    public double MinDamage { get; }
    public double MaxDamage { get; }
    public EWeaponType Type { get; }
    public int ReloadTime { get; }
    public int TimeBeforeReload { get; private set; }

    public bool IsReady => TimeBeforeReload == 0;

    public double AverageDamage => (MinDamage + MaxDamage) / 2;

    /// <param name="random">Générateur aléatoire ; Random.Shared par défaut.
    /// Passer un new Random(graine) rend les tirs reproductibles.</param>
    public Weapon(string name, double minDamage, double maxDamage, EWeaponType type, int reloadTime,
        Random? random = null)
    {
        Name = name;
        MinDamage = minDamage;
        MaxDamage = maxDamage;
        Type = type;
        // Une arme explosive met deux fois plus de temps à recharger
        ReloadTime = type == EWeaponType.Explosive ? reloadTime * 2 : reloadTime;
        TimeBeforeReload = ReloadTime;
        this.random = random ?? Random.Shared;
    }

    /// <summary>
    /// Constructeur de copie : crée une nouvelle arme identique au modèle,
    /// avec son propre compteur de rechargement (diapo 30).
    /// </summary>
    public Weapon(Weapon model)
    {
        Name = model.Name;
        MinDamage = model.MinDamage;
        MaxDamage = model.MaxDamage;
        Type = model.Type;
        // ReloadTime est recopié tel quel : le doublement "Explosive" est déjà appliqué au modèle
        ReloadTime = model.ReloadTime;
        TimeBeforeReload = model.TimeBeforeReload;
        random = model.random;
    }

    public void DecrementReload()
    {
        if (TimeBeforeReload > 0)
        {
            TimeBeforeReload--;
        }
    }

    /// <summary>Tire si l'arme est rechargée, sinon renvoie 0.</summary>
    public double Shoot()
    {
        return Shoot(false);
    }

    /// <summary>
    /// Tire avec l'arme et renvoie les dégâts infligés (0 si raté).
    /// </summary>
    /// <param name="ignoreReload">Vrai pour tirer même si l'arme n'est pas rechargée
    /// (aptitude des Dart et des B-Wings).</param>
    public double Shoot(bool ignoreReload)
    {
        if (!IsReady && !ignoreReload)
        {
            return 0;
        }

        // Après un tir, l'arme doit recharger
        TimeBeforeReload = ReloadTime;

        switch (Type)
        {
            case EWeaponType.Direct:
                // 1 chance sur 10 de rater
                if (random.Next(10) == 0)
                {
                    return 0;
                }
                return RandomDamage();

            case EWeaponType.Explosive:
                // 1 chance sur 4 de rater, dégâts doublés sinon
                if (random.Next(4) == 0)
                {
                    return 0;
                }
                return RandomDamage() * 2;

            case EWeaponType.Guided:
                // Touche toujours, avec les dégâts minimums
                return MinDamage;

            default:
                return 0;
        }
    }

    private double RandomDamage()
    {
        return MinDamage + random.NextDouble() * (MaxDamage - MinDamage);
    }

    public override string ToString()
    {
        return $"{Name} ({Type}) : {MinDamage} - {MaxDamage} dégâts (moy. {AverageDamage}), recharge {ReloadTime} tour(s)";
    }
}
