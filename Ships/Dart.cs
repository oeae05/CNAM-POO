using SpaceInvadersArmory;

namespace Models;

/// <summary>
/// Chasseur cool. Ignore le temps de rechargement de ses armes de type Direct.
/// </summary>
public class Dart : Spaceship
{
    public Dart(Armory armory, Random? random = null)
        : base("Dart", 10, 3, random)
    {
        AddWeapon(armory.CreateWeapon("Laser"));
    }

    protected override bool CanFire(Weapon weapon)
    {
        return base.CanFire(weapon) || weapon.Type == EWeaponType.Direct;
    }
}
