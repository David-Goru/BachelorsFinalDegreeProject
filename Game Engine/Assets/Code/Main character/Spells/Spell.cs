using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Spell", menuName = "Spells/Spell", order = 0)]
public class Spell : ScriptableObject
{
    [SerializeField] List<PlayerInput> inputSequence;

    public int StepsAmount { get => inputSequence.Count; }

    public PlayerInput GetInputAtStep(int step)
    {
        return inputSequence[step];
    }
}