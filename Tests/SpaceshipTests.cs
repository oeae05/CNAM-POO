using Models;
using SpaceInvadersArmory;

namespace TP1.Tests;

public class SpaceshipTests
{
    private readonly Armory armory = new Armory();

    [Fact]
    public void Spaceship_IsAbstract()
    {
        Assert.True(typeof(Spaceship).IsAbstract);
    }

    [Fact]
    public void TakeDamages_ShieldAbsorbsFirst()
    {
        Spaceship viper = new ViperMKII(armory); // structure 10, bouclier 15

        viper.TakeDamages(12);

        Assert.Equal(3, viper.CurrentShield);
        Assert.Equal(10, viper.CurrentStructure);
    }

    [Fact]
    public void TakeDamages_OverflowGoesToStructure()
    {
        Spaceship viper = new ViperMKII(armory);

        viper.TakeDamages(20);

        Assert.Equal(0, viper.CurrentShield);
        Assert.Equal(5, viper.CurrentStructure);
    }

    [Fact]
    public void TakeDamages_NeverBelowZero()
    {
        Spaceship viper = new ViperMKII(armory);

        viper.TakeDamages(1000);

        Assert.Equal(0, viper.CurrentShield);
        Assert.Equal(0, viper.CurrentStructure);
        Assert.True(viper.IsDestroyed);
    }

    [Fact]
    public void RepairShield_DoesNotExceedMaxShield()
    {
        Spaceship viper = new ViperMKII(armory);
        viper.TakeDamages(1);

        viper.RepairShield(2);

        Assert.Equal(viper.MaxShield, viper.CurrentShield);
    }

    [Fact]
    public void ReloadWeapons_DecrementsEveryWeapon()
    {
        Spaceship viper = new ViperMKII(armory);
        int[] before = viper.Weapons.Select(weapon => weapon.TimeBeforeReload).ToArray();

        viper.ReloadWeapons();

        for (int i = 0; i < before.Length; i++)
        {
            Assert.Equal(before[i] - 1, viper.Weapons[i].TimeBeforeReload);
        }
    }

    [Fact]
    public void Ships_HaveOwnWeaponInstances()
    {
        Spaceship first = new Dart(armory);
        Spaceship second = new Dart(armory);

        Assert.NotSame(first.Weapons[0], second.Weapons[0]);
        Assert.False(armory.Contains(first.Weapons[0]));
    }

    [Fact]
    public void OnlyViperBelongsToPlayer()
    {
        Assert.True(new ViperMKII(armory).BelongsToPlayer);
        Assert.False(new Dart(armory).BelongsToPlayer);
        Assert.False(new BWings(armory).BelongsToPlayer);
        Assert.False(new Rocinante(armory).BelongsToPlayer);
        Assert.False(new F18().BelongsToPlayer);
        Assert.False(new Tardis().BelongsToPlayer);
    }

    [Fact]
    public void Dart_FiresDirectWeaponEvenIfNotReloaded()
    {
        // Next -> 1 : le tir direct ne rate pas ; NextDouble -> 0 : dégâts minimums (2)
        Armory riggedArmory = new Armory(new SequenceRandom(1));
        Spaceship dart = new Dart(riggedArmory);
        Spaceship target = new BWings(armory); // 30 de structure, pas de bouclier

        Assert.False(dart.Weapons[0].IsReady);
        dart.ShootTarget(target);

        Assert.Equal(28, target.CurrentStructure);
    }

    [Fact]
    public void BWings_FiresExplosiveWeaponEvenIfNotReloaded()
    {
        // Next -> 1 : l'explosif touche ; dégâts minimums 1, doublés = 2
        Armory riggedArmory = new Armory(new SequenceRandom(1));
        Spaceship bWings = new BWings(riggedArmory);
        Spaceship target = new Dart(armory); // bouclier 3

        bWings.ShootTarget(target);

        Assert.Equal(1, target.CurrentShield);
    }

    [Fact]
    public void Rocinante_DodgesWhenRandomSaysSo()
    {
        Spaceship rocinante = new Rocinante(armory, new SequenceRandom(0));

        rocinante.TakeDamages(4);

        Assert.Equal(5, rocinante.CurrentShield);
    }

    [Fact]
    public void Rocinante_TakesDamagesWhenNotDodging()
    {
        Spaceship rocinante = new Rocinante(armory, new SequenceRandom(1));

        rocinante.TakeDamages(4);

        Assert.Equal(1, rocinante.CurrentShield);
    }

    [Fact]
    public void ViperMKII_FiresOnlyOneWeaponPerRound()
    {
        Spaceship viper = new ViperMKII(armory);
        for (int i = 0; i < 4; i++)
        {
            viper.ReloadWeapons(); // les trois armes sont rechargées
        }
        Assert.All(viper.Weapons, weapon => Assert.True(weapon.IsReady));

        viper.ShootTarget(new BWings(armory));

        Assert.Equal(1, viper.Weapons.Count(weapon => !weapon.IsReady));
    }

    [Fact]
    public void ViperMKII_DoesNotAlwaysPickTheSameWeapon()
    {
        // Le choix se fait parmi les armes prêtes : avec Next -> 2, c'est le Missile
        Spaceship viper = new ViperMKII(armory, new SequenceRandom(2));
        for (int i = 0; i < 4; i++)
        {
            viper.ReloadWeapons();
        }

        viper.ShootTarget(new BWings(armory));

        Weapon fired = viper.Weapons.Single(weapon => !weapon.IsReady);
        Assert.Equal("Missile", fired.Name);
    }
}
