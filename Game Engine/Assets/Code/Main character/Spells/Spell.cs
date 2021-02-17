using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Spell", menuName = "Spells/Spell", order = 0)]
public class Spell : ScriptableObject
{
    [SerializeField] private Projectile projectile;
    [SerializeField] private List<PlayerInput> inputSequence;

    public Projectile Projectile { get => projectile; }

    public PlayerInput GetInputAtStep(int step)
    {
        if (step >= inputSequence.Count) return null;
        return inputSequence[step];
    }
}