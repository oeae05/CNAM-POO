namespace Models;

/// <summary>
/// Aptitude spéciale d'un vaisseau, utilisée au début de chaque tour,
/// avant que le premier vaisseau ne tire.
/// </summary>
public interface IAbility
{
    /// <param name="spaceships">Ordre de jeu du tour (contient le vaisseau du joueur).
    /// L'aptitude peut modifier cette liste.</param>
    void UseAbility(List<Spaceship> spaceships);
}
