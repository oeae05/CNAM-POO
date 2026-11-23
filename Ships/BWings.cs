using SpaceInvadersArmory;

namespace Models;

/// <summary>
/// B-Wings (le tiret est interdit dans un nom de classe C#) : vaisseau lourd, sans bouclier.
/// Ignore le temps de rechargement de ses armes de type Explosive.
/// </summary>
public class BWings : Spaceship
{
    public BWings(Armory armory, Random? random = null)
        : base("B-Wings", 30, 0, random)
    {
        AddWeapon(armory.CreateWeapon("Hammer"));
    }

    protected override bool CanFire(Weapon weapon)
    {
        return base.CanFire(weapon) || weapon.Type == EWeaponType.Explosive;
    }
}
