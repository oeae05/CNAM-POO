using System.Collections.Generic;
using SpaceInvadersArmory;

namespace Models
{
    public interface ISpaceship
    {
        string Name { get; }
        double MaxStructure { get; }
        double MaxShield { get; }
        double CurrentStructure { get; }
        double CurrentShield { get; }
        bool IsDestroyed { get; }
        int MaxWeapons { get; }
        IReadOnlyList<Weapon> Weapons { get; }
        double AverageDamages { get; }
        void AddWeapon(Weapon weapon);
        void RemoveWeapon(Weapon weapon);
        void ClearWeapons();
        void ViewShip();
        void ViewWeapons();

        // Ajouts de la version 2 (étape 0 du TP)

        /// <summary>Vrai si le vaisseau est celui du joueur.</summary>
        bool BelongsToPlayer { get; }

        /// <summary>
        /// Répartit les dégâts d'un tir réussi : le bouclier absorbe en premier,
        /// puis la structure. Ni le bouclier ni la structure ne descendent sous 0.
        /// </summary>
        void TakeDamages(double damages);

        /// <summary>Ajoute des points de bouclier, sans dépasser MaxShield.</summary>
        void RepairShield(double repair);

        /// <summary>
        /// Tire sur le vaisseau cible avec toutes les armes prêtes.
        /// Les classes filles peuvent redéfinir ce comportement.
        /// </summary>
        void ShootTarget(Spaceship target);

        /// <summary>Appelle DecrementReload() sur chaque arme du vaisseau.</summary>
        void ReloadWeapons();
    }
}
