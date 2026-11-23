using SpaceInvadersArmory;

namespace Models;

/// <summary>
/// Vaisseau du joueur. N'utilise qu'une seule arme rechargée par tour.
/// </summary>
public class ViperMKII : Spaceship
{
    public override bool BelongsToPlayer => true;

    public ViperMKII(Armory armory, Random? random = null)
        : base("ViperMKII", 10, 15, random)
    {
        AddWeapon(armory.CreateWeapon("Mitrailleuse"));
        AddWeapon(armory.CreateWeapon("EMG"));
        AddWeapon(armory.CreateWeapon("Missile"));
    }

    /// <summary>
    /// Tire avec UNE seule arme rechargée, choisie au hasard parmi les armes prêtes :
    /// la mitrailleuse (rechargée à chaque tour) n'est donc pas toujours choisie.
    /// </summary>
    public override void ShootTarget(Spaceship target)
    {
        List<Weapon> readyWeapons = Weapons.Where(CanFire).ToList();
        List<Weapon> weaponsToFire = readyWeapons.Count == 0
            ? readyWeapons
            : [readyWeapons[random.Next(readyWeapons.Count)]];
        FireAt(target, weaponsToFire);
    }
}
