namespace TP1.Tests;

/// <summary>
/// Faux générateur aléatoire pour les tests : renvoie en boucle les valeurs données
/// à Next(max) (modulo max), et toujours 0 pour NextDouble (dégâts minimums).
/// Permet de forcer un tir réussi, raté, une esquive, etc.
/// </summary>
public class SequenceRandom : Random
{
    private readonly int[] values;
    private int index;

    public SequenceRandom(params int[] values)
    {
        this.values = values.Length > 0 ? values : [0];
    }

    public override int Next(int maxValue)
    {
        int value = values[index % values.Length] % maxValue;
        index++;
        return value;
    }

    public override double NextDouble()
    {
        return 0;
    }
}
