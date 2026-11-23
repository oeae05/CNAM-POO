using Models;
using SpaceInvadersArmory;

namespace TP1.Tests;

public class AbilityTests
{
    private readonly Armory armory = new Armory();

    [Theory]
    [InlineData(0)] // F-18 juste avant le joueur
    [InlineData(2)] // F-18 juste après le joueur
    public void F18_ExplodesWhenNextToPlayer(int f18Position)
    {
        Spaceship viper = new ViperMKII(armory);
        F18 f18 = new F18();
        List<Spaceship> order = [new Dart(armory), viper, new BWings(armory)];
        order[f18Position] = f18;

        f18.UseAbility(order);

        Assert.True(f18.IsDestroyed);
        Assert.Equal(viper.MaxShield - F18.ExplosionDamages, viper.CurrentShield);
    }

    [Fact]
    public void F18_DoesNothingWhenFarFromPlayer()
    {
        Spaceship viper = new ViperMKII(armory);
        F18 f18 = new F18();
        List<Spaceship> order = [f18, new Dart(armory), viper];

        f18.UseAbility(order);

        Assert.False(f18.IsDestroyed);
        Assert.Equal(viper.MaxShield, viper.CurrentShield);
    }

    [Fact]
    public void Tardis_MovesAShipInTheOrder()
    {
        // 1er Next -> 0 : on retire le vaisseau en position 0 ; 2e Next -> 2 : on le réinsère en position 2
        Tardis tardis = new Tardis(new SequenceRandom(0, 2));
        Spaceship dart = new Dart(armory);
        Spaceship viper = new ViperMKII(armory);
        List<Spaceship> order = [viper, dart, tardis];

        tardis.UseAbility(order);

        Assert.Equal([dart, tardis, viper], order);
    }

    [Fact]
    public void Tardis_CanBringPlayerNextToF18()
    {
        // Le joueur (position 3) est réinséré en position 1, juste après le F-18
        Tardis tardis = new Tardis(new SequenceRandom(3, 1));
        F18 f18 = new F18();
        Spaceship viper = new ViperMKII(armory);
        List<Spaceship> order = [f18, tardis, new Dart(armory), viper];

        tardis.UseAbility(order);
        f18.UseAbility(order);

        Assert.True(f18.IsDestroyed);
        Assert.True(viper.CurrentShield < viper.MaxShield);
    }
}
