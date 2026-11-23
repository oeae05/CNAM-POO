using SpaceInvadersArmory;

namespace TP1.Tests;

public class WeaponTests
{
    [Fact]
    public void CopyConstructor_CreatesIndependentReloadCounter()
    {
        Weapon model = new Weapon("Laser", 2, 3, EWeaponType.Direct, 2);
        Weapon copy = new Weapon(model);

        copy.DecrementReload();

        Assert.NotSame(model, copy);
        Assert.Equal(2, model.TimeBeforeReload);
        Assert.Equal(1, copy.TimeBeforeReload);
    }

    [Fact]
    public void CopyConstructor_DoesNotDoubleExplosiveReloadTwice()
    {
        Weapon model = new Weapon("Hammer", 1, 8, EWeaponType.Explosive, 2);
        Weapon copy = new Weapon(model);

        Assert.Equal(4, model.ReloadTime);
        Assert.Equal(4, copy.ReloadTime);
    }

    [Fact]
    public void Shoot_NotReady_ReturnsZero()
    {
        Weapon weapon = new Weapon("Torpille", 3, 3, EWeaponType.Guided, 2);

        Assert.Equal(0, weapon.Shoot());
    }

    [Fact]
    public void Shoot_IgnoreReload_FiresAndResetsCounter()
    {
        Weapon weapon = new Weapon("Torpille", 3, 3, EWeaponType.Guided, 2);
        weapon.DecrementReload();

        Assert.Equal(3, weapon.Shoot(true));
        Assert.Equal(2, weapon.TimeBeforeReload);
    }

    [Fact]
    public void Shoot_SameSeed_GivesSameResults()
    {
        Weapon first = new Weapon("Laser", 2, 3, EWeaponType.Direct, 0, new Random(42));
        Weapon second = new Weapon("Laser", 2, 3, EWeaponType.Direct, 0, new Random(42));

        for (int i = 0; i < 10; i++)
        {
            Assert.Equal(first.Shoot(), second.Shoot());
        }
    }

    [Fact]
    public void Armory_CreateWeapon_ReturnsCopy()
    {
        Armory armory = new Armory();

        Weapon laser = armory.CreateWeapon("Laser");

        Assert.Equal("Laser", laser.Name);
        Assert.False(armory.Contains(laser));
        Assert.NotSame(laser, armory.CreateWeapon("Laser"));
    }

    [Fact]
    public void Armory_CreateWeapon_UnknownName_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Armory().CreateWeapon("Sabre laser"));
    }
}
