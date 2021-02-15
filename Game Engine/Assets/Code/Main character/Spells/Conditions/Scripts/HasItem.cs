using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HasItem", menuName = "Spells/Conditions/HasItem", order = 0)]
public class HasItem : ICondition
{
    [SerializeField] private string itemName;
    [SerializeField] private int amount;

    public override bool MeetsCondition()
    {
        Debug.Log("Checking for " + amount + " units of " + itemName);
        return true;
    }
}