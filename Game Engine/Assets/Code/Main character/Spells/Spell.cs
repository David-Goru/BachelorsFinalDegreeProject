using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Spell", menuName = "Spell", order = 0)]
public class Spell : ScriptableObject
{
    [SerializeField]
    List<PlayerInput> inputSequence;
}