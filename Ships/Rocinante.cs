using SpaceInvadersArmory;

namespace Models;

/// <summary>
/// Bombardier rapide : il a deux fois moins de chances d'être touché.
/// L'arme qui tire ne connaît pas sa cible, c'est donc le Rocinante qui esquive
/// en recevant les dégâts.
/// </summary>
public class Rocinante : Spaceship
{
    public Rocinante(Armory armory, Random? random = null)
        : base("Rocinante", 3, 5, random)
    {
        AddWeapon(armory.CreateWeapon("Torpille"));
    }

    /// <summary>Une chance sur deux d'esquiver les dégâts reçus.</summary>
    public override void TakeDamages(double damages)
    {
        if (random.Next(2) == 0)
        {
            Console.WriteLine($"    {Name} esquive le tir !");
            return;
        }
        base.TakeDamages(damages);
    }
}
